terraform {
  required_version = "~>1.3.9"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>3.49.0"
    }

    azuread = {
      source  = "hashicorp/azuread"
      version = "~>2.36.0"
    }

    github = {
      source  = "integrations/github"
      version = "~>5.18.0"
    }

    auth0 = {
      source  = "auth0/auth0"
      version = "~>0.45.0"
    }

    sendgrid = {
      source  = "Meuko/sendgrid"
      version = "1.0.5" # Community provider, do not upgrade without inspecting changes.
    }
  }

  backend "azurerm" {
    resource_group_name  = "bhs-production-tfstate-rg"
    storage_account_name = "tfstate1ee5v"
    container_name       = "tfstate"
    key                  = "terraform.tfstate"
  }
}

provider "azurerm" {
  features {}
}

provider "github" {}

provider "auth0" {}

provider "azuread" {}

provider "sendgrid" {}
