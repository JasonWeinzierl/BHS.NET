terraform {
  required_version = "~>1.5.7"

  required_providers {
    auth0 = {
      source  = "auth0/auth0"
      version = "~>1.16.0"
    }
  }
}

data "auth0_tenant" "bhs" {}
data "auth0_client" "terraform" {
  name = "Terraform Provider Auth0"
}

resource "auth0_client" "bhs_spa" {
  name        = "BHS Angular Client"
  description = ""
  app_type    = "spa"

  allowed_logout_urls = var.hostnames
  callbacks           = var.hostnames
  web_origins         = var.hostnames
  logo_uri            = "https://beltonhistoricalsociety.org/assets/img/2019/icons/apple-touch-icon.png"

  is_first_party       = true
  oidc_conformant      = true
  custom_login_page_on = true
  cross_origin_auth    = true

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
  cross_origin_auth    = true

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

  scopes {
    name        = "write:banners"
    description = "Author banner resources"
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

  permissions {
    resource_server_identifier = auth0_resource_server.bhs_api.identifier
    name                       = "write:banners"
  }

  depends_on = [
    auth0_resource_server_scopes.bhs_api_scopes
  ]
}

resource "auth0_connection" "bhs_userpassauth" {
  name     = var.auth0_connection_name
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
