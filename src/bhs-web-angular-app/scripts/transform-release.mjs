// @ts-check
import { readFile, writeFile } from 'node:fs/promises';
import path from 'node:path';
import { z } from 'zod';

const __dirname = import.meta.dirname;

// app-version.ts contains version information about
// a particular build snapshot of this application.
// It should NOT be environment-specific.
// That file is replaced by the output of this script
// by the Angular build process when building the release configuration.

const releaseEnvScheme = z.object({
  SEMANTIC_VERSION: z.string(),
  INFORMATIONAL_VERSION: z.string(),
  COMMIT_DATE: z.string(),
});
const {
  SEMANTIC_VERSION,
  INFORMATIONAL_VERSION,
  COMMIT_DATE,
} = releaseEnvScheme.parse(process.env);

// Value must match angular.json's `fileReplacements.with`.
const filePath = path.join(__dirname, '..', 'src/environments/app-version.release.ts');

console.debug(`WRITE: ${filePath}`);

// Schema must match `app-version.ts`.
const versionData = {
  semver: SEMANTIC_VERSION,
  info: INFORMATIONAL_VERSION,
  commitDate: COMMIT_DATE,
};
const fileContent = `/* eslint-disable */
export default ${JSON.stringify(versionData, null, 2)};
`;

// Write the file
await writeFile(filePath, fileContent, 'utf8');

console.debug('::group::Created File');
console.debug(await readFile(filePath, 'utf8'));
console.debug('::endgroup::');
