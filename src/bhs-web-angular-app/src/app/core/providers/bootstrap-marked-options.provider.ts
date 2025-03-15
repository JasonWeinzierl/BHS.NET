import { EnvironmentProviders, makeEnvironmentProviders } from '@angular/core';
import { Tokens } from 'marked';
import { MARKED_OPTIONS, MarkedOptions, MarkedRenderer, provideMarkdown } from 'ngx-markdown';

/**
 * Initialize our config for ngx-markdown.
 */
export function provideBhsMarkdown(): EnvironmentProviders {
  return makeEnvironmentProviders([
    provideMarkdown({
      markedOptions: {
        provide: MARKED_OPTIONS,
        useFactory: (): MarkedOptions => {
          // marked.js options with bootstrap styles added.

          const renderer = new MarkedRenderer();

          renderer.image = ({ href, title, text }: Tokens.Image): string => {
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
        },
      },
    }),
  ]);
}
