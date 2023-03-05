/**
 * This script transforms the prod environment file during build.
 */
import { writeFileSync } from 'fs';

const targetPath = `${__dirname}/../src/environments/environment.production.ts`;
console.info(`Creating production environment file at ${targetPath}`);

const transformedConfig = `export const environment = {
  appInsights: {
    connectionString: '${process.env['APPLICATIONINSIGHTS_CONNECTION_STRING']}',
  },
  auth0: {
    domain: '${process.env['AUTH0_DOMAIN']}',
    clientId: '${process.env['AUTH0_CLIENT_ID']}',
    authorizationParams: {
      audience: '${process.env['AUTH0_AUDIENCE']}',
      // eslint-disable-next-line @typescript-eslint/naming-convention
      redirect_uri: window.location.origin,
    },
  },
  version: {
    semver: '${process.env['SEMANTIC_VERSION']}',
    info: '${process.env['INFORMATIONAL_VERSION']}',
  },
};
`;

writeFileSync(targetPath, transformedConfig, 'utf8');
console.info('Finished creating environment file.');
