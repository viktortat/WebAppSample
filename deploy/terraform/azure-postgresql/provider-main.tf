###########################
## Azure Provider - Main ##
###########################

# Define Terraform provider
terraform {
  required_version = ">= 0.13"
}

# Configure the Azure provider
provider "azurerm" {
  environment = "public"
  version     = ">= 2.25.0"
  features {}
  # client_id       = var.azure-client-id
  # client_secret   = var.azure-client-secret
  subscription_id = var.azure-subscription-id
  tenant_id       = var.azure-tenant-id
}
