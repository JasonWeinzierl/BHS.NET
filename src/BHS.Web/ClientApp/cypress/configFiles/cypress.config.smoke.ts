import { defineConfig } from 'cypress';

export default defineConfig({

  e2e: {
    // This will be overridden first by --config baseUrl=http://example.com and then by CYPRESS_BASE_URL.
    baseUrl: 'http://localhost:4200/',
    specPattern: 'cypress/smokeTests/**/*.cy.ts',
  },

});
