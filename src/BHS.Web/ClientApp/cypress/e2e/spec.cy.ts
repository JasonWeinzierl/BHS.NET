describe('ClientApp', () => {
  beforeEach(() => {
    cy.intercept('GET', '/api/banners/current', { fixture: 'currentBanners' }).as('currentBanners');
  });

  it('should display welcome message', () => {
    cy.visit('/');
    cy.get('app-root h1').should('contain.text', 'The City Hall Museum sponsored by the Belton Historical Society');
  });

  it('should navigate to News', () => {
    cy.intercept('GET', '/api/blog/posts', { fixture: 'blogPosts' }).as('blogPosts');
    cy.intercept('GET', '/api/blog/categories', { fixture: 'blogCategories' }).as('blogCategories');

    cy.visit('/');
    cy.get('.nav-item > a').contains('News').click();

    cy.get('h1').should('contain.text', 'News');
  });

  it('should navigate to Photos', () => {
    cy.intercept('GET', '/api/photos/albums', { fixture: 'photosAlbums' }).as('photosAlbums');

    cy.visit('/');
    cy.get('.nav-item > a').contains('Content').click();
    cy.contains('Photos').click();

    cy.get('h1').should('contain.text', 'Photo Albums');
  });
});