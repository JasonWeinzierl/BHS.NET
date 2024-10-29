import * as core from '@actions/core';
import { Agent, fetch } from 'undici';

const { auth0Domain, auth0ClientId, username, password } = {
  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  auth0Domain: process.env.E2E_auth0Domain!,
  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  auth0ClientId: process.env.E2E_auth0ClientId!,
  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  username: process.env.E2E_auth0TestUsername!,
  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  password: process.env.E2E_auth0TestPassword!,
};

describe('liveness', () => {
  it('should pass the health check', async () => {
    if (!browser.options.baseUrl) {
      throw new Error('baseUrl is not set.');
    }

    const response = await fetch(browser.options.baseUrl + '/api/healthcheck/status', {
      dispatcher: new Agent({
        connect: {
          rejectUnauthorized: false,
        },
      }),
    });

    await expect(response.status).toBe(200);

    const body = await response.text();

    await expect(['Healthy', 'Degraded']).toContain(body);

    if (body === 'Degraded') {
      core.warning('The health check reports Degraded.');
    }
  });

  it('should authenticate with Auth0', async () => {
    if (!browser.options.baseUrl) {
      throw new Error('baseUrl is not set.');
    }

    await browser.url('/');

    // Login.
    await browser.url('/admin');

    await expect(browser).toHaveUrl(expect.stringContaining(`https://${auth0Domain}/`));

    await $('input#username').setValue(username);
    await $('input#password').setValue(password);
    await $('button[value=default], button[type=submit').click();

    await expect(browser).toHaveUrl(expect.stringContaining(browser.options.baseUrl));

    await browser.waitUntil(async () => {
      return (await browser.execute(clientId => {
        return localStorage.getItem(`@@auth0spajs@@::${clientId}::@@user@@`);
      }, auth0ClientId)) !== null;
    });

    // Go to Admin page after login.
    await browser.url('/admin');

    await $('button=Log out').click();

    await expect(browser).toHaveUrl(expect.stringContaining(browser.options.baseUrl));
  });
});
