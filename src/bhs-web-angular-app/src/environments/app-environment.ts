import { InjectionToken } from '@angular/core';
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

export type AppEnvironment = z.infer<typeof APP_ENVIRONMENT_VALIDATOR>;
export const APP_ENVIRONMENT = new InjectionToken<AppEnvironment>('AppEnvironment');
