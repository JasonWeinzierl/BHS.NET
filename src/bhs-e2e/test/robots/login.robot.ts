import * as core from '@actions/core';
import getRunnerEnvironment from '../get-runner-environment';

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
      } else if (await this.isUserLoggedIn(runnerEnvironment.E2E_auth0ClientId)) {
        return 'logged-in';
      } else {
        await browser.url(baseUrl + '/admin');
        return false;
      }
    }, {
      timeout: 100_000,
      interval: 2000,
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

    await browser.waitUntil(async () => {
      return this.isUserLoggedIn(runnerEnvironment.E2E_auth0ClientId);
    }, { timeoutMsg: 'Timed out waiting for Auth0 to persist to storage.' });
  }

  private async isUserLoggedIn(auth0ClientId: string): Promise<boolean> {
    return (await browser.execute(clientId => {
      // eslint-disable-next-line n/no-unsupported-features/node-builtins -- this is running in the browser.
      return localStorage.getItem(`@@auth0spajs@@::${clientId}::@@user@@`);
    }, auth0ClientId)) !== null;
  }
}
export default new LoginRobot();
