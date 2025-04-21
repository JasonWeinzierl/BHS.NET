output "bhs_web_name" {
  description = "The name of the web app."
  value       = azurerm_linux_web_app.bhs_web.name
}

output "bhs_web_verification_id" {
  description = "The verification ID for the custom domain."
  value       = azurerm_linux_web_app.bhs_web.custom_domain_verification_id
}
