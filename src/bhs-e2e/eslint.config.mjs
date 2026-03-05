// @ts-check
import js from '@eslint/js';
import stylistic from '@stylistic/eslint-plugin';
import gitignore from 'eslint-config-flat-gitignore';
import n from 'eslint-plugin-n';
import perfectionist from 'eslint-plugin-perfectionist';
import unicorn from 'eslint-plugin-unicorn';
import * as wdio from 'eslint-plugin-wdio';
import { defineConfig } from 'eslint/config';
import tseslint from 'typescript-eslint';

export default defineConfig(gitignore(), {
  files: [
    '**/*.js',
    '**/*.mjs',
    '**/*.ts',
  ],
  extends: [
    js.configs.recommended,
    unicorn.configs.recommended,
    stylistic.configs['disable-legacy'],
    stylistic.configs.customize({
      quotes: 'single',
      semi: true,
      jsx: false,
      braceStyle: '1tbs',
      commaDangle: 'always-multiline',
    }),
    n.configs['flat/recommended-module'],
  ],
  plugins: {
    perfectionist,
  },
  rules: {
    '@stylistic/arrow-parens': 'off',
    '@stylistic/quote-props': [
      'error',
      'as-needed',
    ],

    // Use zod to safely parse process.env.
    'n/no-process-env': 'error',

    'perfectionist/sort-imports': [
      'warn',
      {
        newlinesBetween: 0,
        internalPattern: [
          // From tsconfig paths, minus ng-mocks.
          '^@(core|data|shared|features|env|app)/.+',
        ],
      },
    ],
    'perfectionist/sort-named-imports': 'warn',

    'n/no-unsupported-features/node-builtins': [
      'error',
      {
        version: '24',
      },
    ],

    'unicorn/prevent-abbreviations': [
      'error',
      {
        ignore: [
          String.raw`\.e2e$`,
          String.raw`\.conf$`,
          /^ignore/i,
        ],
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
