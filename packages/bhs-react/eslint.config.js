// https://docs.expo.dev/guides/using-eslint/
const { defineConfig, globalIgnores } = require('eslint/config');
const expoConfig = require('eslint-config-expo/flat');
const tailwindcss = require('eslint-plugin-better-tailwindcss');
const packageJson = require('eslint-plugin-package-json');

module.exports = defineConfig([
  globalIgnores(['dist/*']),
  expoConfig,
  {
    extends: [
      tailwindcss.configs.recommended,
    ],
    settings: {
      'better-tailwindcss': {
        entryPoint: './global.css',
      },
    },
    rules: {
      'better-tailwindcss/enforce-consistent-line-wrapping': 'off',
      // Uniwind changes.
      // See: https://docs.uniwind.dev/class-names
      'better-tailwindcss/no-unknown-classes': [
        'error',
        {
          ignore: [
            '^ios:.*',
            '^android:.*',
            '^web:.*',
            '.*-safe$',
          ],
        },
      ],
    },
  },
  {
    files: [
      'package.json',
    ],
    extends: [
      packageJson.configs.recommended,
      packageJson.configs.stylistic,
    ],
  }
]);
