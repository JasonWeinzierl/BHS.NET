// @ts-check
const js = require('@eslint/js');
const stylistic = require('@stylistic/eslint-plugin');
const angular = require('angular-eslint');
const cypress = require('eslint-plugin-cypress/flat');
const importX = require('eslint-plugin-import-x');
const jest = require('eslint-plugin-jest');
const jsdoc = require('eslint-plugin-jsdoc');
const globals = require('globals');
const tseslint = require('typescript-eslint');

// TODO: re-add these plugins when they support eslint v9:
// - rxjs

module.exports = tseslint.config({
  files: [
    '**/*.js',
    '**/*.ts',
  ],
  ignores: [
    'dist/**',
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
    // @ts-expect-error -- TODO: cjs problem with jsdoc plugin
    jsdoc.configs['flat/recommended-typescript-error'],
  ],
  plugins: {
    'import-x': importX,
  },
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
    'import-x/no-useless-path-segments': 'warn',
    // TODO: import-x doesn't support flat config yet, so we're enabling these rules manually
    ...importX.configs.recommended.rules,
    ...importX.configs.typescript.rules,

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
    'eslint.config.js',
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
