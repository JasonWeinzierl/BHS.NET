resource "azurerm_linux_web_app" "bhs_web" {
  name                = var.app_service_name
  resource_group_name = var.resource_group_name
  location            = var.location
  service_plan_id     = azurerm_service_plan.bhs.id

  https_only = true

  site_config {
    always_on                         = false
    ftps_state                        = "Disabled"
    health_check_path                 = "/api/healthcheck/status"
    health_check_eviction_time_in_min = 10
    http2_enabled                     = true

    application_stack {
      dotnet_version = "8.0"
    }
  }

  identity {
    type = "SystemAssigned"
  }

  sticky_settings {
    app_setting_names = [
      "APPINSIGHTS_INSTRUMENTATIONKEY",
      "APPINSIGHTS_PROFILERFEATURE_VERSION",
      "APPINSIGHTS_SNAPSHOTFEATURE_VERSION",
      "APPLICATIONINSIGHTS_CONNECTION_STRING",
      "ApplicationInsightsAgent_EXTENSION_VERSION",
      "DiagnosticServices_EXTENSION_VERSION",
      "InstrumentationEngine_EXTENSION_VERSION",
      "SnapshotDebugger_EXTENSION_VERSION",
      "XDT_MicrosoftApplicationInsights_BaseExtensions",
      "XDT_MicrosoftApplicationInsights_Mode",
      "XDT_MicrosoftApplicationInsights_PreemptSdk",
      "APPLICATIONINSIGHTS_CONFIGURATION_CONTENT",
      "XDT_MicrosoftApplicationInsightsJava",
      "XDT_MicrosoftApplicationInsights_NodeJS",
    ]
  }

  app_settings = {
    ASPNETCORE_ENVIRONMENT = title(var.environment),

    // TODO: figure out if this is all necessary?
    // see https://github.com/Azure/appservice-landing-zone-accelerator/blob/main/scenarios/secure-baseline-multitenant/terraform/modules/spoke/app-service/main.tf
    APPINSIGHTS_INSTRUMENTATIONKEY                  = var.insights_key
    APPINSIGHTS_PROFILERFEATURE_VERSION             = "1.0.0"
    APPINSIGHTS_SNAPSHOTFEATURE_VERSION             = "1.0.0"
    APPLICATIONINSIGHTS_CONNECTION_STRING           = var.insights_conn_str
    ApplicationInsightsAgent_EXTENSION_VERSION      = "~2"
    DiagnosticServices_EXTENSION_VERSION            = "~3"
    InstrumentationEngine_EXTENSION_VERSION         = "disabled" // example has ~1
    SnapshotDebugger_EXTENSION_VERSION              = "disabled" // example has ~1
    XDT_MicrosoftApplicationInsights_BaseExtensions = "disabled" // example has ~1
    XDT_MicrosoftApplicationInsights_Mode           = "recommended"
    XDT_MicrosoftApplicationInsights_PreemptSdk     = "disabled"
  }

  connection_string {
    name  = "AppConfig"
    type  = "Custom"
    value = var.app_config_conn_str
  }

  lifecycle {
    ignore_changes = [
      tags["hidden-link: /app-insights-conn-string"],
      tags["hidden-link: /app-insights-instrumentation-key"],
      tags["hidden-link: /app-insights-resource-id"],
    ]
  }
}

resource "azurerm_role_assignment" "bhs_web_key_vault" {
  scope                = var.key_vault_id
  role_definition_name = "Key Vault Secrets User"
  principal_id         = azurerm_linux_web_app.bhs_web.identity[0].principal_id
}

resource "azurerm_role_assignment" "bhs_github_actions" {
  scope                = azurerm_linux_web_app.bhs_web.id
  role_definition_name = "Website Contributor"
  principal_id         = var.build_server_principal_id
}
