import { Category, Post, PostRequest } from '@data/blog';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

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
  @Input() initialPost!: Post;
  @Input() allCategories!: Array<Category>;

  @Output() publish = new EventEmitter<PostRequest>();

  editFormGroup?: EditFormGroup;

  constructor(
    private formBuilder: FormBuilder,
  ) { }

  ngOnChanges(): void {
    this.editFormGroup = this.formBuilder.nonNullable.group({
      title: [this.initialPost.title, [Validators.required]],
      categories: [this.initialPost.categories.map(x => x.slug)],
      publishDate: [this.initialPost.datePublished],
      contentMarkdown: [this.initialPost.contentMarkdown],
    });
  }

  onSubmit(): void {
    if (!this.editFormGroup) {
      return;
    }

    const raw = this.editFormGroup.getRawValue();

    const request: PostRequest = {
      title: raw.title,
      contentMarkdown: raw.contentMarkdown,
      filePath: this.initialPost.filePath,
      photosAlbumSlug: this.initialPost.photosAlbumSlug,
      author: this.initialPost.author,
      datePublished: raw.publishDate,
      categories: this.allCategories.filter(c => raw.categories.includes(c.slug)),
    };

    this.publish.emit(request);
  }
}
