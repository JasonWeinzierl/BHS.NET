terraform {
  required_version = "~>1.5.7"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>4.26.0"
    }
  }
}

data "azurerm_client_config" "current" {}

resource "azurerm_key_vault" "bhs" {
  name                = "bhs-${var.environment}-secrets"
  resource_group_name = var.resource_group_name
  location            = var.location
  tenant_id           = data.azurerm_client_config.current.tenant_id

  enable_rbac_authorization = true
  sku_name                  = "standard"
}
