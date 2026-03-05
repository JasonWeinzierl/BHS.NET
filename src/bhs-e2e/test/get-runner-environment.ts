import { z } from 'zod';

const runnerEnvironmentScheme = z.object({
  E2E_auth0Domain: z.string(),
  E2E_auth0ClientId: z.string(),
  E2E_auth0TestUsername: z.string(),
  E2E_auth0TestPassword: z.string(),
});

/**
 * Parses `process.env` into an object with the expected keys.
 *
 * Note this function is only safe to call inside runner processes (e.g. test suites)
 * which have the expected environment variables set.
 */
// eslint-disable-next-line n/no-process-env
const getRunnerEnvironment = () => runnerEnvironmentScheme.parse(process.env);
export default getRunnerEnvironment;
