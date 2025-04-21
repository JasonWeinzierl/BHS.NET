output "web_insights_conn_str" {
  description = "The connection string for a 'web' Application Insights instance."
  value       = azurerm_application_insights.bhs.connection_string
  sensitive   = true
}

output "web_insights_key" {
  description = "The instrumentation key for a 'web' Application Insights instance."
  value       = azurerm_application_insights.bhs.instrumentation_key
  sensitive   = true
}
