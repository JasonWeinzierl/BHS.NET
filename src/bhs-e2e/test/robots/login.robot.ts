import getRunnerEnv from '../get-runner-env';
import adminPage from '../pageobjects/admin.page';

class LoginRobot {
  async loginFromEnvironmentVariables() {
    const runnerEnv = getRunnerEnv();

    const baseUrl = browser.options.baseUrl;
    if (!baseUrl) {
      throw new Error('baseUrl is not set.');
    }

    await adminPage.open();

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
      return (await browser.execute(clientId => {
        return localStorage.getItem(`@@auth0spajs@@::${clientId}::@@user@@`);
      }, runnerEnv.E2E_auth0ClientId)) !== null;
    }, { timeoutMsg: 'Timed out waiting for Auth0 to persist to storage.' });
  }
}
export default new LoginRobot();
