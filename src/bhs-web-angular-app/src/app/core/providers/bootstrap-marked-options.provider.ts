import { InjectionToken } from '@angular/core';
import { MarkedOptions, Renderer, Tokens } from 'marked';

export const MARKED_OPTIONS = new InjectionToken<MarkedOptions>('MarkedOptions', {
  factory: () => {
    // marked.js options with bootstrap styles added.

    const renderer = new Renderer();

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
});
