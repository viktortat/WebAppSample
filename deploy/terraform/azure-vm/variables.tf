variable "prefix" {
  description = "The Prefix used for all resources in this example"
  default     = "svrtest3"
}

variable "subscription_id" {
  description = "Subscription ID"
  type        = string
  default     = "13af85d4-24f0-4580-8f7b-77777777777"
}

variable "tenant_id" {
  description = "Tenant ID"
  type        = string
  default     = "8c5bdf25-9bff-4600-9553-77777777777"
}

variable "location" {
  description = "The Azure Region in which all resources in this example should be created."
  default     = "West Europe"
}
variable "virtual_machine_size" {
  description = "Size of the VM"
  type        = string
  default     = "Standard_B1s"
}
variable "os_disk_size_gb" {
  default = 30
}
variable "computer_name" {
  description = "Computer name"
  type        = string
  default     = "SvrLinuxvm"
}
variable "vnet_address_range" {
  description = "IP Range of the virtual network"
  type        = string
  default     = "10.0.0.0/16"
}

variable "subnet_address_range" {
  description = "IP Range of the virtual network"
  type        = string
  default     = "10.0.1.0/24"
}
variable "tags" {
  description = "Resouce tags"
  type        = map(string)
  default = {
    "project"       = "Svrtest"
    "deployed_with" = "Terraform"
  }
}
