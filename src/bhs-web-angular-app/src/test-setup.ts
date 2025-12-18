import { EnvironmentProviders } from '@angular/core';
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
    Element: {
      _mockScrollIntoView?: Mock;
    };
  }
}

export default [] satisfies Array<EnvironmentProviders>;
