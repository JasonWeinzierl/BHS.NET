import loginCommand from './login.command';

declare global {
  // eslint-disable-next-line @typescript-eslint/no-namespace
  namespace WebdriverIO {
    interface Browser {
      login: typeof loginCommand;
    }
  }
}

export default function registerCustomCommands(browser: WebdriverIO.Browser) {
  browser.addCommand('login', loginCommand);
}
