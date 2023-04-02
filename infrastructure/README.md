# BHS infrastructure

## Provision

```sh
cd modules/shared
terraform init
terraform apply

cd ../env-specific
terraform init

terraform workspace select staging
terraform apply -var-file="env/staging.tfvars"

# Don't forget to swap env var credentials too!

terraform workspace select default
terraform apply -var-file="env/production.tfvars"
```

## Getting Started

Populate environment variables:

* ARM_ACCESS_KEY
* GITHUB_TOKEN
* AUTH0_DOMAIN
* AUTH0_CLIENT_ID
* AUTH0_CLIENT_SECRET
* SENDGRID_API_KEY (needs Full Access. We will use this API key to create additional API keys for Mail Send.)

The `LoadDotEnv.ps1` script in this directory can help load these values from a `.env` file with Powershell.

### Auth0

Auth0 Tenants cannot be created via terraform and must be created in their UI first.

1. Create an Auth0 tenant for the desired environment.
2. Create a Machine to Machine Application for terraform to use.
3. Authorize the new application to call the Auth0 Management API with all scopes.
4. Use the resulting credentials to populate the required environment variables above.

You will need to swap credentials when switching terraform workspaces.

## Standards

* Names should be `bhs-{environment}-{application (if app specific)}-{resource abbreviation}`
  * ...unless the resource has different naming requirements.
* Follow abbreviations recommendations: [https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-abbreviations]
* App Configuration labels should be the .NET environment in lower case.
