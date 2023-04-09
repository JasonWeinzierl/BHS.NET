import './commands';
import { createBanner } from 'cypress/factories/dataFactories';

beforeEach(() => {
  cy.intercept('GET', '/api/banners/current', [ createBanner() ]).as('currentBanners');
});
