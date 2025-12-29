import getRunnerEnv from '../get-runner-env';
import adminPage from '../pageobjects/admin.page';

class LoginRobot {
  async loginFromEnvironmentVariables() {
    const runnerEnv = getRunnerEnv();

    if (!browser.options.baseUrl) {
      throw new Error('baseUrl is not set.');
    }

    await adminPage.open();

    await expect(browser).toHaveUrl(expect.stringContaining(`https://${runnerEnv.E2E_auth0Domain}/`));

    await $('input#username').setValue(runnerEnv.E2E_auth0TestUsername);
    await $('input#password').setValue(runnerEnv.E2E_auth0TestPassword);
    await $('button[value=default], button[type=submit]').click();

    await expect(browser).toHaveUrl(expect.stringContaining(browser.options.baseUrl));

    await browser.waitUntil(async () => {
      return (await browser.execute(clientId => {
        return localStorage.getItem(`@@auth0spajs@@::${clientId}::@@user@@`);
      }, runnerEnv.E2E_auth0ClientId)) !== null;
    }, { timeoutMsg: 'Timed out waiting for Auth0 to persist to storage.' });
  }
}
export default new LoginRobot();
