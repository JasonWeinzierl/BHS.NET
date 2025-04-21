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

    auth0 = {
      source  = "auth0/auth0"
      version = "~>1.16.0"
    }

    sendgrid = {
      source  = "Meuko/sendgrid"
      version = "1.0.5" # Community provider, do not upgrade without inspecting changes.
    }

    namecheap = {
      source  = "namecheap/namecheap"
      version = "~>2.2.0"
    }
  }

  backend "azurerm" {
    resource_group_name  = "bhs-production-tfstate-rg"
    storage_account_name = "tfstate1ee5v"
    container_name       = "tfstate"
    key                  = "production.tfstate"
  }
}

provider "azurerm" {
  features {}
}

provider "github" {}

provider "auth0" {}

provider "azuread" {}

provider "sendgrid" {}

provider "namecheap" {}

data "github_repository" "bhs" {
  full_name = "JasonWeinzierl/BHS.NET"
}

data "azurerm_resource_group" "bhs_shared" {
  name = "bhs-shared-web-rg"
}

data "azurerm_app_configuration" "bhs" {
  name                = "bhs-shared-web-appcs"
  resource_group_name = data.azurerm_resource_group.bhs_shared.name
}

resource "azurerm_resource_group" "bhs" {
  name     = "bhs-production-web-rg"
  location = "centralus"
}

module "auth" {
  source = "../../modules/auth"

  hostnames = [
    "https://beltonhistoricalsociety.org",
    "https://www.beltonhistoricalsociety.org",
  ]
  auth0_connection_name = "Username-Password-Authentication"
}

module "build_server" {
  source = "../../modules/build_server"

  environment            = "production"
  github_repository_name = data.github_repository.bhs.name
  e2e_url                = "https://beltonhistoricalsociety.org"
  e2e_username           = module.auth.test_user_username
  e2e_password           = module.auth.test_user_password
  auth_domain            = module.auth.auth_domain
  auth_client_id         = module.auth.spa_client_id
}

module "mongo" {
  source = "../../modules/mongo"

  resource_group_name = azurerm_resource_group.bhs.name
  location            = azurerm_resource_group.bhs.location
  cosmos_account_name = "bhsmongo"
}

module "key_vault" {
  source = "../../modules/key_vault"

  environment         = "production"
  resource_group_name = azurerm_resource_group.bhs.name
  location            = azurerm_resource_group.bhs.location
}

module "public_storage" {
  source = "../../modules/public_storage"

  location             = azurerm_resource_group.bhs.location
  resource_group_name  = azurerm_resource_group.bhs.name
  storage_account_name = "beltonhistoricalstorage"
}

module "insights" {
  source = "../../modules/insights"

  environment         = "production"
  resource_group_name = azurerm_resource_group.bhs.name
  location            = azurerm_resource_group.bhs.location
}

module "web_apps" {
  source = "../../modules/web_apps"

  environment               = "production"
  location                  = azurerm_resource_group.bhs.location
  resource_group_name       = azurerm_resource_group.bhs.name
  insights_conn_str         = module.insights.web_insights_conn_str
  insights_key              = module.insights.web_insights_key
  app_service_name          = "beltonhistorical"
  app_config_id             = data.azurerm_app_configuration.bhs.id
  app_config_conn_str       = data.azurerm_app_configuration.bhs.primary_read_key[0].connection_string
  build_server_principal_id = module.build_server.build_server_principal_id
  mongo_connection_string   = module.mongo.connection_string
  mongo_database_name       = module.mongo.database_name
  key_vault_id              = module.key_vault.key_vault_id
  auth_domain               = module.auth.auth_domain
  api_auth_audience         = module.auth.api_auth_audience
  spa_auth_client_id        = module.auth.spa_client_id
  api_auth_client_id        = module.auth.api_client_id
  api_auth_client_secret    = module.auth.api_client_secret
}

module "bhs_hostname_root" {
  count = 1

  source = "../../modules/custom-hostname"

  hostname            = "beltonhistoricalsociety.org"
  resource_group_name = azurerm_resource_group.bhs.name
  app_service_name    = module.web_apps.bhs_web_name
}

# TODO: set up the A record for the root domain.
# TODO: move this to custom-hostname module?

resource "namecheap_domain_records" "subdomain_beltonhistoricalsociety_org" {
  domain = "beltonhistoricalsociety.org"
  mode   = "MERGE"

  record {
    hostname = "www"
    address  = "${module.web_apps.bhs_web_name}.azurewebsites.net"
    ttl      = 1799
    type     = "CNAME"
  }

  record {
    hostname = "asuid.www"
    address  = module.web_apps.bhs_web_verification_id
    ttl      = 1799
    type     = "TXT"
  }
}

module "bhs_hostname_subdomain" {
  source = "../../modules/custom-hostname"

  hostname            = "www.beltonhistoricalsociety.org"
  resource_group_name = azurerm_resource_group.bhs.name
  app_service_name    = module.web_apps.bhs_web_name

  depends_on = [
    namecheap_domain_records.subdomain_beltonhistoricalsociety_org, # NOTE: This may still fail to create until the records have propagated.
  ]
}

module "me" {
  source = "../../modules/me"

  key_vault_id = module.key_vault.key_vault_id
}
