
describe('Liveness', () => {

  it('should pass the health check', () => {
    cy.request('/api/healthcheck/status').its('body').should('equal', 'Healthy');
  });

  it('should authenticate with Auth0', () => {
    cy.visit('/');

    cy.login();

    cy.visit('/admin');

    cy.contains('Log out').click();

    cy.url().should('contain', Cypress.config().baseUrl);
  });

});
