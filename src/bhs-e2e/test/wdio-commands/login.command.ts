import getRunnerEnv from '../get-runner-env';

export default async function loginCommand(this: WebdriverIO.Browser): Promise<void> {
  const runnerEnv = getRunnerEnv();

  if (!browser.options.baseUrl) {
    throw new Error('baseUrl is not set.');
  }

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
}
