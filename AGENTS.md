# AGENTS.md - AI Agents Guide for BHS.NET

This document provides guidance for AI coding assistants working on the **Belton Historical Society on .NET (BHS.NET)** project.

## Project Overview

The BHS.NET project is a full-stack web application featuring:

- **Backend**: ASP.NET Core Web API
- **Frontend**: Modern Angular Single Page Application
- **Database**: MongoDB (Azure Cosmos DB for MongoDB) with a unique storage and query pattern
- **Authentication**: Auth0 integration with RBAC
- **Testing**: Comprehensive unit (both frontend and backend), integration (backend), and E2E testing
- **DevOps**: CI/CD via GitHub Actions, deploys to Azure App Service
- **Infrastructure as Code**: Terraform, but in a separate git repository (see References)

## Architecture

### Project Structure

```txt
src/                        # Main source code.
|-- BHS.*                   # .NET backend API projects.
|-- bhs-web-angular-app     # Angular frontend SPA.
|-- bhs-e2e                 # E2E test suite.
tests/
|-- BHS.*.Tests             # .NET unit test projects.
|-- BHS.*.IntegrationTests  # .NET integration test projects.
migrations/                 # MongoDB manual migration scripts.
```

### Backend Development

This project uses centralized package management via the `Directory.Packages.props` file.

#### ASP.NET Core Web API

This is a standard ASP.NET Core Web API application using modern best practices.

- It uses the new Microsoft.AspNetCore.SpaProxy package to expose the Angular frontend
  instead of the deprecated Microsoft.AspNetCore.SpaServices.Extensions package.
- It uses controllers for endpoints.

##### Database Access

The database interactions for this project are unique,
and this is the only non-standard part of this project.

- Documents in MongoDB often have arrays for changes over time:
  - Updates are made by pushing new items to various arrays.
  - Dates are used to indicate the latest change.
  - If a change has a date in the future, then it must not be honored yet.
- MongoDB aggregation pipelines are used extensively with `$unwind` operations to compare dates.
- All repositories use projection to progressively convert DTOs to domain models.
  - Intermediate DTOs are allowed. Type-safety within .NET is preferred over BsonDocument raw strings.

The core principle is this: **data is not deleted, only hidden**. Failure to follow this principle is unacceptable!

#### Migrations

When making incompatible changes to persisted data models,
migrations must be created in the `migrations/` directory.
Incompatible changes should be avoided unless absolutely necessary.

#### RBAC permissions and authentication

This project uses Auth0 for authentication and permissions.
The permissions are configured in the separate infrastructure repository via Terraform.

### Frontend Development

This project uses `yarn` for package management and script execution.

#### TypeScript

TypeScript best practices are to be followed, including:

- Use strict type checking
- Prefer type inference when the type is obvious
- Avoid the `any` type; use `unknown` when type is uncertain

Additionally, this project uses Zod to parse all external data and enforce type safety.

#### ESLint

The ESLint configurations are extremely strict and enforce best practices on all libraries, including:

- typescript-eslint strict type-checked rules
- All ESLint and typescript-eslint stylistic rules
- Vitest rules
- angular-eslint rules
- TailwindCSS rules, via `eslint-plugin-better-tailwindcss` (not the typical Prettier plugin)
- JSDoc rules
- RxJS rules
- Import sorting

#### Styling

This project uses modern Tailwind v4, with DaisyUI for components and Bootstrap Icons for icons.

No style tags or stylesheets are allowed.
ESLint rules will enforce this.

Tailwind classes must be sorted according to the ESLint plugin.
`ng lint --fix` will perform sorting automatically.

#### Angular

This is a bleeding-edge Angular app, which is always uses the latest versions.

All modern Angular features are preferred, including:

- Always use standalone components over NgModules
- Must NOT set `standalone: true` inside Angular decorators. It's the default in Angular v20+.
- Use signals for state management
- Implement lazy loading for feature routes
- Do NOT use the `@HostBinding` and `@HostListener` decorators. Put host bindings inside the `host` object of the `@Component` or `@Directive` decorator instead
- Use `NgOptimizedImage` for all static images.
  - `NgOptimizedImage` does not work for inline base64 images.

##### Declarative Programming

- Use signals or the async pipe to declaratively chain behavior. Do not `subscribe`.
  - ESLint rules will enforce this.

##### Components

- Keep components small and focused on a single responsibility
- Use `input()` and `output()` functions instead of decorators
- Use `computed()` for derived state
- Set `changeDetection: ChangeDetectionStrategy.OnPush` in `@Component` decorator
- Prefer inline templates for small components
- Prefer Reactive forms instead of Template-driven ones
- Do NOT use `ngClass`, use `class` bindings instead
- Do NOT use `ngStyle`, use `style` bindings instead
- When using external templates/styles, use paths relative to the component TS file.

##### State Management

- Use signals for local component state
- Use `computed()` for derived state
- Keep state transformations pure and predictable
- Do NOT use `mutate` on signals, use `update` or `set` instead

##### Templates

- Keep templates simple and avoid complex logic
- Use native control flow (`@if`, `@for`, `@switch`) instead of `*ngIf`, `*ngFor`, `*ngSwitch`
- Use the async pipe to handle observables
- Do not assume globals like (`new Date()`) are available.
- Do not write arrow functions in templates (they are not supported).

##### Services

- Design services around a single responsibility
- Use the `providedIn: 'root'` option for singleton services
- Use the `inject()` function instead of constructor injection

## Testing Strategy

Test projects to work with:

- `tests/*.Tests/` - .NET unit tests with Xunit.v3
- `src/bhs-web-angular-app/**/*.spec.ts` - Angular unit tests with Vitest
- `tests/BHS.Api.IntegrationTests/` - .NET integration tests with Xunit.v3, EphemeralMongo, and Microsoft.AspNetCore.Mvc.Testing
- `src/bhs-e2e/` - End-to-end browser testing with WebdriverIO

## Development Workflow

.NET commands in the repository root:

- Backend build: `dotnet build`
- Backend test: `dotnet test`

Angular commands must be executed within `src/bhs-web-angular-app`:

- Frontend build: `yarn build`
- Frontend lint: `yarn lint`
  - Automatically fix with `yarn lint --fix`
- Frontend single test run: `yarn test --watch=false`

## Configuration Requirements

Configurations are documented in the infrastructure repository's Terraform configurations.

When making infrastructure changes,
Agents must provide explicit instructions to the supervising developer,
as the agent does not have access to the infrastructure repository.

## Meaningful git history

The git history of this project is well-maintained and meaningful.
Commit messages use standard semantic prefixes like `feat`, 'fix`, `chore`, etc.
Agents may choose to look back on previous commits for historical context.

As a caveat, commits marked with `chore(deps)` or `chore(deps-dev)` may be filtered out
because those commits rarely contain useful context and are pure dependency bumps.

## References

- DaisyUI llms.txt: [https://daisyui.com/llms.txt]
- Angular llms.txt: [https://angular.dev/llms.txt]
- Infrastructure repository: [https://github.com/JasonWeinzierl/belton-historical-society-infrastructure]
