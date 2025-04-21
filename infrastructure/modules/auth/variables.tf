variable "hostnames" {
  description = "List of hostnames for the application."
  type        = list(string)
}

variable "auth0_connection_name" {
  # WARNING: Auth0 automatically creates a database connection when a client is created.
  # You must manually delete that connection in the UI.
  description = "Name of the Auth0 connection."
  type        = string
}
