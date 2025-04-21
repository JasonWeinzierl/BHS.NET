terraform {
  required_version = "~>1.5.7"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>4.26.0"
    }

    azuread = {
      source  = "hashicorp/azuread"
      version = "~>3.3.0"
    }

    github = {
      source  = "integrations/github"
      version = "~>6.6.0"
    }
  }
}

data "azuread_client_config" "current" {}
data "azurerm_client_config" "current" {}

resource "github_repository_environment" "this" {
  environment = var.environment
  repository  = var.github_repository_name

  deployment_branch_policy {
    protected_branches     = true
    custom_branch_policies = false
  }
}

resource "github_actions_environment_secret" "azure_client_id" {
  repository      = var.github_repository_name
  environment     = github_repository_environment.this.environment
  secret_name     = "AZURE_CLIENT_ID"
  plaintext_value = azuread_application.bhs_github_actions.client_id
}

resource "github_actions_environment_secret" "azure_client_secret" {
  repository      = var.github_repository_name
  environment     = github_repository_environment.this.environment
  secret_name     = "AZURE_CLIENT_SECRET"
  plaintext_value = azuread_service_principal_password.bhs_github_actions.value
}

resource "github_actions_environment_secret" "azure_subscription_id" {
  repository      = var.github_repository_name
  environment     = github_repository_environment.this.environment
  secret_name     = "AZURE_SUBSCRIPTION_ID"
  plaintext_value = data.azurerm_client_config.current.subscription_id
}

resource "github_actions_environment_secret" "azure_tenant_id" {
  repository      = var.github_repository_name
  environment     = github_repository_environment.this.environment
  secret_name     = "AZURE_TENANT_ID"
  plaintext_value = data.azuread_client_config.current.tenant_id
}

resource "github_actions_environment_variable" "cypress_auth0_domain" {
  repository    = var.github_repository_name
  environment   = github_repository_environment.this.environment
  variable_name = "CYPRESS_AUTH0_DOMAIN"
  value         = var.auth_domain
}

resource "github_actions_environment_variable" "cypress_auth0_client_id" {
  repository    = var.github_repository_name
  environment   = github_repository_environment.this.environment
  variable_name = "CYPRESS_AUTH0_CLIENT_ID"
  value         = var.auth_client_id
}

resource "github_actions_environment_variable" "cypress_auth0_test_username" {
  repository    = var.github_repository_name
  environment   = github_repository_environment.this.environment
  variable_name = "CYPRESS_AUTH0_TEST_USERNAME"
  value         = var.e2e_username
}

resource "github_actions_environment_secret" "cypress_auth0_test_password" {
  repository      = var.github_repository_name
  environment     = github_repository_environment.this.environment
  secret_name     = "CYPRESS_AUTH0_TEST_PASSWORD"
  plaintext_value = var.e2e_password
}

resource "github_actions_environment_variable" "cypress_base_url" {
  repository    = var.github_repository_name
  environment   = github_repository_environment.this.environment
  variable_name = "CYPRESS_BASE_URL"
  value         = var.e2e_url
}


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
  service_principal_id = azuread_service_principal.bhs_github_actions.id

  rotate_when_changed = {
    # Change this to rotate the password.
    last_changed = "2025-04-21T07:37:01Z"
  }
}
