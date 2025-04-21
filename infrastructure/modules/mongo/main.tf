terraform {
  required_version = "~>1.5.7"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>4.26.0"
    }
  }
}

resource "azurerm_cosmosdb_account" "bhs_db" {
  name                = var.cosmos_account_name
  resource_group_name = var.resource_group_name
  location            = var.location

  offer_type           = "Standard"
  kind                 = "MongoDB"
  mongo_server_version = "4.2"
  free_tier_enabled    = var.enable_free_cosmos

  ip_range_filter = [
    "97.135.189.47",
    "104.42.195.92",
    "40.76.54.131",
    "52.176.6.30",
    "52.169.50.45",
    "52.187.184.26",
    "40.80.152.199",
    "13.95.130.121",
    "20.245.81.54",
    "40.118.23.126",
    "0.0.0.0",
  ]

  consistency_policy {
    consistency_level = "Session"
  }

  geo_location {
    failover_priority = 0
    location          = var.location
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
  resource_group_name = var.resource_group_name
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
