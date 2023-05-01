import { AuthConfig } from '@auth0/auth0-angular';

export class AppEnvironment {
  constructor(
    public readonly appInsights?: { connectionString?: string },
    public readonly auth0?: AuthConfig,
  ) { }
}
