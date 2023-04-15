import { AppEnvironment } from '../../src/environments/app-environment';

describe('Admin', () => {

  beforeEach(() => {
    const appEnv: AppEnvironment = {
      auth0: {
        domain: Cypress.env('auth0Domain'),
        clientId: Cypress.env('auth0ClientId'),
        authorizationParams: {
          audience: Cypress.env('auth0Audience'),
        },
      },
    };
    cy.intercept('GET', '/api/client-app-environment', appEnv).as('clientAppEnvironment');
  });

  it('should login', () => {
    cy.visit('/');

    cy.login();

    cy.visit('/admin');
  });

  it('should logout', () => {
    cy.login();
    cy.visit('/admin');

    cy.contains('Log out').click();

    cy.url().should('contain', Cypress.config().baseUrl);
  });

});
