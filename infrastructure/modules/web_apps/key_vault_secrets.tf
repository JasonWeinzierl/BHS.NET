# TODO: import sendgrid domain authentication to use in dns records.
resource "sendgrid_api_key" "bhs_mail_send" {
  name = "bhs-${var.environment}-web-mailsend"
  scopes = [
    "2fa_required",
    "mail.send",
    "sender_verification_eligible",
    "whitelabel.create",
    "whitelabel.delete",
    "whitelabel.read",
    "whitelabel.update",
  ]
}

resource "azurerm_key_vault_secret" "send_grid_api_key" {
  key_vault_id = var.key_vault_id

  name  = "send-grid-api-key"
  value = sendgrid_api_key.bhs_mail_send.api_key
}

resource "azurerm_key_vault_secret" "bhs_db_connstr" {
  key_vault_id = var.key_vault_id

  name  = "connection-strings-bhs-mongo"
  value = replace(var.mongo_connection_string, "/?", "/${var.mongo_database_name}?")
}

resource "azurerm_key_vault_secret" "auth0_management_client_secret" {
  key_vault_id = var.key_vault_id

  name  = "auth0-management-client-secret"
  value = var.api_auth_client_secret
}
