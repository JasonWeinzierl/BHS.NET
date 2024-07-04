
locals {
  subdomain_hostname = "https://${var.subdomain}.beltonhistoricalsociety.org"
  hostnames          = var.enable_root_binding ? ["https://beltonhistoricalsociety.org", local.subdomain_hostname] : [local.subdomain_hostname]
}


data "github_repository" "bhs" {
  full_name = "JasonWeinzierl/BHS.NET"
}

resource "github_repository_environment" "this" {
  environment = var.environment
  repository  = data.github_repository.bhs.name

  deployment_branch_policy {
    protected_branches     = true
    custom_branch_policies = false
  }
}

resource "github_actions_environment_secret" "azure_client_id" {
  repository      = data.github_repository.bhs.name
  environment     = github_repository_environment.this.environment
  secret_name     = "AZURE_CLIENT_ID"
  plaintext_value = azuread_application.bhs_github_actions.client_id
}

resource "github_actions_environment_secret" "azure_client_secret" {
  repository      = data.github_repository.bhs.name
  environment     = github_repository_environment.this.environment
  secret_name     = "AZURE_CLIENT_SECRET"
  plaintext_value = azuread_service_principal_password.bhs_github_actions.value
}

resource "github_actions_environment_secret" "azure_subscription_id" {
  repository      = data.github_repository.bhs.name
  environment     = github_repository_environment.this.environment
  secret_name     = "AZURE_SUBSCRIPTION_ID"
  plaintext_value = data.azurerm_client_config.current.subscription_id
}

resource "github_actions_environment_secret" "azure_tenant_id" {
  repository      = data.github_repository.bhs.name
  environment     = github_repository_environment.this.environment
  secret_name     = "AZURE_TENANT_ID"
  plaintext_value = data.azurerm_client_config.current.tenant_id
}

resource "github_actions_environment_variable" "cypress_auth0_domain" {
  repository    = data.github_repository.bhs.name
  environment   = github_repository_environment.this.environment
  variable_name = "CYPRESS_AUTH0_DOMAIN"
  value         = data.auth0_tenant.bhs.domain
}

resource "github_actions_environment_variable" "cypress_auth0_client_id" {
  repository    = data.github_repository.bhs.name
  environment   = github_repository_environment.this.environment
  variable_name = "CYPRESS_AUTH0_CLIENT_ID"
  value         = auth0_client.bhs_spa.client_id
}

resource "github_actions_environment_variable" "cypress_auth0_audience" {
  repository    = data.github_repository.bhs.name
  environment   = github_repository_environment.this.environment
  variable_name = "CYPRESS_AUTH0_AUDIENCE"
  value         = auth0_resource_server.bhs_api.identifier
}

resource "github_actions_environment_variable" "cypress_auth0_test_username" {
  repository    = data.github_repository.bhs.name
  environment   = github_repository_environment.this.environment
  variable_name = "CYPRESS_AUTH0_TEST_USERNAME"
  value         = auth0_user.noreply.email
}

resource "github_actions_environment_secret" "cypress_auth0_test_password" {
  repository      = data.github_repository.bhs.name
  environment     = github_repository_environment.this.environment
  secret_name     = "CYPRESS_AUTH0_TEST_PASSWORD"
  plaintext_value = auth0_user.noreply.password
}

resource "github_actions_environment_variable" "cypress_base_url" {
  repository    = data.github_repository.bhs.name
  environment   = github_repository_environment.this.environment
  variable_name = "CYPRESS_BASE_URL"
  value         = local.hostnames[0]
}


data "azuread_client_config" "current" {}

resource "azuread_application" "bhs_github_actions" {
  display_name = "bhs-${var.environment}-buildserver-adapp"
  owners = [
    data.azuread_client_config.current.object_id,
  ]

  required_resource_access {
    resource_app_id = "00000003-0000-0000-c000-000000000000" # Microsoft Graph

    resource_access {
      id   = "1bfefb4e-e0b5-418b-a88f-73c46d2cc8e9" # Application.ReadWrite.All
      type = "Role"
    }
  }
}

resource "azuread_service_principal" "bhs_github_actions" {
  client_id                    = azuread_application.bhs_github_actions.client_id
  app_role_assignment_required = false
  owners = [
    data.azuread_client_config.current.object_id,
  ]
}

