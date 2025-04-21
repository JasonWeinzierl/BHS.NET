output "key_vault_id" {
  description = "The ID of the Azure Key Vault instance."
  value       = azurerm_key_vault.bhs.id
}