import { FactoryProvider } from '@angular/core';
import { MarkedOptions, MarkedRenderer } from 'ngx-markdown';

const markedOptionsFactory = (): MarkedOptions => {
  const renderer = new MarkedRenderer();

  renderer.image = (href: string | null, title: string | null, text: string): string => {
    if (!href) {
      return text;
    }

    const encodedSrc = encodeURI(href).replace(/%25/g, '%');
    if (!encodedSrc) {
      return text;
    }

    let out = '<img src="' + encodedSrc + '" alt="' + text + '"';
    if (title) {
      out += ' title="' + title + '"';
    }
    out += ' class="img-fluid"';
    out += '>';
    return out;
  };

  return {
    renderer,
  };
};

/**
 * Provider for marked.js options with bootstrap styles added.
 */
export const bootstrapMarkedOptionsProvider: FactoryProvider = {
  provide: MarkedOptions,
  useFactory: markedOptionsFactory,
};
