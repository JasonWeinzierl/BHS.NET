variable "environment" {
  description = "The non-abbreviated environment name."
  type        = string
}

variable "github_repository_name" {
  description = "The name of the GitHub repository."
  type        = string
}

variable "e2e_url" {
  description = "The URL of the environment to run E2E tests against."
  type        = string
}

variable "e2e_username" {
  description = "The username for the E2E tests."
  type        = string
}

variable "e2e_password" {
  description = "The password for the E2E tests."
  type        = string
  sensitive   = true
}

variable "auth_domain" {
  description = "The domain for the authentication service."
  type        = string
}

variable "auth_client_id" {
  description = "The authentication client ID for the app under E2E."
  type        = string
}
