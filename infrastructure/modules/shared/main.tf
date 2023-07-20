terraform {
  required_version = "~>1.5.3"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>3.65.0"
    }
  }

  backend "azurerm" {
    resource_group_name  = "bhs-production-tfstate-rg"
    storage_account_name = "tfstate1ee5v"
    container_name       = "tfstate"
    key                  = "shared.tfstate"
  }
}

provider "azurerm" {
  features {}
}

data "azurerm_client_config" "current" {}

resource "azurerm_resource_group" "bhs_shared" {
  name     = "bhs-shared-web-rg"
  location = "centralus"
}

resource "azurerm_app_configuration" "bhs" {
  name                = "bhs-shared-web-appcs"
  resource_group_name = azurerm_resource_group.bhs_shared.name
  location            = azurerm_resource_group.bhs_shared.location

  sku = "free"
}

# Provisioning keys requires this role on the appcs or one of its parents.
resource "azurerm_role_assignment" "bhs_appcs_dataowner" {
  scope                = azurerm_app_configuration.bhs.id
  role_definition_name = "App Configuration Data Owner"
  principal_id         = data.azurerm_client_config.current.object_id
}
