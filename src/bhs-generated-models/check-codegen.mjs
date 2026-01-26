// @ts-check
import fs from 'node:fs/promises';
import path from 'node:path';

const __dirname = import.meta.dirname;
const OPENAPI_FILE = path.join(__dirname, '../../artifacts/openapi.json');
const GENERATED_FILE = path.join(__dirname, './dist/zod.gen.ts');

try {
    await fs.access(OPENAPI_FILE);
    await fs.access(GENERATED_FILE);
} catch (e) {
    if (/** @type {NodeJS.ErrnoException} */(e)?.code === 'ENOENT') {
        console.error('Code generation files are missing. Please run "dotnet build" and then "yarn models:build" to generate the necessary files.', e);
    } else {
        console.error('An unexpected error occurred while checking for code generation files.', e);
    }
    process.exit(1);
}
