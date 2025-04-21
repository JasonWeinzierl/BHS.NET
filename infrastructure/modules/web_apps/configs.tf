resource "azurerm_app_configuration_key" "send_grid_api_key" {
  configuration_store_id = var.app_config_id

  type                = "vault"
  vault_key_reference = azurerm_key_vault_secret.send_grid_api_key.versionless_id

  label = var.environment
  key   = "SendGridClientOptions:ApiKey"
}

resource "azurerm_app_configuration_key" "bhs_db_connstr" {
  configuration_store_id = var.app_config_id

  type                = "vault"
  vault_key_reference = azurerm_key_vault_secret.bhs_db_connstr.versionless_id

  label = var.environment
  key   = "ConnectionStrings:bhsMongo"
}

resource "azurerm_app_configuration_key" "bhs_serilog_mongo_connstr" {
  configuration_store_id = var.app_config_id

  type                = "vault"
  vault_key_reference = azurerm_key_vault_secret.bhs_db_connstr.versionless_id

  label = var.environment
  key   = "Serilog:WriteTo:0:Args:databaseUrl"
}

resource "azurerm_app_configuration_key" "bhs_auth_bearer_authority" {
  configuration_store_id = var.app_config_id

  label = var.environment
  key   = "Authentication:Schemes:Bearer:Authority"
  value = "https://${var.auth_domain}/"
}

resource "azurerm_app_configuration_key" "bhs_auth_bearer_issuer" {
  configuration_store_id = var.app_config_id

  label = var.environment
  key   = "Authentication:Schemes:Bearer:ValidIssuer"
  value = var.auth_domain
}

resource "azurerm_app_configuration_key" "bhs_auth_bearer_audience" {
  configuration_store_id = var.app_config_id

  label = var.environment
  key   = "Authentication:Schemes:Bearer:ValidAudiences:0"
  value = var.api_auth_audience
}

resource "azurerm_app_configuration_key" "auth0_audience" {
  configuration_store_id = var.app_config_id

  label = var.environment
  key   = "AUTH0_AUDIENCE"
  value = var.api_auth_audience
}

resource "azurerm_app_configuration_key" "auth0_client_id" {
  configuration_store_id = var.app_config_id

  label = var.environment
  key   = "AUTH0_CLIENT_ID"
  value = var.spa_auth_client_id
}

resource "azurerm_app_configuration_key" "auth0_domain" {
  configuration_store_id = var.app_config_id

  label = var.environment
  key   = "AUTH0_DOMAIN"
  value = var.auth_domain
}

resource "azurerm_app_configuration_key" "auth0_management_domain" {
  configuration_store_id = var.app_config_id

  label = var.environment
  key   = "Auth0ManagementApiOptions:Domain"
  value = var.auth_domain
}

resource "azurerm_app_configuration_key" "auth0_management_client_id" {
  configuration_store_id = var.app_config_id

  label = var.environment
  key   = "Auth0ManagementApiOptions:ClientId"
  value = var.api_auth_client_id
}

resource "azurerm_app_configuration_key" "auth0_management_client_secret" {
  configuration_store_id = var.app_config_id

  label = var.environment
  key   = "Auth0ManagementApiOptions:ClientSecret"

  type                = "vault"
  vault_key_reference = azurerm_key_vault_secret.auth0_management_client_secret.versionless_id
}
