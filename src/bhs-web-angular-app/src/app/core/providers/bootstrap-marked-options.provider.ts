import { Provider } from '@angular/core';
import { MARKED_OPTIONS, MarkedOptions, MarkedRenderer, provideMarkdown } from 'ngx-markdown';

/**
 * marked.js options with bootstrap styles added.
 */
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
 * Initialize our config for ngx-markdown.
 */
export function provideBhsMarkdown(): Array<Provider> {
  return provideMarkdown({
    markedOptions: {
      provide: MARKED_OPTIONS,
      useFactory: markedOptionsFactory,
    },
  });
}
