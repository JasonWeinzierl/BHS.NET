# BHS.NET

The [Belton Historical Society website](https://www.beltonhistoricalsociety.org/),
built on ASP.NET Core, Angular, and MongoDB.

[![Azure Release](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/azure-release.yml/badge.svg)](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/azure-release.yml)

## Development Setup

### Prerequisites

- .NET 7 SDK and Visual Studio 2022
- Node.JS and Angular CLI `npm install @angular/cli -g`
- MongoDB

### Build and Debug

#### Database

1. You must have access to an instance of MongoDB.
    - Either use a free cloud offering or set up a local instance.
2. Put your connection string in `ConnectionStrings:bhsMongo`.

#### Frontend

1. Open the BHS.Web/ClentApp directory in VS Code or other editor.
2. `npm install`
3. `npm run start`
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

#### Authentication

Set up User Secrets (or edit `appsettings.Production.json`) with the following:

```json
{
  "Authentication:Schemes:Bearer:Authority": "https://AUTH_DOMAIN/",
  "Authentication:Schemes:Bearer:ValidAudiences:0": "AUDIENCE",
  "Authentication:Schemes:Bearer:ValidIssuer": "AUTH_DOMAIN"
}
```
