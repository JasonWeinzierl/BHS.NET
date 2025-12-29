// @ts-check
import { spawn } from 'node:child_process';
import { existsSync, mkdirSync } from 'node:fs';
import { join } from 'node:path';

// This script sets up HTTPS for the application using the ASP.NET Core HTTPS certificate

const baseFolder = process.env.APPDATA !== undefined && process.env.APPDATA !== ''
  ? `${process.env.APPDATA}/ASP.NET/https`
  : `${process.env.HOME}/.aspnet/https`;

const certificateArg = process.argv.map(arg => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
const certificateName = certificateArg?.groups?.value ?? process.env.npm_package_name;

if (!certificateName) {
  console.error('Invalid certificate name. Run this script in the context of an yarn script or pass --name=<<app>> explicitly.');
  process.exit(-1);
}

const certFilePath = join(baseFolder, `${certificateName}.pem`);
const keyFilePath = join(baseFolder, `${certificateName}.key`);

if (!existsSync(baseFolder)) {
  mkdirSync(baseFolder, { recursive: true });
}

if (!existsSync(certFilePath) || !existsSync(keyFilePath)) {
  spawn('dotnet', [
    'dev-certs',
    'https',
    '--export-path',
    certFilePath,
    '--format',
    'Pem',
    '--no-password',
  ], { stdio: 'inherit' })
  .on('exit', (code) => process.exit(code));
}
