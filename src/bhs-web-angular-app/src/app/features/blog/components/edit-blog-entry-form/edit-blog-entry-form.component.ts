import { ChangeDetectionStrategy, Component, EventEmitter, inject, Input, OnChanges, Output, signal } from '@angular/core';
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
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    BsDatepickerModule,
    RouterLink,
  ],
})
export class EditBlogEntryFormComponent implements OnChanges {
  private readonly fb = inject(FormBuilder);

  @Input() initialPost?: Post;
  @Input() currentAuthor?: Author | null;
  @Input() allCategories: Array<Category> = [];

  @Output() readonly publish = new EventEmitter<PostRequest>();

  cancelRoute = signal<Array<string>>(['/apps/blog']);
  editFormGroup = this.fb.nonNullable.group({
    title: ['', [Validators.required]],
    categories: [[] as Array<string>],
    publishDate: [new Date()],
    contentMarkdown: [''],
  });
  authorWarning = signal<string | null>(null);

  ngOnChanges(): void {
    this.cancelRoute.set(this.initialPost ? ['/apps/blog/entry', this.initialPost.slug] : ['/apps/blog']);

    this.editFormGroup.reset({
      title: this.initialPost?.title ?? '',
      categories: this.initialPost?.categories.map(x => x.slug) ?? [],
      publishDate: this.initialPost?.datePublished ?? new Date(),
      contentMarkdown: this.initialPost?.contentMarkdown ?? '',
    });

    this.authorWarning.set(this.isChangingAuthor()
      ? `Author is changing from '${this.initialPost?.author?.username ?? '(null)'}' to '${this.currentAuthor?.username ?? '(null)'}'.`
      : null);
  }

  private isChangingAuthor(): boolean {
    return !this.initialPost?.author && !!this.currentAuthor;
  }

  onSubmit(): void {
    const raw = this.editFormGroup.getRawValue();

    const request: PostRequest = {
      title: raw.title,
      contentMarkdown: raw.contentMarkdown,
      filePath: this.initialPost?.filePath ?? null,
      photosAlbumSlug: this.initialPost?.photosAlbumSlug ?? null,
      author: this.isChangingAuthor() ? this.currentAuthor ?? null : this.initialPost?.author ?? null, // TODO: support multiple authors.
      datePublished: raw.publishDate,
      categories: this.allCategories.filter(c => raw.categories.includes(c.slug)).map(c => categorySchema.parse(c)),
    };

    this.publish.emit(request);
  }
}
