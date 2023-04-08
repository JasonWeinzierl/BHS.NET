import { createBanner, createPhotoAlbum } from 'cypress/factories/dataFactories';

describe('Photos', () => {
  beforeEach(() => {
    cy.intercept('GET', '/api/banners/current', [ createBanner() ]).as('currentBanners');
  });

  it('should navigate to Photos', () => {
    cy.intercept('GET', '/api/photos/albums', [ createPhotoAlbum() ]).as('photosAlbums');

    cy.visit('/');
    cy.get('.nav-item > a').contains('Content').click();
    cy.contains('Photos').click();

    cy.get('h1').should('contain.text', 'Photo Albums');
  });
});
