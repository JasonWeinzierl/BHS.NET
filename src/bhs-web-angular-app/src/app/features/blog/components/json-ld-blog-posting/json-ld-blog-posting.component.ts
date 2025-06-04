/* eslint-disable @typescript-eslint/naming-convention -- JSON-LD format */
import { ChangeDetectionStrategy, Component, computed, inject, input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Post } from '@data/blog';

@Component({
  selector: 'app-json-ld-blog-posting',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: '',
  host: {
    '[innerHtml]': 'trustedJsonLdHtml()',
  },
  imports: [],
})
export class JsonLdBlogPostingComponent {
  private readonly sanitizer = inject(DomSanitizer);

  readonly post = input.required<Post>();

  readonly trustedJsonLdHtml = computed(() => {
    const post = this.post();

    // https://schema.org/BlogPosting
    const json = JSON.stringify({
      '@context': 'https://schema.org',
      '@type': 'BlogPosting',
      'headline': post.title,
      'name': post.title,
      'author': post.author ? {
        '@type': 'Person',
        'name': post.author.name,
      } : undefined,
      'datePublished': post.datePublished.toISOString(),
      'dateModified': post.dateLastModified.toISOString(),
      'keywords': post.categories.map(category => category.name),
    }, null, 2);
    const html = `<script type="application/ld+json">${json}</script>`;

    return this.sanitizer.bypassSecurityTrustHtml(html);
  });
}
