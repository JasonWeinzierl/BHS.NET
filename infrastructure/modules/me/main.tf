terraform {
  required_version = "~>1.5.7"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>4.26.0"
    }
  }
}

# Roles provisioned for me, the current user.
# In a collaborative environment,
# you would provision service principals instead of doing this.

data "azurerm_client_config" "current" {}

resource "azurerm_role_assignment" "me_admin" { # TODO: rename to me_kv_admin
  scope                = var.key_vault_id
  role_definition_name = "Key Vault Administrator"
  principal_id         = data.azurerm_client_config.current.object_id
}
