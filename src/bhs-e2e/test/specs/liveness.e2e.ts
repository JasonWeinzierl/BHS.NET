import * as core from '@actions/core';
import { Agent, fetch } from 'undici';
import runnerEnv from '../runnerEnv';

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

    await expect(browser).toHaveUrl(expect.stringContaining(`https://${runnerEnv.E2E_auth0Domain}/`));

    await $('input#username').setValue(runnerEnv.E2E_auth0TestUsername);
    await $('input#password').setValue(runnerEnv.E2E_auth0TestPassword);
    await $('button[value=default], button[type=submit').click();

    await expect(browser).toHaveUrl(expect.stringContaining(browser.options.baseUrl));

    await browser.waitUntil(async () => {
      return (await browser.execute(clientId => {
        return localStorage.getItem(`@@auth0spajs@@::${clientId}::@@user@@`);
      }, runnerEnv.E2E_auth0ClientId)) !== null;
    });

    // Go to Admin page after login.
    await browser.url('/admin');

    await $('button=Log out').click();

    await expect(browser).toHaveUrl(expect.stringContaining(browser.options.baseUrl));
  });
});
