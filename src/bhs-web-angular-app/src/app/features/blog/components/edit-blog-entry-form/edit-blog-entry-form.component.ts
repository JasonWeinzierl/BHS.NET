import { ChangeDetectionStrategy, Component, computed, effect, inject, input, output } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { Author } from '@data/authors';
import { Category, categorySchema, Post, PostRequest } from '@data/blog';

@Component({
  selector: 'app-edit-blog-entry-form',
  templateUrl: './edit-blog-entry-form.component.html',
  styleUrl: './edit-blog-entry-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
      FormsModule,
      ReactiveFormsModule,
      BsDatepickerModule,
      RouterLink,
  ],
})
export class EditBlogEntryFormComponent {
  private readonly fb = inject(FormBuilder);

  readonly initialPost = input<Post>();
  readonly currentAuthor = input<Author | null>();
  readonly allCategories = input<Array<Category>>([]);
  readonly publish = output<PostRequest>();

  readonly cancelRoute = computed(() => {
    const initialPost = this.initialPost();
    return initialPost ? ['/apps/blog/entry', initialPost.slug] : ['/apps/blog'];
  });

  readonly editFormGroup = this.fb.nonNullable.group({
    title: ['', [Validators.required]],
    categories: [[] as Array<string>],
    publishDate: [new Date()],
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

  constructor() {
    effect(() => {
      const initialPost = this.initialPost();

      this.editFormGroup.reset({
        title: initialPost?.title ?? '',
        categories: initialPost?.categories.map(x => x.slug) ?? [],
        publishDate: initialPost?.datePublished ?? new Date(),
        contentMarkdown: initialPost?.contentMarkdown ?? '',
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
      datePublished: raw.publishDate,
      categories: this.allCategories().filter(c => raw.categories.includes(c.slug)).map(c => categorySchema.parse(c)),
    };

    this.publish.emit(request);
  }
}
