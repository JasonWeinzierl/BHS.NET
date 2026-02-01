/* eslint-disable import-x/no-named-as-default */
/* eslint-disable import-x/no-named-as-default-member */
// @ts-check
import js from '@eslint/js';
import stylistic from '@stylistic/eslint-plugin';
import { defineConfig } from 'eslint/config';
import gitignore from 'eslint-config-flat-gitignore';
import importX from 'eslint-plugin-import-x';
import n from 'eslint-plugin-n';
import * as wdio from 'eslint-plugin-wdio';
import tseslint from 'typescript-eslint';

export default defineConfig(gitignore(), {
  files: [
    '**/*.js',
    '**/*.mjs',
    '**/*.ts',
  ],
  extends: [
    js.configs.recommended,
    stylistic.configs['disable-legacy'],
    stylistic.configs.customize({
      quotes: 'single',
      semi: true,
      jsx: false,
      braceStyle: '1tbs',
      commaDangle: 'always-multiline',
    }),
    // @ts-expect-error
    importX.flatConfigs.recommended,
    // @ts-expect-error
    importX.flatConfigs.typescript,
    n.configs['flat/recommended-module'],
  ],
  rules: {
    '@stylistic/arrow-parens': 'off',
    '@stylistic/quote-props': [
      'error',
      'as-needed',
    ],

    // Use zod to safely parse process.env.
    'n/no-process-env': 'error',

    'sort-imports': [
      'warn',
      {
        ignoreCase: true,
        ignoreDeclarationSort: true,
      },
    ],
    'import-x/order': [
      'warn',
      {
        alphabetize: {
          order: 'asc',
          orderImportKind: 'asc',
          caseInsensitive: true,
        },
      },
    ],

    'n/no-unsupported-features/node-builtins': [
      'error',
      {
        version: '24',
      },
    ],
  },
}, {
  files: [
    '**/*.ts',
  ],
  extends: [
    tseslint.configs.strictTypeChecked,
    tseslint.configs.stylisticTypeChecked,
  ],
  settings: {
    'import-x/resolver': {
      typescript: true,
    },
  },
  languageOptions: {
    parserOptions: {
      projectService: true,
    },
  },
}, {
  files: [
    'test/**/*.ts',
  ],
  extends: [
    wdio.configs['flat/recommended'],
  ],
});
