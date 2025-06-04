import { makeEnvironmentProviders } from '@angular/core';
import { Mock, vi } from 'vitest';

// jsdom doesn't implement scrollIntoView.
if (window.navigator.userAgent.includes('jsdom')) {
  Object.defineProperties(window.Element.prototype, {
    _mockScrollIntoView: {
      writable: true,
      configurable: true,
    },
    scrollIntoView: {
      configurable: true,
      enumerable: true,
      get(this: Window['Element']) {
        this._mockScrollIntoView ??= vi.fn();
        return this._mockScrollIntoView;
      },
    },
  });
}

declare global {
  interface Window {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    Element: {
      _mockScrollIntoView?: Mock;
    };
  }
}

// angular.json providersFile expects a default export of providers.
export default makeEnvironmentProviders([]);
