terraform {
  required_version = "~>1.5.7"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>4.26.0"
    }
  }
}

resource "azurerm_storage_account" "bhs" {
  name                = var.storage_account_name
  resource_group_name = var.resource_group_name
  location            = var.location

  account_tier                     = "Standard"
  account_replication_type         = "LRS"
  cross_tenant_replication_enabled = true
}

resource "azurerm_storage_container" "bhs_photos" {
  name               = "photos"
  storage_account_id = azurerm_storage_account.bhs.id

  container_access_type = "blob"
}

resource "azurerm_storage_container" "bhs_videos" {
  name               = "videos"
  storage_account_id = azurerm_storage_account.bhs.id

  container_access_type = "blob"
}
