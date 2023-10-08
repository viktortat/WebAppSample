################################
## Azure Provider - Variables ##
################################

# Azure authentication variables
variable "azure-tenant-id" {
  type        = string
  description = "Azure Tenant ID"
  default     = "8c5bdf25-9bff-4600-9553-877777777777"
}
variable "azure-subscription-id" {
  type        = string
  description = "Azure Subscription ID"
  default     = "13af85d4-24f0-4580-8f7b-877777777777"
}

# variable "azure-client-id" {
#   type        = string
#   description = "Azure Client ID"
# }

# variable "azure-client-secret" {
#   type        = string
#   description = "Azure Client Secret"
# }
