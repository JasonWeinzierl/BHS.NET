import { AuthConfig } from '@auth0/auth0-angular';
import { InjectionToken } from '@angular/core';

export class AppEnvironment {
  constructor(
    public appInsights?: { connectionString?: string },
    public auth0?: AuthConfig,
  ) { }
}

export const APP_ENV = new InjectionToken<AppEnvironment>('app-environment');
