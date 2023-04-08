
describe('Liveness', () => {

  it('should pass the health check', () => {
    cy.request('/api/healthcheck/status').its('body').should('equal', 'Healthy');
  });

});
