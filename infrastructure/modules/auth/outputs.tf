output "auth_domain" {
  description = "The domain of the authentication server."
  value       = data.auth0_tenant.bhs.domain
}

output "api_auth_audience" {
  description = "The authentication audience of the API."
  value       = auth0_resource_server.bhs_api.identifier
}

output "spa_client_id" {
  description = "The client ID for the SPA."
  value       = auth0_client.bhs_spa.client_id
}

output "api_client_id" {
  description = "The client ID for the API."
  value       = auth0_client.bhs_api.client_id
}

output "api_client_secret" {
  description = "The client secret for the API."
  value       = auth0_client_credentials.bhs_api.client_secret
  sensitive   = true
}

output "test_user_username" {
  description = "The username of the test user."
  value       = auth0_user.noreply.email
}

output "test_user_password" {
  description = "The password of the test user."
  value       = auth0_user.noreply.password
  sensitive   = true
}
