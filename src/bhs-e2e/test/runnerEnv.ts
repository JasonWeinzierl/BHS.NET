import { z } from 'zod';

const runnerEnvScheme = z.object({
  E2E_auth0Domain: z.string(),
  E2E_auth0ClientId: z.string(),
  E2E_auth0Audience: z.string(),
  E2E_auth0TestUsername: z.string(),
  E2E_auth0TestPassword: z.string(),
});

// eslint-disable-next-line n/no-process-env
export default runnerEnvScheme.parse(process.env);
