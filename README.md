# BHS.NET

The [Belton Historical Society website](https://www.beltonhistoricalsociety.org/),
built on ASP.NET Core, Angular, and MongoDB.

[![Azure Release](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/azure-release.yml/badge.svg)](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/azure-release.yml)

## Development Setup

### Prerequisites

- .NET 10 SDK and Visual Studio 2026
  - `dotnet tool restore`
- Node.JS 24 and yarn
  - `corepack enable`
- MongoDB

### Build and Debug

#### Database

1. You must have access to an instance of MongoDB.
    - Either use a free cloud offering or set up a local instance.
2. Add a connection string named `ConnectionStrings:bhsMongo`.
    - See the below section on Configuration for other required settings.

#### Frontend

1. Open the `src/bhs-web-angular-app` directory in VS Code or other editor.
2. `yarn`
3. `yarn start`
    - Use the included VS Code launch profiles to attach the debugger.

This starts the angular development server on `localhost:4200`, but backend requests will not succeed.
See the next section for starting up the backend
so that the web host can forward requests to the frontend development server if not handled by other middleware.

#### Backend

Use the launch profile in Visual Studio, or

```sh
dotnet run --project src/BHS.Web/BHS.Web.csproj --launch-profile BHS.Web
```

This starts the web application server and forwards SPA requests to the frontend.
Test the frontend application through the launched browser window.

Navigate to `/api/swagger` to use the Swagger UI.
This does not require the frontend development server to be running.

#### Infrastructure

See separate repository: [https://github.com/JasonWeinzierl/belton-historical-society-infrastructure]

#### Auth

This application uses Auth0;
you must set up an Auth0 tenant with RBAC permissions matching those set up in `Startup.cs`.
You must also add the configuration settings as mentioned below.

#### Configuration

The required configuration settings are listed in the `modules/web-apps/configs.tf` file **of the separate infrastructure repository**.
These settings include authentication settings, database connection strings, API keys, etc.
Without these settings, various features may fail to work locally.

##### Example

The infrastructure lists the following resource, which is a required setting you need to run this application locally.

```hcl
resource "azurerm_app_configuration_key" "auth0_domain" {
  configuration_store_id = data.azurerm_app_configuration.bhs.id

  label = var.environment
  key   = "AUTH0_DOMAIN"
  value = ...
}
```

Based on this resource, create a setting named `AUTH0_DOMAIN` with a value of your Auth0 tenant's domain.

You may add this setting either to your local appsettings.Development.json,
user secrets, machine environment variables, command line arguments,
or else create an instance of Azure App Configuration with these.

Repeat this step for every other resource of type `azurerm_app_configuration_key`.

If using Azure App Configuration locally, each key must be labeled with nothing or else `development`.
Add the App Configuration's connection string to a setting named `ConnectionStrings:AppConfig`.