resource "azuread_service_principal_password" "bhs_github_actions" {
  service_principal_id = azuread_service_principal.bhs_github_actions.object_id
}

resource "azurerm_role_assignment" "bhs_github_actions" {
  scope                = azurerm_linux_web_app.bhs_web.id
  role_definition_name = "Website Contributor"
  principal_id         = azuread_service_principal.bhs_github_actions.object_id
}


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


data "azurerm_client_config" "current" {}

data "azurerm_resource_group" "bhs_shared" {
  name = "bhs-shared-web-rg"
}

resource "azurerm_resource_group" "bhs" {
  name     = "bhs-${var.environment}-web-rg"
  location = var.location
}


resource "azurerm_cosmosdb_account" "bhs_db" {
  name                = var.cosmos_account_name
  resource_group_name = azurerm_resource_group.bhs.name
  location            = azurerm_resource_group.bhs.location

  offer_type           = "Standard"
  kind                 = "MongoDB"
  mongo_server_version = "4.2"
  enable_free_tier     = var.enable_free_cosmos

  ip_range_filter = "97.135.189.47,104.42.195.92,40.76.54.131,52.176.6.30,52.169.50.45,52.187.184.26,40.80.152.199,13.95.130.121,20.245.81.54,40.118.23.126,0.0.0.0"

  consistency_policy {
    consistency_level = "Session"
  }

  geo_location {
    failover_priority = 0
    location          = azurerm_resource_group.bhs.location
  }

  capabilities {
    name = "EnableMongo"
  }

  dynamic "capabilities" {
    for_each = var.enable_free_cosmos ? [] : [1]
    content {
      name = "EnableServerless"
    }
  }

  dynamic "capabilities" {
    for_each = var.enable_free_cosmos ? [] : [1]
    content {
      name = "DisableRateLimitingResponses"
    }
  }

  dynamic "capacity" {
    for_each = var.enable_free_cosmos ? [] : [1]
    content {
      total_throughput_limit = 4000
    }
  }
}

resource "azurerm_cosmosdb_mongo_database" "bhs_db" {
  name                = "bhs"
  resource_group_name = azurerm_resource_group.bhs.name
  account_name        = azurerm_cosmosdb_account.bhs_db.name

  # Free accounts get 1000 RU/s of provisioned throughput.
  # Otherwise, we're using serverless.
  dynamic "autoscale_settings" {
    for_each = var.enable_free_cosmos ? [1] : []
    content {
      max_throughput = 1000
    }
  }
}


resource "azurerm_key_vault" "bhs" {
  name                = "bhs-${var.environment}-secrets"
  resource_group_name = azurerm_resource_group.bhs.name
  location            = azurerm_resource_group.bhs.location
  tenant_id           = data.azurerm_client_config.current.tenant_id

  enable_rbac_authorization = true
  sku_name                  = "standard"
}

resource "azurerm_role_assignment" "me_admin" {
  scope                = azurerm_key_vault.bhs.id
  role_definition_name = "Key Vault Administrator"
  principal_id         = data.azuread_client_config.current.object_id
}

resource "azurerm_role_assignment" "bhs_web_key_vault" {
  scope                = azurerm_key_vault.bhs.id
  role_definition_name = "Key Vault Secrets User"
  principal_id         = azurerm_linux_web_app.bhs_web.identity[0].principal_id
}

resource "azurerm_key_vault_secret" "send_grid_api_key" {
  key_vault_id = azurerm_key_vault.bhs.id

  name  = "send-grid-api-key"
  value = sendgrid_api_key.bhs_mail_send.api_key

  depends_on = [
    azurerm_role_assignment.me_admin,
  ]
}

resource "azurerm_key_vault_secret" "bhs_db_connstr" {
  key_vault_id = azurerm_key_vault.bhs.id

  name  = "connection-strings-bhs-mongo"
  value = replace(azurerm_cosmosdb_account.bhs_db.connection_strings[0], "/?", "/${azurerm_cosmosdb_mongo_database.bhs_db.name}?")

  depends_on = [
    azurerm_role_assignment.me_admin,
  ]
}

