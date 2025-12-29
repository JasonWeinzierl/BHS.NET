import * as core from '@actions/core';
import getRunnerEnv from '../get-runner-env';

class LoginRobot {
  async loginFromEnvironmentVariables() {
    const runnerEnv = getRunnerEnv();

    const baseUrl = browser.options.baseUrl;
    if (!baseUrl) {
      throw new Error('baseUrl is not set.');
    }

    await browser.url(baseUrl + '/admin');

    const result = await browser.waitUntil(async () => {
      if ((await browser.getUrl()).startsWith(`https://${runnerEnv.E2E_auth0Domain}/`)) {
        return 'ready-for-login';
      } else if (await this.isUserLoggedIn(runnerEnv.E2E_auth0ClientId)) {
        return 'logged-in';
      } else {
        return false;
      }
    }, {
      timeout: 50_000,
      interval: 1000,
      timeoutMsg: 'Timed out waiting for Auth0 redirect or already logged in.',
    });

    if (result === 'logged-in') {
      core.notice('User is already logged in; skipping.');
      return;
    }

    await $('input#username').setValue(runnerEnv.E2E_auth0TestUsername);
    await $('input#password').setValue(runnerEnv.E2E_auth0TestPassword);
    await $('button[value=default], button[type=submit]').click();

    await browser.waitUntil(async () => {
      return (await browser.getUrl()).startsWith(baseUrl);
    }, { timeoutMsg: 'Timed out waiting for redirect after Auth0 login.' });

    await browser.waitUntil(async () => {
      return this.isUserLoggedIn(runnerEnv.E2E_auth0ClientId);
    }, { timeoutMsg: 'Timed out waiting for Auth0 to persist to storage.' });
  }

  private async isUserLoggedIn(auth0ClientId: string): Promise<boolean> {
    return (await browser.execute(clientId => {
      return localStorage.getItem(`@@auth0spajs@@::${clientId}::@@user@@`);
    }, auth0ClientId)) !== null;
  }
}
export default new LoginRobot();
