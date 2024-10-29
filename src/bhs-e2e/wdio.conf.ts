import * as core from '@actions/core';

export const config: WebdriverIO.Config = {
  specs: [
    './test/specs/**/*.ts',
  ],

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

  logLevel: 'info',
  bail: 0,
  reporters: ['spec'],

  framework: 'jasmine',
  jasmineOpts: {},
};
