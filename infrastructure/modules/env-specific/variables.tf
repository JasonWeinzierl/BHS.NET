variable "environment" {
  description = "The non-abbreviated environment name."
  type        = string
}

variable "location" {
  description = "The geographic region in Azure."
  type        = string
}

variable "auto_deploy" {
  description = "True if the environment should deploy without a reviewer."
  type        = bool
}

variable "subdomain" {
  description = "The subdomain binding to be created for the web app."
  type        = string
}

variable "storage_account_name" {
  description = "The globally unique name for the storage account."
  type        = string
}

variable "app_service_name" {
  description = "The globally unique name for the web app."
  type        = string
}

variable "enable_root_binding" {
  description = "True if a binding for the root hostname should be created."
  type        = bool
  default     = false
}

variable "enable_free_cosmos" {
  description = "True if the cosmos account should be free. Only one per subscription."
  type        = bool
  default     = false
}
