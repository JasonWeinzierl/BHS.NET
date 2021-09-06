# BHS.NET

[![.NET](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/dotnet.yml/badge.svg)](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/dotnet.yml)
[![Angular](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/angular.yml/badge.svg)](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/angular.yml)
[![SSDT](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/ssdt.yml/badge.svg)](https://github.com/JasonWeinzierl/BHS.NET/actions/workflows/ssdt.yml)

The Belton Historical Society website, built on ASP.NET Core, Angular, and SQL Server.

https://www.beltonhistoricalsociety.org/

## Getting Started

You will need the following:

- .NET 6 SDK and Visual Studio 2022
- Angular CLI `npm install @angular/cli -g`
- SQL Server 2019 and SQL Server Data Tools

## Build and Debug

### SQL Server

1. Set up a localhost database named "bhs".
2. Ensure your login has Integrated Security access.
3. Publish the BHS.SSDT project to your local database.

### Frontend

1. Navigate to BHS.Web\ClientApp
2. `npm install`
3. `npm run build`
4. `npm run start`

This starts the angular development server on `localhost:4200`.  The API will proxy to this port.

### Backend

Use the launch profile in Visual Studio, or:

1. `dotnet restore`
2. `dotnet build`
3. `dotnet run -p BHS.Web`

This starts the web host and proxies SPA requests to the frontend.  Open a browser to the listening port.

Navigate to /api/swagger to use the Swagger UI.  This does not require the frontend proxy.
