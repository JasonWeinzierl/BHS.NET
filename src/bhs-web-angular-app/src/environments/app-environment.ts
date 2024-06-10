import { AuthConfig } from '@auth0/auth0-angular';
import { z } from 'zod';

export const APP_ENVIRONMENT_VALIDATOR = z.object({
  appInsights: z.object({
    connectionString: z.string().nullish(),
  }).optional(),
  auth0: z.object({
    domain: z.string(),
    clientId: z.string(),
    authorizationParams: z.object({
      audience: z.string(),
    }),
  }).optional(),
});

// TODO: this can be converted to an injection token.
export class AppEnvironment {
  constructor(
    public readonly appInsights?: { connectionString?: string | null },
    public readonly auth0?: AuthConfig,
  ) { }
}
