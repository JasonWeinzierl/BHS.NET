// @ts-check
import { exec } from 'node:child_process';
import fs from 'node:fs/promises';
import path from 'node:path';
import { promisify } from 'node:util';

const execAsync = promisify(exec);

const __dirname = import.meta.dirname;
const OPENAPI_FILE = path.join(__dirname, '../../artifacts/openapi.json');
const GENERATED_FILE = path.join(__dirname, './dist/zod.gen.ts');

const codegenCommands = '"dotnet build" and then "yarn models:build"';

async function getLatestCommitDate() {
    try {
        const result = await execAsync('git log -1 --format=%ct -- src/', { 
            encoding: 'utf8', 
            cwd: path.join(__dirname, '../..') 
        });
        const timestamp = parseInt(result.stdout.trim(), 10);
        return new Date(timestamp * 1000);
    } catch (e) {
        console.warn('Could not determine latest git commit date.', e);
        return new Date(0);
    }
}

/**
 * @param {string} filePath
 */
async function getFileInfo(filePath) {
    const filename = path.basename(filePath);

    try {
        const stats = await fs.stat(filePath);
        return {
            exists: /** @type {const} */(true),
            mtime: stats.mtime,
            filename,
        };
    } catch (e) {
        if (/** @type {NodeJS.ErrnoException} */(e)?.code === 'ENOENT') {
            return {
                exists: /** @type {const} */(false),
                filename,
            };
        }
        throw e;
    }
}

const [openapiInfo, generatedInfo, latestCommitDate] = await Promise.all([
    getFileInfo(OPENAPI_FILE),
    getFileInfo(GENERATED_FILE),
    getLatestCommitDate()
]);

const missingFiles = [openapiInfo, generatedInfo]
    .filter(info => !info.exists)
    .map(info => info.filename);

if (missingFiles.length > 0) {
    console.error(`Code generation files are missing: ${missingFiles.join(', ')}. Please run ${codegenCommands} to generate the necessary files.`);
    process.exit(1);
}

const staleFiles = [openapiInfo, generatedInfo]
    .filter(info => info.exists && info.mtime < latestCommitDate)
    .map(info => info.filename);

if (staleFiles.length > 0) {
    console.warn(`Warning: code generation files are older than the latest git commit (${latestCommitDate.toISOString()}): ${staleFiles.join(', ')}`);
    console.warn(`Consider running ${codegenCommands} to ensure generated files are up to date.`);
}
