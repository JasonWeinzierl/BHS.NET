variable "resource_group_name" {
  description = "The Azure resource group."
  type        = string
}

variable "location" {
  description = "The geographic region in Azure."
  type        = string
}

variable "cosmos_account_name" {
  description = "The globally unique name for the cosmos account."
  type        = string
}

variable "enable_free_cosmos" {
  description = "True if the cosmos account should be free. Only one per subscription."
  type        = bool
  default     = false
}