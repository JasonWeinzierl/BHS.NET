
describe('Home', () => {

  it('should display welcome message', () => {

    cy.visit('/');

    cy.get('app-root h1').should('contain.text', 'The City Hall Museum sponsored by the Belton Historical Society');
  });

  it('should load banners', () => {

    cy.visit('/');
    cy.wait('@currentBanners');

    cy.get('alert').should('exist');
  });

});
