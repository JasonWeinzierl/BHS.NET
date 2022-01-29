describe('App', () => {
  it('should display welcome message', () => {
    cy.visit('/');
    cy.get('app-root h1').should('contain.text', 'The City Hall Museum sponsored by the Belton Historical Society');
  });

  it('should navigate to News', () => {
    cy.visit('/');
    cy.contains('News').click();
    cy.get('h1').should('contain.text', 'News');
  });
});
