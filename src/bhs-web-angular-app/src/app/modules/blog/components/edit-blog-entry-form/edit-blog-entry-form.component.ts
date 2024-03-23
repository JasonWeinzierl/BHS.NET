import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Author } from '@data/authors';
import { Category, categorySchema, Post, PostRequest } from '@data/blog';

@Component({
  selector: 'app-edit-blog-entry-form',
  templateUrl: './edit-blog-entry-form.component.html',
  styleUrl: './edit-blog-entry-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EditBlogEntryFormComponent implements OnChanges {
  @Input() initialPost?: Post;
  @Input() currentAuthor: Author | null = null;
  @Input() allCategories: Array<Category> = [];

  @Output() readonly publish = new EventEmitter<PostRequest>();

  cancelRoute: Array<string> = ['/apps/blog'];
  editFormGroup = this.formBuilder.nonNullable.group({
    title: ['', [Validators.required]],
    categories: [[] as Array<string>],
    publishDate: [new Date()],
    contentMarkdown: [''],
  });
  authorWarning?: string | null;

  constructor(
    private readonly formBuilder: FormBuilder,
  ) { }

  ngOnChanges(): void {
    this.cancelRoute = this.initialPost ? ['/apps/blog/entry', this.initialPost.slug] : ['/apps/blog'];

    this.editFormGroup.reset({
      title: this.initialPost?.title ?? '',
      categories: this.initialPost?.categories.map(x => x.slug) ?? [],
      publishDate: this.initialPost?.datePublished ?? new Date(),
      contentMarkdown: this.initialPost?.contentMarkdown ?? '',
    });

    this.authorWarning = this.isChangingAuthor()
      ? `Author is changing from '${this.initialPost?.author?.username ?? '(null)'}' to '${this.currentAuthor?.username ?? '(null)'}'.`
      : null;
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
      author: this.isChangingAuthor() ? this.currentAuthor : this.initialPost?.author ?? null, // TODO: support multiple authors.
      datePublished: raw.publishDate,
      categories: this.allCategories.filter(c => raw.categories.includes(c.slug)).map(c => categorySchema.parse(c)),
    };

    this.publish.emit(request);
  }
}
