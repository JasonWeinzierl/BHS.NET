import { Category, Post, PostRequest } from '@data/blog';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Author } from '@data/authors';

type EditFormGroup = FormGroup<{
  title: FormControl<string>;
  categories: FormControl<Array<string>>;
  publishDate: FormControl<Date>;
  contentMarkdown: FormControl<string>;
}>;

@Component({
  selector: 'app-edit-blog-entry-form',
  templateUrl: './edit-blog-entry-form.component.html',
  styleUrls: ['./edit-blog-entry-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EditBlogEntryFormComponent implements OnChanges {
  @Input() initialPost?: Post;
  @Input() currentAuthor: Author | null = null;
  @Input() allCategories: Array<Category> = [];

  @Output() publish = new EventEmitter<PostRequest>();

  cancelRoute: Array<string> = [];
  editFormGroup?: EditFormGroup;
  authorWarning?: string | null;

  constructor(
    private formBuilder: FormBuilder,
  ) { }

  ngOnChanges(): void {
    this.cancelRoute = this.initialPost ? ['/apps/blog/entry', this.initialPost.slug] : ['/apps/blog'];

    this.editFormGroup = this.formBuilder.nonNullable.group({
      title: [this.initialPost?.title ?? '', [Validators.required]],
      categories: [this.initialPost?.categories.map(x => x.slug) ?? []],
      publishDate: [this.initialPost?.datePublished ?? new Date()],
      contentMarkdown: [this.initialPost?.contentMarkdown ?? ''],
    });

    this.authorWarning = this.isChangingAuthor()
      ? `Author is changing from '${this.initialPost?.author?.username ?? '(null)'}' to '${this.currentAuthor?.username ?? '(null)'}'.`
      : null;
  }

  private isChangingAuthor(): boolean {
    return !this.initialPost?.author && !!this.currentAuthor;
  }

  onSubmit(): void {
    if (!this.editFormGroup) {
      return;
    }

    const raw = this.editFormGroup.getRawValue();

    const request: PostRequest = {
      title: raw.title,
      contentMarkdown: raw.contentMarkdown,
      filePath: this.initialPost?.filePath ?? null,
      photosAlbumSlug: this.initialPost?.photosAlbumSlug ?? null,
      author: this.isChangingAuthor() ? this.currentAuthor : this.initialPost?.author ?? null, // TODO: support multiple authors.
      datePublished: raw.publishDate,
      categories: this.allCategories.filter(c => raw.categories.includes(c.slug)),
    };

    this.publish.emit(request);
  }
}
