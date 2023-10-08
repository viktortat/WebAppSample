provider "azurerm" {
  version = "~>2.55"
  features {}
}
# Configure the Azure provider
# terraform {
#   required_providers {
#     azurerm = {
#       source  = "hashicorp/azurerm"
#       version = ">=2.55"
#     }
#   }
#   # required_version = ">= 0.14.9"
# }

# provider "azurerm" {
#   features {}
# }

# ## https://www.terraform.io/docs/providers/azurerm/index.html
# provider "azurerm" {
#   # source  = "hashicorp/azurerm"
#   version = "=2.55.0"
#   # version = "~>2.55"
#   # client_id       =   var.client_id
#   # client_secret   =   var.client_secret
#   subscription_id = var.subscription_id
#   tenant_id       = var.tenant_id
#   features {}
# }

resource "azurerm_resource_group" "rg" {
  name     = "${var.prefix}-rg"
  location = var.location
  tags = {
    Environment = "dev"
    Application = "Azure Compliance"
    AppInstall = "Terraform"
  }
}

resource "azurerm_public_ip" "mypubip" {
  name                = "${var.prefix}-linuxvm-pip"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  allocation_method   = "Static"
  # sku                 = "Basic"
}

resource "azurerm_virtual_network" "main" {
  name                = "${var.prefix}-vnet"
  address_space       = [var.vnet_address_range]
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
}

resource "azurerm_subnet" "internal" {
  name                 = "${var.prefix}-web-subnet"
  resource_group_name  = azurerm_resource_group.rg.name
  virtual_network_name = azurerm_virtual_network.main.name
  address_prefixes     = [var.subnet_address_range]
}

resource "azurerm_network_security_group" "nsg" {
  name                = "${var.prefix}-web-nsg"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  # tags                = var.tags
  security_rule {
    name                       = "HTTP"
    priority                   = 100
    direction                  = "Inbound"
    access                     = "Allow"
    protocol                   = "Tcp"
    source_port_range          = "*"
    destination_port_range     = "80"
    source_address_prefix      = "*"
    destination_address_prefix = "*"
  }
  security_rule {
    name                   = "Allow_SSH"
    priority               = 1000
    direction              = "Inbound"
    access                 = "Allow"
    protocol               = "Tcp"
    source_port_range      = "*"
    destination_port_range = 22
    # source_address_prefix      = "PASTE_YOUR_IP_ADDRESS_HERE"
    source_address_prefix      = "77.143.135.77"
    destination_address_prefix = "*"
  }
}

resource "azurerm_subnet_network_security_group_association" "subnet-nsg" {
  subnet_id                 = azurerm_subnet.internal.id
  network_security_group_id = azurerm_network_security_group.nsg.id
}

resource "azurerm_network_interface" "nic" {
  name                = "${var.prefix}-nic"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  ip_configuration {
    name                          = "${var.prefix}-nic-ipconfig"
    subnet_id                     = azurerm_subnet.internal.id
    private_ip_address_allocation = "Dynamic"
    public_ip_address_id          = azurerm_public_ip.mypubip.id
  }
}

resource "azurerm_linux_virtual_machine" "vm" {
  name                            = "${var.prefix}-linuxvm-dev"
  resource_group_name             = azurerm_resource_group.rg.name
  location                        = azurerm_resource_group.rg.location
  computer_name                   = var.computer_name
  size                            = var.virtual_machine_size
  network_interface_ids           = [azurerm_network_interface.nic.id]
  admin_username                  = "ubuntu"
  # disable_password_authentication = true

  admin_ssh_key {
    username   = "ubuntu"
    public_key = file("~/.ssh/id_rsa.pub")
  }
  
  source_image_reference {
    publisher = "Canonical"
    offer     = "UbuntuServer"
    sku       = "18.04-LTS"
    version   = "latest"
  }

  os_disk {
    storage_account_type = "Standard_LRS"
    caching              = "ReadWrite"
  }
  tags = {
    environment = "testing"
  }

  # provisioner "local-exec" {
  #   # command = "ansible-playbook -i ../ansible/inventory.yml --private-key ${var.ssh_key_path} ../ansible/httpd.yml"
  #   command = "ansible-playbook -i ../inventory.yml ../ansible/utils.yml"
  # }
}

data "template_file" "inventory" {
  template = file("_templates/inventory.tpl")

  vars = {
    user = "ubuntu"
    host = join("", [azurerm_linux_virtual_machine.vm.name, " ansible_host=", azurerm_linux_virtual_machine.vm.public_ip_address])
  }
}

resource "local_file" "save_inventory" {
  content  = data.template_file.inventory.rendered
  filename = "../inventory.yml"
}



