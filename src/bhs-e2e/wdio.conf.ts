import * as core from '@actions/core';
import registerCustomCommands from './test/wdio-commands/register-custom-commands';

export const config: WebdriverIO.Config = {
  specs: [
    './test/specs/**/*.ts',
  ],

  // This will be overridden by wdio --baseUrl=http://example.com
  baseUrl: 'https://localhost:5001', // When running locally, requires the backend api and mongo database to be running.
  runnerEnv: {
    // These will be overridden by environment variables.
    E2E_auth0Domain: 'dev-wz656qr7u8q11d84.us.auth0.com',
    E2E_auth0ClientId: 'MEQJ4p2Y10CLMRebfZAEV2o2zxJVNFk7',
    E2E_auth0Audience: 'https://beltonhistoricalsociety.org/api/swagger/index.html',
    // These credentials do not work on any published app; this Auth0 tenant is for local development only.
    E2E_auth0TestUsername: 'noreply@beltonhistoricalsociety.org',
    E2E_auth0TestPassword: 'This password is fine to publish!',
  },

  maxInstances: 10,
  capabilities: [{
    browserName: 'chrome',
  }, {
    browserName: 'firefox',
  }, {
    browserName: 'safari',
  }, {
    browserName: 'edge',
  }],

  onPrepare(_cfg, capabilities) {
    if (!core.platform.isMacOS && Array.isArray(capabilities)) {
      core.warning(`Safari is not available on ${core.platform.platform}. Removing Safari capabilities.`);
      const safariIndex = capabilities.findIndex(cap => 'browserName' in cap && cap.browserName === 'safari');
      capabilities.splice(safariIndex, 1);
    }
  },

  before(_capabilities, _specs, browser) {
    registerCustomCommands(browser as WebdriverIO.Browser);
  },

  logLevel: 'info',
  bail: 0,
  reporters: [
    'spec',
    // TODO: consider adding wdio-video-reporter
  ],
  waitforTimeout: 10_000,

  framework: 'jasmine',
  jasmineOpts: {
    defaultTimeoutInterval: 120_000,
  },
};
