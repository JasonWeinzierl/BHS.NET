terraform {
  required_version = "~>1.5.7"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>4.26.0"
    }
  }
}

resource "azurerm_app_service_custom_hostname_binding" "this" {
  hostname            = var.hostname
  resource_group_name = var.resource_group_name
  app_service_name    = var.app_service_name

  # These are managed through the certificate bindings.
  lifecycle {
    ignore_changes = [
      ssl_state,
      thumbprint,
    ]
  }
}

resource "azurerm_app_service_managed_certificate" "this" {
  custom_hostname_binding_id = azurerm_app_service_custom_hostname_binding.this.id
}

resource "azurerm_app_service_certificate_binding" "this" {
  hostname_binding_id = azurerm_app_service_custom_hostname_binding.this.id
  certificate_id      = azurerm_app_service_managed_certificate.this.id
  ssl_state           = "SniEnabled"
}
