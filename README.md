# BHS.NET

**NOTE: This branch is the SQL Server implementation.  This application now uses MongoDB instead, and this branch is only available for historical purposes.**

[![.NET](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/dotnet.yml/badge.svg)](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/dotnet.yml)
[![Angular](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/angular.yml/badge.svg)](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/angular.yml)
[![SSDT](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/ssdt.yml/badge.svg)](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/ssdt.yml)

The [Belton Historical Society website](https://www.beltonhistoricalsociety.org/), built on ASP.NET Core, Angular, and SQL Server.

## Development Setup

### Prerequisites

- .NET 7 SDK and Visual Studio 2022
- Node.JS and Angular CLI `npm install @angular/cli -g`
- SQL Server 2019 and SQL Server Data Tools

### Build and Debug

#### Database

1. Set up a localhost SQL Server database named "bhs".
2. Ensure your Windows login has Integrated Security access.
3. Publish the BHS.SSDT project to your local database.

#### Frontend

1. Open the BHS.Web/ClentApp directory in VS Code or other editor.
2. `npm install`
3. `npm run start`
    - Use the included VS Code launch profiles to attach the debugger.

This starts the angular development server on `localhost:4200`.
The backend API will forward requests to the development server if not handled by other middleware.

#### Backend

Use the launch profile in Visual Studio, or

```sh
dotnet run --project src/BHS.Web/BHS.Web.csproj --launch-profile BHS.Web
```

This starts the web application server and forwards SPA requests to the frontend.

Navigate to `/api/swagger` to use the Swagger UI.
This does not require the frontend development server to be running.
