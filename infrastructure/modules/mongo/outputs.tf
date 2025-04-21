output "connection_string" {
  description = "MongoDB connection string"
  value       = azurerm_cosmosdb_account.bhs_db.primary_mongodb_connection_string
}

output "database_name" {
  description = "MongoDB database name"
  value       = azurerm_cosmosdb_mongo_database.bhs_db.name
}
