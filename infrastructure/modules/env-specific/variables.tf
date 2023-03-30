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

variable "custom_hostnames" {
  description = "A list of custom hostnames."
  type = list(object({
    name     = string
    hostname = string
  }))
}

variable "enable_free_cosmos" {
  description = "True if the cosmos account should be free. Only one per subscription."
  type        = bool
  default     = false
}
