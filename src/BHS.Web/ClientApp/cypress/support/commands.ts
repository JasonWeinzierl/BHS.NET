const { auth0Domain, auth0ClientId, username, password } = {
  auth0Domain: Cypress.env('auth0Domain') as string,
  auth0ClientId: Cypress.env('auth0ClientId') as string,
  username: Cypress.env('auth0TestUsername') as string,
  password: Cypress.env('auth0TestPassword') as string,
};

Cypress.Commands.add('login', () => {
  const log = Cypress.log({
    displayName: 'Auth0 Login',
    message: [`Authenticating | ${username}`],
    autoEnd: false,
  });
  log.snapshot('before');

  cy.session(
    `auth0-${username}`,
    () => {
      cy.visit('/admin');

      cy.origin(
        `https://${auth0Domain}/`,
        { args: { username, password } },
        ({ username, password }) => {
          cy.get('input#username').type(username);
          cy.get('input#password').type(password, { log: false });
          cy.contains('button[value=default]:visible', 'Continue').click();
        },
      );

      cy.url().should('include', Cypress.config().baseUrl);
    },
    {
      validate: () => {
        cy.wrap(localStorage)
          .invoke('getItem', `@@auth0spajs@@::${auth0ClientId}::@@user@@`)
          .should('exist');
      },
    },
  );

  log.snapshot('after');
  log.end();
});
