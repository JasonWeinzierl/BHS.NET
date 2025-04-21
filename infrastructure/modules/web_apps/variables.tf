variable "environment" {
  description = "The non-abbreviated environment name."
  type        = string
}

variable "location" {
  description = "The geographic region in Azure."
  type        = string
}

variable "resource_group_name" {
  description = "The Azure resource group name."
  type        = string
}

variable "insights_conn_str" {
  description = "The connection string to the Application Insights instance."
  type        = string
  sensitive   = true
}

variable "insights_key" {
  description = "The instrumentation key for the Application Insights instance."
  type        = string
  sensitive   = true
}

variable "app_service_name" {
  description = "The globally unique name for the web app."
  type        = string
}

variable "app_config_id" {
  description = "The ID of the Azure App Configuration instance."
  type        = string
}

variable "app_config_conn_str" {
  description = "The connection string to the App Configuration instance."
  type        = string
  sensitive   = true
}

variable "key_vault_id" {
  description = "The ID of the Azure Key Vault instance."
  type        = string
}

variable "build_server_principal_id" {
  description = "The object ID of the build server's service principal."
  type        = string
}

variable "mongo_connection_string" {
  description = "The connection string to the MongoDB instance."
  type        = string
  sensitive   = true
}

variable "mongo_database_name" {
  description = "The name of the MongoDB database."
  type        = string
}

variable "auth_domain" {
  description = "The domain of the authentication server."
  type        = string
}

variable "api_auth_audience" {
  description = "The authentication audience of the API."
  type        = string
}

variable "spa_auth_client_id" {
  description = "The authentication client ID for the SPA."
  type        = string
}

variable "api_auth_client_id" {
  description = "The authentication client ID for the API."
  type        = string
}

variable "api_auth_client_secret" {
  description = "The authentication client secret for the API."
  type        = string
  sensitive   = true
}

