
describe('Liveness', () => {

  it('should pass the health check', () => {
    cy.request('/api/healthcheck/status').then(response => {
      expect(response.status).to.equal(200);
      expect(response.body).to.be.oneOf(['Healthy', 'Degraded']);

      if (response.body === 'Degraded') {
        cy.log('WARNING: The health check reports Degraded.');
      }
    });
  });

  it('should authenticate with Auth0', () => {
    cy.visit('/');

    cy.login();

    cy.visit('/admin');

    cy.contains('Log out').click();

    cy.url().should('contain', Cypress.config().baseUrl);
  });

});
