import * as core from '@actions/core';
import { Agent, fetch } from 'undici';
import adminPage from '../pageobjects/admin.page';
import homePage from '../pageobjects/home.page';
import loginRobot from '../robots/login.robot';

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
  }, 61_000); // The site can take a while to start up.

  it('should login and logout', async () => {
    await loginRobot.loginFromEnvironmentVariables();

    // Go to Admin page after login.
    await adminPage.open();

    await adminPage.logoutLink.click();

    await expect(homePage.self).toBeDisplayed();
  }, 121_000);
});