resource "azurerm_key_vault_secret" "auth0_management_client_secret" {
  key_vault_id = azurerm_key_vault.bhs.id

  name  = "auth0-management-client-secret"
  value = auth0_client_credentials.bhs_api.client_secret

  depends_on = [
    azurerm_role_assignment.me_admin,
  ]
}

data "azurerm_app_configuration" "bhs" {
  name                = "bhs-shared-web-appcs"
  resource_group_name = data.azurerm_resource_group.bhs_shared.name
}

resource "azurerm_app_configuration_key" "send_grid_api_key" {
  configuration_store_id = data.azurerm_app_configuration.bhs.id

  type                = "vault"
  vault_key_reference = azurerm_key_vault_secret.send_grid_api_key.versionless_id

  label = var.environment
  key   = "SendGridClientOptions:ApiKey"
}

resource "azurerm_app_configuration_key" "bhs_db_connstr" {
  configuration_store_id = data.azurerm_app_configuration.bhs.id

  type                = "vault"
  vault_key_reference = azurerm_key_vault_secret.bhs_db_connstr.versionless_id

  label = var.environment
  key   = "ConnectionStrings:bhsMongo"
}

resource "azurerm_app_configuration_key" "bhs_serilog_mongo_connstr" {
  configuration_store_id = data.azurerm_app_configuration.bhs.id

  type                = "vault"
  vault_key_reference = azurerm_key_vault_secret.bhs_db_connstr.versionless_id

  label = var.environment
  key   = "Serilog:WriteTo:0:Args:databaseUrl"
}

resource "azurerm_app_configuration_key" "bhs_auth_bearer_authority" {
  configuration_store_id = data.azurerm_app_configuration.bhs.id

  label = var.environment
  key   = "Authentication:Schemes:Bearer:Authority"
  value = "https://${data.auth0_tenant.bhs.domain}/"
}

resource "azurerm_app_configuration_key" "bhs_auth_bearer_issuer" {
  configuration_store_id = data.azurerm_app_configuration.bhs.id

  label = var.environment
  key   = "Authentication:Schemes:Bearer:ValidIssuer"
  value = data.auth0_tenant.bhs.domain
}

resource "azurerm_app_configuration_key" "bhs_auth_bearer_audience" {
  configuration_store_id = data.azurerm_app_configuration.bhs.id

  label = var.environment
  key   = "Authentication:Schemes:Bearer:ValidAudiences:0"
  value = auth0_resource_server.bhs_api.identifier
}

resource "azurerm_app_configuration_key" "auth0_audience" {
  configuration_store_id = data.azurerm_app_configuration.bhs.id

  label = var.environment
  key   = "AUTH0_AUDIENCE"
  value = auth0_resource_server.bhs_api.identifier
}

resource "azurerm_app_configuration_key" "auth0_client_id" {
  configuration_store_id = data.azurerm_app_configuration.bhs.id

  label = var.environment
  key   = "AUTH0_CLIENT_ID"
  value = auth0_client.bhs_spa.client_id
}

resource "azurerm_app_configuration_key" "auth0_domain" {
  configuration_store_id = data.azurerm_app_configuration.bhs.id

  label = var.environment
  key   = "AUTH0_DOMAIN"
  value = data.auth0_tenant.bhs.domain
}

resource "azurerm_app_configuration_key" "auth0_management_domain" {
  configuration_store_id = data.azurerm_app_configuration.bhs.id

  label = var.environment
  key   = "Auth0ManagementApiOptions:Domain"
  value = data.auth0_tenant.bhs.domain
}

resource "azurerm_app_configuration_key" "auth0_management_client_id" {
  configuration_store_id = data.azurerm_app_configuration.bhs.id

  label = var.environment
  key   = "Auth0ManagementApiOptions:ClientId"
  value = auth0_client_credentials.bhs_api.client_id
}

resource "azurerm_app_configuration_key" "auth0_management_client_secret" {
  configuration_store_id = data.azurerm_app_configuration.bhs.id

  label = var.environment
  key   = "Auth0ManagementApiOptions:ClientSecret"

  type                = "vault"
  vault_key_reference = azurerm_key_vault_secret.auth0_management_client_secret.versionless_id
}


