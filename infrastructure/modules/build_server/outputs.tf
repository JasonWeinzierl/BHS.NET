output "build_server_principal_id" {
  description = "The object ID of the build server's service principal."
  value       = azuread_service_principal.bhs_github_actions.object_id
}
