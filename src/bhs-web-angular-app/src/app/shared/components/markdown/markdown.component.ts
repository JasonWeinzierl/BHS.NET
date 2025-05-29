import { ChangeDetectionStrategy, Component, effect, ElementRef, inject, input } from '@angular/core';
import { marked } from 'marked';
import { MARKED_OPTIONS } from '@core/providers/bootstrap-marked-options.provider';

@Component({
  selector: 'app-markdown',
  templateUrl: './markdown.component.html',
  styleUrl: './markdown.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [],
})
export class MarkdownComponent {
  readonly data = input<string | null>();

  private readonly element = inject(ElementRef) as ElementRef<HTMLElement>;
  private readonly markedOptions = inject(MARKED_OPTIONS);

  constructor() {
    effect(() => {
      const data = this.data();
      if (data == null) {
        return;
      }

      const parsed = marked.parse(data, {
        ...this.markedOptions,
        async: false,
      });

      this.element.nativeElement.innerHTML = parsed;
    });
  }
}
