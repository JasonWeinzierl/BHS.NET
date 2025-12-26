import * as core from '@actions/core';
import { Agent, fetch } from 'undici';

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

    expect(response.status).toBe(200);

    const body = await response.text();

    expect(['Healthy', 'Degraded']).toContain(body);

    if (body === 'Degraded') {
      core.warning('The health check reports Degraded.');
    }
  });

  it('should authenticate with Auth0', async () => {
    if (!browser.options.baseUrl) {
      throw new Error('baseUrl is not set.');
    }

    await browser.url('/');

    await browser.login();

    // Go to Admin page after login.
    await browser.url('/admin');

    await $('[data-testid="logout"]').click();

    await expect(browser).toHaveUrl(expect.stringContaining(browser.options.baseUrl));
  });
});
