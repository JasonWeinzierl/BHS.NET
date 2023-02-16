import { createBanner } from 'cypress/factories/dataFactories';

describe('Home', () => {
  beforeEach(() => {
    cy.intercept('GET', '/api/banners/current', [ createBanner() ]).as('currentBanners');
  });

  it('should display welcome message', () => {
    cy.visit('/');
    cy.get('app-root h1').should('contain.text', 'The City Hall Museum sponsored by the Belton Historical Society');
  });
});
