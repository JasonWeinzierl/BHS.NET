// https://docs.expo.dev/guides/using-eslint/
const { defineConfig } = require('eslint/config');
const expoConfig = require('eslint-config-expo/flat');
const tailwindcss = require('eslint-plugin-better-tailwindcss');

module.exports = defineConfig([
  expoConfig,
  {
    ignores: ['dist/*'],
  },
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
]);
