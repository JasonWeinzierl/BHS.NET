import * as core from '@actions/core';
import getRunnerEnvironment from '../get-runner-environment';
import adminPage from '../pageobjects/admin.page';

class LoginRobot {
  async loginFromEnvironmentVariables() {
    const runnerEnvironment = getRunnerEnvironment();

    const baseUrl = browser.options.baseUrl;
    if (!baseUrl) {
      throw new Error('baseUrl is not set.');
    }

    await browser.url(baseUrl + '/admin');

    const result = await browser.waitUntil(async () => {
      const url = await browser.getUrl();
      if (url.startsWith(`https://${runnerEnvironment.E2E_auth0Domain}/`)) {
        return 'ready-for-login';
      } else if (await adminPage.self.isDisplayed()) {
        return 'logged-in';
      }

      return false;
    }, {
      timeout: 100_000,
      timeoutMsg: 'Timed out waiting for Auth0 redirect.',
    });

    if (result === 'logged-in') {
      core.notice('User is already logged in; skipping.');
      return;
    }

    await $('input#username').setValue(runnerEnvironment.E2E_auth0TestUsername);
    await $('input#password').setValue(runnerEnvironment.E2E_auth0TestPassword);
    await $('button[value=default], button[type=submit]').click();

    await browser.waitUntil(async () => {
      const url = await browser.getUrl();
      return url.startsWith(baseUrl);
    }, { timeoutMsg: 'Timed out waiting for redirect after Auth0 login.' });

    await adminPage.self.waitForDisplayed({
      timeoutMsg: 'Timed out waiting for the authenticated admin page.',
    });
  }
}
export default new LoginRobot();
