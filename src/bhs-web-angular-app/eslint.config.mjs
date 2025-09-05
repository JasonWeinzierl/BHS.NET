/* eslint-disable import-x/no-named-as-default-member */
/* eslint-disable import-x/no-rename-default */
// @ts-check
import js from '@eslint/js';
import stylistic from '@stylistic/eslint-plugin';
import vitest from '@vitest/eslint-plugin';
import angular from 'angular-eslint';
import gitignore from 'eslint-config-flat-gitignore';
import { importX } from 'eslint-plugin-import-x';
import jsdoc from 'eslint-plugin-jsdoc';
import rxjsAngularX from 'eslint-plugin-rxjs-angular-x';
import rxjsX from 'eslint-plugin-rxjs-x';
import globals from 'globals';
import tseslint from 'typescript-eslint';

// TODO: tseslint.config() is now deprecated, replace with eslint/config defineConfig().
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
    // #region General
    // Maximum cyclomatic complexity.
    'complexity': [
      'warn',
      15,
    ],

    // Consistent brace style.
    'curly': [
      'error',
      'all',
    ],

    // Warn against TODOs.
    'no-warning-comments': 'warn',

    // Require the 2nd arg of parseInt so we don't get unexpected hexadecimal.
    'radix': 'error',

    // Bitwise operators are usually a bug.
    'no-bitwise': 'error',

    // Double-equals is usually a bug.
    'eqeqeq': [
      'error',
      'smart',
    ],

    // Throwing a non-error is unexpected. Note the TypeScript equivalent only-throw-error is included in their config.
    'no-throw-literal': 'error',
    // #endregion General

    // #region Rules with TypeScript equivalents
    // Require default parameters to be last.
    'default-param-last': 'error',

    // Shadowing may cause bugs when refactoring.
    'no-shadow': 'error',
    // #endregion Rules with TypeScript equivalents

    // #region Stylistic
    // Don't need parenthesis around arrow function args.
    '@stylistic/arrow-parens': 'off',

    // Indent rules are too strict.
    '@stylistic/indent': 'off',
    '@stylistic/indent-binary-ops': 'off',

    // The stylistic config enables keyword-spacing but we want its counterpart for functions.
    '@stylistic/function-call-spacing': 'error',

    // Too strict.
    '@stylistic/multiline-ternary': 'off',

    // Remove unnecessary semicolons.
    '@stylistic/no-extra-semi': 'error',

    // The defaults of these rules are unconventional.
    '@stylistic/generator-star-spacing': [
      'error',
      'after',
    ],
    '@stylistic/yield-star-spacing': [
      'error',
      'after',
    ],
    // #endregion Stylistic

    // #region Imports
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
    // #endregion Imports

    // #region JSDoc
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
    // #endregion JSDoc
  },
}, {
  // #region Config and Script files
  files: [
    'eslint.config.mjs',
    'scripts/**/*.mjs',
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
  // #endregion Config and Script files
}, {
  files: [
    '**/*.ts',
  ],
  extends: [
    ...tseslint.configs.strictTypeChecked,
    ...tseslint.configs.stylisticTypeChecked,
    rxjsX.configs.strict,
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
  plugins: {
    'rxjs-angular-x': rxjsAngularX,
  },
  rules: {
    // #region RxJS
    'rxjs-x/finnish': [
      'warn',
      {
        methods: false,
        strict: true,
        names: {
          // Add paramMap|data to defaults for ActivatedRoute.
          '^(canActivate|canActivateChild|canDeactivate|canLoad|intercept|resolve|validate|paramMap|data)$': false,
        },
      },
    ],
    'rxjs-x/suffix-subjects': 'warn',
    'rxjs-x/no-ignored-subscribe': 'warn',
    'rxjs-x/no-ignored-subscription': 'warn',
    'rxjs-x/no-subject-value': 'error',

    'rxjs-angular-x/prefer-async-pipe': 'error',
    // #endregion RxJS
    // #region TypeScript
    // Use Array<T> for all types.
    '@typescript-eslint/array-type': [
      'error',
      {
        default: 'generic',
      },
    ],

    // Strict about return types.
    '@typescript-eslint/explicit-function-return-type': [
      'error',
      {
        allowExpressions: true,
      },
    ],

    // Add readonly where possible.
    '@typescript-eslint/prefer-readonly': 'error',

    // Returning un-awaited promises is unnecessary optimization.
    '@typescript-eslint/promise-function-async': 'error',

    // Allow static unbound methods.
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
    // #endregion TypeScript
  },
}, {
  // #region Angular
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
    '@angular-eslint/prefer-signals': [
      'error',
      {
        useTypeChecking: true,
      },
    ],

    // Allow Angular modules with only a decorator.
    '@typescript-eslint/no-extraneous-class': [
      'error',
      {
        allowWithDecorator: true,
      },
    ],
  },
  // #endregion Angular
}, {
  // #region Angular templates
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
    // Attribute ordering is too opinionated.
    '@angular-eslint/template/attributes-order': 'off',

    // This project doesn't use Angular's i18n.
    '@angular-eslint/template/i18n': 'off',

    // TODO: this conflicts with signals.
    '@angular-eslint/template/no-call-expression': 'off',

    // Increase for new Angular control flow syntax.
    '@angular-eslint/template/cyclomatic-complexity': [
      'error',
      {
        maxComplexity: 10,
      },
    ],
  },
  // #endregion Angular templates
}, {
  // #region Unit tests
  files: [
    '**/*.spec.ts',
  ],
  extends: [
    vitest.configs.recommended,
  ],
  settings: {
    vitest: {
      typecheck: true,
    },
  },
  rules: {
    // Allow expect(unbound methods).
    '@typescript-eslint/unbound-method': 'off',
    // 'vitest/unbound-method': 'error', // Not finished in their plugin yet. #591

    // Require everything to be wrapped in describe().
    'vitest/require-top-level-describe': 'error',

    // All formatting.
    'vitest/padding-around-all': 'error',
  },
  // #endregion Unit tests
});
