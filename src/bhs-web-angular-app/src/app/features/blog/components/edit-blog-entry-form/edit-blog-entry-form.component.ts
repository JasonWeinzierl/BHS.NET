import { ChangeDetectionStrategy, Component, EventEmitter, inject, input, OnChanges, Output, signal } from '@angular/core';
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
export class EditBlogEntryFormComponent implements OnChanges {
  private readonly fb = inject(FormBuilder);

  readonly initialPost = input<Post>();
  readonly currentAuthor = input<Author | null>();
  readonly allCategories = input<Array<Category>>([]);

  @Output() readonly publish = new EventEmitter<PostRequest>();

  readonly cancelRoute = signal<Array<string>>(['/apps/blog']);
  editFormGroup = this.fb.nonNullable.group({
    title: ['', [Validators.required]],
    categories: [[] as Array<string>],
    publishDate: [new Date()],
    contentMarkdown: [''],
  });

  readonly authorWarning = signal<string | null>(null);

  ngOnChanges(): void {
    const initialPost = this.initialPost();
    this.cancelRoute.set(initialPost ? ['/apps/blog/entry', initialPost.slug] : ['/apps/blog']);

    this.editFormGroup.reset({
      title: initialPost?.title ?? '',
      categories: initialPost?.categories.map(x => x.slug) ?? [],
      publishDate: initialPost?.datePublished ?? new Date(),
      contentMarkdown: initialPost?.contentMarkdown ?? '',
    });

    this.authorWarning.set(this.isChangingAuthor()
      ? `Author is changing from '${initialPost?.author?.username ?? '(null)'}' to '${this.currentAuthor()?.username ?? '(null)'}'.`
      : null);
  }

  private isChangingAuthor(): boolean {
    return !this.initialPost()?.author && !!this.currentAuthor();
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
