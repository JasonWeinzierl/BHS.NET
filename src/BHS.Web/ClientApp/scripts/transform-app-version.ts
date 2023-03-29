/**
 * This script transforms the app version file during build.
 */
import { writeFileSync } from 'fs';

const targetPath = `${__dirname}/../src/environments/app-version.release.ts`;
console.info(`Creating app version file at ${targetPath}`);

const transformedConfig = `export const appVersion = {
  semver: '${process.env['SEMANTIC_VERSION']}',
  info: '${process.env['INFORMATIONAL_VERSION']}',
  commitDate: '${process.env['COMMIT_DATE']}',
};
`;

writeFileSync(targetPath, transformedConfig, 'utf8');
console.info('Finished creating app version file.');
