terraform {
  required_version = "~>1.5.7"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>4.26.0"
    }

    sendgrid = {
      source  = "Meuko/sendgrid"
      version = "1.0.5" # Community provider, do not upgrade without inspecting changes.
    }
  }
}

resource "azurerm_service_plan" "bhs" {
  name                = "bhs-${var.environment}-web-asp"
  resource_group_name = var.resource_group_name
  location            = var.location

  os_type  = "Linux"
  sku_name = "B1"
}
