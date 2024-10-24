/* eslint-disable import-x/no-named-as-default-member */
/* eslint-disable import-x/no-rename-default */
// @ts-check
import js from '@eslint/js';
import stylistic from '@stylistic/eslint-plugin';
import angular from 'angular-eslint';
import gitignore from 'eslint-config-flat-gitignore';
import cypress from 'eslint-plugin-cypress/flat';
import importX from 'eslint-plugin-import-x';
import jest from 'eslint-plugin-jest';
import jsdoc from 'eslint-plugin-jsdoc';
import globals from 'globals';
import tseslint from 'typescript-eslint';

// TODO: re-add these plugins when they support eslint v9:
// - rxjs

export default tseslint.config(gitignore(), {
  files: [
    '**/*.js',
    '**/*.mjs',
    '**/*.ts',
  ],
  extends: [
    js.configs.recommended,
    stylistic.configs['disable-legacy'],
    stylistic.configs.customize({
      flat: true,
      quotes: 'single',
      semi: true,
      jsx: false,
      braceStyle: '1tbs',
      commaDangle: 'always-multiline',
    }),
    importX.flatConfigs.recommended,
    importX.flatConfigs.typescript,
    jsdoc.configs['flat/recommended-typescript-error'],
  ],
  rules: {
    // general
    'complexity': [
      'warn',
      15,
    ],
    'curly': [
      'error',
      'all',
    ],
    'no-shadow': 'error',
    'default-param-last': 'error',

    // stylistic
    '@stylistic/arrow-parens': 'off',
    '@stylistic/indent': 'off',
    '@stylistic/multiline-ternary': 'off',
    '@stylistic/no-extra-semi': 'error',
    '@stylistic/generator-star-spacing': [
      'error',
      'after',
    ],

    // imports
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
    'import-x/no-rename-default': 'warn',
    'import-x/no-useless-path-segments': 'warn',

    // jsdoc
    'valid-jsdoc': 'off',
    'require-jsdoc': 'off',
    'jsdoc/require-jsdoc': [
      'warn',
      {
        publicOnly: true,
      },
    ],
    'jsdoc/require-param': 'off',
    'jsdoc/require-param-type': 'off',
    'jsdoc/require-returns': 'off',
    'jsdoc/require-returns-type': 'off',
    'jsdoc/require-yields': 'off',
  },
}, {
  files: [
    'eslint.config.mjs',
  ],
  languageOptions: {
    globals: {
      ...globals.node,
    },
  },
  settings: {
    'import-x/resolver': {
      node: true,
    },
  },
}, {
  files: [
    '**/*.ts',
  ],
  extends: [
    ...tseslint.configs.strictTypeChecked,
    ...tseslint.configs.stylisticTypeChecked,
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
  rules: {
    '@typescript-eslint/array-type': [
      'error',
      {
        default: 'generic',
      },
    ],
    '@typescript-eslint/explicit-function-return-type': [
      'error',
      {
        allowExpressions: true,
      },
    ],
    '@typescript-eslint/naming-convention': 'error',
    '@typescript-eslint/prefer-readonly': 'error',
    '@typescript-eslint/promise-function-async': 'error',
    '@typescript-eslint/unbound-method': [
      'error',
      {
        ignoreStatic: true,
      },
    ],

    'no-shadow': 'off',
    '@typescript-eslint/no-shadow': 'error',

    'default-param-last': 'off',
    '@typescript-eslint/default-param-last': 'error',
  },
}, {
  files: [
    'src/**/*.ts',
    'projects/**/*.ts',
  ],
  extends: [
    ...angular.configs.tsRecommended,
  ],
  processor: angular.processInlineTemplates,
  rules: {
    '@angular-eslint/directive-selector': [
      'error',
      {
        type: 'attribute',
        prefix: 'app',
        style: 'camelCase',
      },
    ],
    '@angular-eslint/component-selector': [
      'error',
      {
        type: 'element',
        prefix: 'app',
        style: 'kebab-case',
      },
    ],
    '@typescript-eslint/no-extraneous-class': [
      'error',
      {
        allowWithDecorator: true,
      },
    ],
  },
}, {
  files: [
    '**/*.html',
  ],
  ignores: [
    '**/index.html',
  ],
  extends: [
    ...angular.configs.templateAll,
  ],
  rules: {
    '@angular-eslint/template/attributes-order': 'off',
    '@angular-eslint/template/i18n': 'off',
    // TODO: this conflicts with signals.
    '@angular-eslint/template/no-call-expression': 'off',
  },
}, {
  files: [
    '**/*.spec.ts',
  ],
  extends: [
    jest.configs['flat/recommended'],
    jest.configs['flat/style'],
  ],
  languageOptions: {
    globals: {
      ...globals.jest,
    },
  },
}, {
  files: [
    'cypress/**/*.ts',
  ],
  extends: [
    cypress.configs.recommended,
  ],
});
