terraform {
  required_version = "~>1.5.7"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>4.26.0"
    }
  }
}

resource "azurerm_log_analytics_workspace" "bhs" {
  name                = "bhs-${var.environment}-web-log"
  resource_group_name = var.resource_group_name
  location            = var.location
}

resource "azurerm_application_insights" "bhs" {
  name                = "bhs-${var.environment}-web-insights"
  resource_group_name = var.resource_group_name
  location            = var.location
  workspace_id        = azurerm_log_analytics_workspace.bhs.id

  application_type    = "web"
  sampling_percentage = 0
}
