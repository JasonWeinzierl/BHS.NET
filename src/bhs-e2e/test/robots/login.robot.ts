import * as core from '@actions/core';
import getRunnerEnv from '../get-runner-env';

class LoginRobot {
  async loginFromEnvironmentVariables() {
    const runnerEnv = getRunnerEnv();

    if (await this.isUserLoggedIn(runnerEnv.E2E_auth0ClientId)) {
      core.notice('User is already logged in; skipping.');
      return;
    }

    const baseUrl = browser.options.baseUrl;
    if (!baseUrl) {
      throw new Error('baseUrl is not set.');
    }

    await browser.url(baseUrl + '/admin');

    await browser.waitUntil(async () => {
      return (await browser.getUrl()).startsWith(`https://${runnerEnv.E2E_auth0Domain}/`);
    }, { timeoutMsg: 'Timed out waiting for redirect to Auth0 login page.' });

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