resource "azurerm_storage_account" "bhs" {
  name                = var.storage_account_name
  resource_group_name = azurerm_resource_group.bhs.name
  location            = azurerm_resource_group.bhs.location

  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "bhs_photos" {
  name                 = "photos"
  storage_account_name = azurerm_storage_account.bhs.name

  container_access_type = "blob"
}

resource "azurerm_storage_container" "bhs_videos" {
  name                 = "videos"
  storage_account_name = azurerm_storage_account.bhs.name

  container_access_type = "blob"
}


resource "azurerm_log_analytics_workspace" "bhs" {
  name                = "bhs-${var.environment}-web-log"
  resource_group_name = azurerm_resource_group.bhs.name
  location            = azurerm_resource_group.bhs.location
}

resource "azurerm_application_insights" "bhs" {
  name                = "bhs-${var.environment}-web-insights"
  resource_group_name = azurerm_resource_group.bhs.name
  location            = azurerm_resource_group.bhs.location
  workspace_id        = azurerm_log_analytics_workspace.bhs.id

  application_type    = "web"
  sampling_percentage = 0
}


resource "azurerm_service_plan" "bhs" {
  name                = "bhs-${var.environment}-web-asp"
  resource_group_name = azurerm_resource_group.bhs.name
  location            = azurerm_resource_group.bhs.location

  os_type  = "Linux"
  sku_name = "B1"
}

resource "azurerm_linux_web_app" "bhs_web" {
  name                = var.app_service_name
  resource_group_name = azurerm_resource_group.bhs.name
  location            = azurerm_resource_group.bhs.location
  service_plan_id     = azurerm_service_plan.bhs.id

  https_only = true

  site_config {
    always_on         = false
    ftps_state        = "Disabled"
    health_check_path = "/api/healthcheck/status"
    http2_enabled     = true

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
    APPINSIGHTS_INSTRUMENTATIONKEY                  = azurerm_application_insights.bhs.instrumentation_key
    APPINSIGHTS_PROFILERFEATURE_VERSION             = "1.0.0"
    APPINSIGHTS_SNAPSHOTFEATURE_VERSION             = "1.0.0"
    APPLICATIONINSIGHTS_CONNECTION_STRING           = azurerm_application_insights.bhs.connection_string
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
    value = data.azurerm_app_configuration.bhs.primary_read_key[0].connection_string
  }
}

module "bhs_hostname_root" {
  count = var.enable_root_binding ? 1 : 0

  source = "../custom-hostname"

  hostname            = "beltonhistoricalsociety.org"
  resource_group_name = azurerm_resource_group.bhs.name
  app_service_name    = azurerm_linux_web_app.bhs_web.name
}

module "bhs_hostname_subdomain" {
  source = "../custom-hostname"

  hostname            = "${var.subdomain}.beltonhistoricalsociety.org"
  resource_group_name = azurerm_resource_group.bhs.name
  app_service_name    = azurerm_linux_web_app.bhs_web.name

  depends_on = [
    namecheap_domain_records.subdomain_beltonhistoricalsociety_org, # NOTE: This may still fail to create until the records have propagated.
  ]
}


data "auth0_tenant" "bhs" {}
data "auth0_client" "terraform" {
  name = "Terraform Provider Auth0"
}

resource "auth0_client" "bhs_spa" {
  name        = "BHS Angular Client"
  description = ""
  app_type    = "spa"

  allowed_logout_urls = local.hostnames
  callbacks           = local.hostnames
  web_origins         = local.hostnames
  logo_uri            = "https://beltonhistoricalsociety.org/assets/img/2019/icons/apple-touch-icon.png"

  is_first_party       = true
  oidc_conformant      = true
  custom_login_page_on = true

  jwt_configuration {
    alg                 = "RS256"
    lifetime_in_seconds = 36000
    secret_encoded      = false
  }

  refresh_token {
    expiration_type              = "non-expiring"
    rotation_type                = "non-rotating"
    leeway                       = 0
    infinite_token_lifetime      = true
    infinite_idle_token_lifetime = true
  }
}

resource "auth0_client_credentials" "bhs_spa" {
  client_id = auth0_client.bhs_spa.id

  authentication_method = "none"
}

resource "auth0_client" "bhs_api" {
  name        = "BHS .NET Client"
  description = ""
  app_type    = "non_interactive"

  is_first_party       = true
  oidc_conformant      = true
  custom_login_page_on = true

  jwt_configuration {
    alg                 = "RS256"
    lifetime_in_seconds = 36000
    secret_encoded      = false
  }

  refresh_token {
    expiration_type              = "non-expiring"
    rotation_type                = "non-rotating"
    leeway                       = 0
    infinite_token_lifetime      = true
    infinite_idle_token_lifetime = true
  }

  grant_types = [
    "client_credentials",
  ]
}

resource "auth0_client_credentials" "bhs_api" {
  client_id = auth0_client.bhs_api.id

  authentication_method = "client_secret_post"
}

resource "auth0_client_grant" "bhs_api_auth0_management" {
  client_id = auth0_client.bhs_api.id
  audience  = data.auth0_tenant.bhs.management_api_identifier

  scopes = [
    "read:users",
  ]
}

resource "auth0_resource_server" "bhs_api" {
  name        = "BHS .NET API"
  identifier  = "https://beltonhistoricalsociety.org/api/swagger/index.html"
  signing_alg = "RS256"

  enforce_policies                                = true
  token_dialect                                   = "access_token_authz"
  skip_consent_for_verifiable_first_party_clients = true
}

resource "auth0_resource_server_scopes" "bhs_api_scopes" {
  resource_server_identifier = auth0_resource_server.bhs_api.identifier

  scopes {
    name        = "write:blog"
    description = "Author blog resources"
  }

  scopes {
    name        = "write:museum"
    description = "Author museum resources"
  }
}

resource "auth0_role" "owner" {
  name        = "Owner"
  description = "Full access to all resources"
}

resource "auth0_role_permissions" "owner_permissions" {
  role_id = auth0_role.owner.id

  permissions {
    resource_server_identifier = auth0_resource_server.bhs_api.identifier
    name                       = "write:blog"
  }

  permissions {
    resource_server_identifier = auth0_resource_server.bhs_api.identifier
    name                       = "write:museum"
  }

  depends_on = [
    auth0_resource_server_scopes.bhs_api_scopes
  ]
}

resource "auth0_connection" "bhs_userpassauth" {
  name     = "Username-Password-Authentication" # WARNING: Auth0 automatically creates a database connection when a client is created. You must manually delete that connection in the UI.
  strategy = "auth0"

  options {
    disable_signup         = true
    brute_force_protection = true

    mfa {
      active                 = true
      return_enroll_settings = true
    }

    password_policy = "excellent"
    password_complexity_options {
      min_length = 8
    }
    password_history {
      size   = 5
      enable = true
    }
    password_dictionary {
      enable = true
    }
    password_no_personal_info {
      enable = true
    }
  }
}

resource "auth0_connection_client" "bhs_userpassauth_spa_assoc" {
  connection_id = auth0_connection.bhs_userpassauth.id
  client_id     = auth0_client.bhs_spa.id
}

resource "auth0_connection_client" "bhs_userpassauth_terraform_assoc" {
  connection_id = auth0_connection.bhs_userpassauth.id
  client_id     = data.auth0_client.terraform.id
}

resource "random_password" "auth0_noreply_password" {
  length  = 16
  special = true
}

resource "auth0_user" "noreply" {
  connection_name = auth0_connection.bhs_userpassauth.name

  email          = "noreply@beltonhistoricalsociety.org"
  email_verified = true
  password       = random_password.auth0_noreply_password.result

  depends_on = [
    auth0_connection_client.bhs_userpassauth_terraform_assoc,
  ]
}


# TODO: set up the A record for the root domain, if enable_root_binding is true.
# TODO: move this to custom-hostname module?

resource "namecheap_domain_records" "subdomain_beltonhistoricalsociety_org" {
  domain = "beltonhistoricalsociety.org"
  mode   = "MERGE"

  record {
    hostname = var.subdomain
    address  = "${azurerm_linux_web_app.bhs_web.name}.azurewebsites.net"
    ttl      = 1799
    type     = "CNAME"
  }

  record {
    hostname = "asuid.${var.subdomain}"
    address  = azurerm_linux_web_app.bhs_web.custom_domain_verification_id
    ttl      = 1799
    type     = "TXT"
  }
}
