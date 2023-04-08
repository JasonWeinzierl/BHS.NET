import { createBanner, createBlogPostPreview, createCategorySummary } from 'cypress/factories/dataFactories';

describe('News', () => {
  beforeEach(() => {
    cy.intercept('GET', '/api/banners/current', [ createBanner() ]).as('currentBanners');
  });

  it('should navigate to News', () => {
    cy.intercept('GET', '/api/blog/posts', [ createBlogPostPreview() ]).as('blogPosts');
    cy.intercept('GET', '/api/blog/categories', [ createCategorySummary() ]).as('blogCategories');

    cy.visit('/');
    cy.get('.nav-item > a').contains('News').click();

    cy.get('h1').should('contain.text', 'News');
  });

  it('should display each post preview', () => {
    const preview1 = createBlogPostPreview();
    cy.intercept('GET', '/api/blog/posts', [ preview1, createBlogPostPreview(), createBlogPostPreview() ]).as('blogPosts');
    const category1 = createCategorySummary();
    cy.intercept('GET', '/api/blog/categories', [ category1, createCategorySummary() ]).as('blogCategories');

    cy.visit('/apps/blog');
    cy.wait('@blogPosts');

    cy.get('app-post-card').should('have.length', 3);
    cy.get(`[data-testid=${preview1.slug}]`).contains('.card-title', preview1.title);
    cy.get(`[data-testid=${preview1.slug}]`).contains('.card-body', preview1.contentPreview);
    cy.get(`[data-testid=${preview1.slug}]`).contains('.card-subtitle', preview1.datePublished.getFullYear());
    if (preview1.author) {
      cy.get(`[data-testid=${preview1.slug}]`).contains('.card-subtitle', preview1.author.name);
    }
  });
});
