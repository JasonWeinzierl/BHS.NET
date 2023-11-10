terraform {
  required_version = "~>1.5.3"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>3.79.0"
    }

    azuread = {
      source  = "hashicorp/azuread"
      version = "~>2.45.0"
    }

    github = {
      source  = "integrations/github"
      version = "~>5.42.0"
    }

    auth0 = {
      source  = "auth0/auth0"
      version = "~>1.0.0"
    }

    sendgrid = {
      source  = "Meuko/sendgrid"
      version = "1.0.5" # Community provider, do not upgrade without inspecting changes.
    }

    namecheap = {
      source  = "namecheap/namecheap"
      version = "~>2.1.0"
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

provider "namecheap" {}
