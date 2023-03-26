import { AuthConfig } from '@auth0/auth0-angular';

export class AppEnvironment {
  constructor(
    public appInsights?: { connectionString?: string },
    public auth0?: AuthConfig,
  ) { }
}
