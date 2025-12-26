import * as core from '@actions/core';

const defaultRunnerEnv: Record<string, string> = {
  E2E_auth0Domain: 'dev-wz656qr7u8q11d84.us.auth0.com',
  E2E_auth0ClientId: 'MEQJ4p2Y10CLMRebfZAEV2o2zxJVNFk7',
  // These credentials do not work on any published app; this Auth0 tenant is for local development only.
  E2E_auth0TestUsername: 'noreply@beltonhistoricalsociety.org',
  E2E_auth0TestPassword: 'This password is fine to publish!',
};

export const config: WebdriverIO.Config = {
  specs: [
    './test/specs/**/*.ts',
  ],

  // This will be overridden by wdio --baseUrl=http://example.com
  baseUrl: 'https://localhost:5001', // When running locally, requires the backend api and mongo database to be running.

  maxInstances: 10,
  capabilities: [{
    browserName: 'chrome',
    'goog:chromeOptions': {
      args: ['headless', 'disable-gpu'],
    },
  }, {
    browserName: 'firefox',
    'moz:firefoxOptions': {
      args: ['-headless'],
    },
  }, {
    browserName: 'safari',
  }, {
    browserName: 'edge',
    'ms:edgeOptions': {
      args: ['--headless'],
    },
  }],

  onPrepare(cfg, capabilities) {
    if (!core.platform.isMacOS && Array.isArray(capabilities)) {
      core.warning(`Safari is not available on ${core.platform.platform}. Removing Safari capabilities.`);
      const safariIndex = capabilities.findIndex(cap => 'browserName' in cap && cap.browserName === 'safari');
      capabilities.splice(safariIndex, 1);
    }

    cfg.runnerEnv = {};
    for (const key in defaultRunnerEnv) {
      // eslint-disable-next-line n/no-process-env
      if (process.env[key] === undefined) {
        cfg.runnerEnv[key] = defaultRunnerEnv[key];
      }
    }
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
