# BHS infrastructure

## Provision

```sh
az login

cd modules/shared
terraform init
terraform apply

cd ../env-specific
terraform init

../../LoadDotEnv.ps1 staging
terraform workspace select staging
terraform apply -var-file="env/staging.tfvars"

../../LoadDotEnv.ps1 production
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
* NAMECHEAP_USER_NAME
* NAMECHEAP_API_USER
* NAMECHEAP_API_KEY

The `LoadDotEnv.ps1` script in this directory can help load these values from a `.env` file with Powershell.

### Manual Steps

The GitHub repo, the Azure subscription, the domain purchased through Namecheap, and several Auth0 resources are not managed in terraform.

#### Auth0 pre-provision

Auth0 Tenants cannot be created via terraform and must be created in their UI first.

1. Create an Auth0 tenant for the desired environment.
2. Create a Machine to Machine Application for terraform to use named "Terraform Provider Auth0" (name must be exact so we can reference it).
3. Authorize the new application to call the Auth0 Management API with all scopes.
4. Use the resulting credentials to populate the required environment variables above.

#### Auth0 post-provision

After provisioning, you'll need to manually delete the auto-created connections in Auth0's UI.
This is both a database connection and a google-oauth2 connection.

#### Namecheap API Access

Namecheap API Access must be enabled.

1. Go to Profile -> Tools -> API Access
2. Set API Access to ON
3. Use the given API Key for NAMECHEAP_API_KEY
4. Add your current machine's API address to the whitelist.

#### DNS propagation

Creating the hostname bindings may fail until DNS updates have propagated.

## Standards

* Names should be `bhs-{environment}-{application (if app specific)}-{resource abbreviation}`
  * ...unless the resource has different naming requirements.
* Follow abbreviations recommendations: [https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-abbreviations]
* App Configuration labels should be the .NET environment in lower case.
