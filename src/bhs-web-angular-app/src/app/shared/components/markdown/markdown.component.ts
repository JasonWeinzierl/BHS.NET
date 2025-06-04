import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';
import { marked } from 'marked';

@Component({
  selector: 'app-markdown',
  template: '',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    'class': 'prose max-w-none',
    '[innerHTML]': 'parsed()',
  },
  imports: [],
})
export class MarkdownComponent {
  readonly data = input<string | null>();

  readonly parsed = computed(() => {
    const data = this.data();
    if (data == null) {
      return '';
    }
    return marked.parse(data, {
      async: false,
    });
  });
}
