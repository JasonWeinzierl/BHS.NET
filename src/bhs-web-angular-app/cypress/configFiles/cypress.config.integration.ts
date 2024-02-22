import { defineConfig } from 'cypress';

export default defineConfig({

  e2e: {
    baseUrl: 'http://localhost:4200',
    specPattern: 'cypress/integrationTests/**/*.cy.ts',
    env: {
      auth0Domain: 'dev-wz656qr7u8q11d84.us.auth0.com',
      auth0ClientId: 'MEQJ4p2Y10CLMRebfZAEV2o2zxJVNFk7',
      auth0Audience: 'https://beltonhistoricalsociety.org/api/swagger/index.html',
      // These credentials do not work on any published app; this Auth0 tenant is for local development only.
      auth0TestUsername: 'noreply@beltonhistoricalsociety.org',
      auth0TestPassword: 'This password is fine to publish!',
    },
  },

});
