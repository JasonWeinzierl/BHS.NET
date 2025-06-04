import { DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, effect, ElementRef, inject, input, output, viewChildren } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormsModule, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Author } from '@data/authors';
import { Category, categorySchema, Post, PostRequest } from '@data/blog';
import { MarkdownComponent } from '@shared/components/markdown/markdown.component';

@Component({
  selector: 'app-edit-blog-entry-form',
  templateUrl: './edit-blog-entry-form.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RouterLink,
    MarkdownComponent,
  ],
  providers: [
    DatePipe,
  ],
})
export class EditBlogEntryFormComponent {
  private readonly fb = inject(NonNullableFormBuilder);
  private readonly datePipe = inject(DatePipe);

  readonly scrollableElements = viewChildren<string, ElementRef<HTMLElement>>('scrollable', { read: ElementRef });

  readonly initialPost = input<Post>();
  readonly currentAuthor = input<Author | null>();
  readonly allCategories = input<Array<Category>>([]);
  readonly publish = output<PostRequest>();

  readonly cancelRoute = computed(() => {
    const initialPost = this.initialPost();
    return initialPost ? ['/apps/blog/entry', initialPost.slug] : ['/apps/blog'];
  });

  readonly editFormGroup = this.fb.group({
    title: ['', [Validators.required]],
    categories: [[] as Array<string>],
    publishDate: [this.datePipe.transform(new Date(), 'yyyy-MM-ddTHH:mm:ss') ?? '', [Validators.required]],
    contentMarkdown: [''],
  });

  private readonly isChangingAuthor = computed(() => {
    return !this.initialPost()?.author && !!this.currentAuthor();
  });

  readonly authorWarning = computed(() => {
    const isChangingAuthor = this.isChangingAuthor();
    const currentAuthor = this.currentAuthor();
    return isChangingAuthor && currentAuthor
      ? `Author is changing from '${this.initialPost()?.author?.username ?? '(null)'}' to '${currentAuthor.username}'.`
      : null;
  });

  readonly isMacOS = /(macintosh|macinte|macppc|mac68k|macos)/.test(window.navigator.userAgent.toLowerCase());

  readonly contentSignal = toSignal(this.editFormGroup.controls.contentMarkdown.valueChanges);

  constructor() {
    effect(() => {
      const initialPost = this.initialPost();

      this.editFormGroup.reset({
        title: initialPost?.title ?? '',
        categories: initialPost?.categories.map(x => x.slug) ?? [],
        publishDate: this.datePipe.transform(initialPost?.datePublished ?? new Date(), 'yyyy-MM-ddTHH:mm:ss') ?? '',
        contentMarkdown: initialPost?.contentMarkdown ?? '',
      });
    });

    effect(onCleanup => {
      const scrollableElements = this.scrollableElements();

      let isScrolling = false;
      const scrollListener = (event: Event): void => {
        // Prevent infinite loop when scroll causes scroll.
        if (isScrolling) {
          return;
        }
        isScrolling = true;

        const scrolledElement = event.target as HTMLElement;

        for (const { nativeElement } of scrollableElements) {
          if (nativeElement === scrolledElement) {
            continue;
          }
          // Keep in sync via percentages, assuming heights are equal.
          const heightPercent = scrolledElement.scrollTop / (scrolledElement.scrollHeight - scrolledElement.clientHeight);
          const top = heightPercent * (nativeElement.scrollHeight - nativeElement.clientHeight);

          nativeElement.scrollTo({
            behavior: 'instant',
            top,
          });
        }

        // Re-enable listener right before browser paints.
        window.requestAnimationFrame(() => {
          isScrolling = false;
        });
      };

      for (const ele of scrollableElements) {
        ele.nativeElement.addEventListener('scroll', scrollListener);
      }

      onCleanup(() => {
        for (const ele of scrollableElements) {
          ele.nativeElement.removeEventListener('scroll', scrollListener);
        }
      });
    });
  }

  onSubmit(): void {
    const raw = this.editFormGroup.getRawValue();

    const request: PostRequest = {
      title: raw.title,
      contentMarkdown: raw.contentMarkdown,
      filePath: this.initialPost()?.filePath ?? null,
      photosAlbumSlug: this.initialPost()?.photosAlbumSlug ?? null,
      author: this.isChangingAuthor() ? this.currentAuthor() ?? null : this.initialPost()?.author ?? null, // TODO: support multiple authors.
      datePublished: new Date(raw.publishDate),
      categories: this.allCategories().filter(c => raw.categories.includes(c.slug)).map(c => categorySchema.parse(c)),
    };

    this.publish.emit(request);
  }
}
