/**
 * This script transforms the prod environment file during build.
 */
import { writeFileSync } from 'fs';

const targetPath = './src/environments/environment.production.ts';

const transformedConfig = `export const environment: any = {
  appInsights: {
    connectionString: '${process.env['APPLICATIONINSIGHTS_CONNECTION_STRING']}',
  },
};
`;

writeFileSync(targetPath, transformedConfig, 'utf8');
