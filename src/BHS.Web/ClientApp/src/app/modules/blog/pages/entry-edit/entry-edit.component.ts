import { BehaviorSubject, catchError, map, Observable, of, startWith, switchMap } from 'rxjs';
import { BlogService, Category, Post } from '@data/blog';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

type PublishFormGroup = FormGroup<{
  title: FormControl<string>;
  categories: FormControl<Array<string>>;
  publishDate: FormControl<Date>;
  contentMarkdown: FormControl<string>;
}>;

@Component({
  selector: 'app-entry-edit',
  templateUrl: './entry-edit.component.html',
  styleUrls: ['./entry-edit.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EntryEditComponent {
  // TODO: split this into separate component with form etc. so the view model isn't so crazy
  vm$: Observable<{ initialPost?: Post, publishForm?: PublishFormGroup, categories: Array<Category>, isLoading: boolean, error?: string }>;
  private isSubmittingSubject = new BehaviorSubject(false);

  constructor(
    private activatedRoute: ActivatedRoute,
    private blogService: BlogService,
    private formBuilder: FormBuilder,
  ) {
    this.vm$ = this.activatedRoute.paramMap.pipe(
      map(params => {
        const slug = params.get('slug');
        if (!slug) {
          throw new Error('Failed to get entry slug from URL.');
        }
        return slug;
      }),
      switchMap(slug => this.blogService.getPost(slug)),
      switchMap(post => this.blogService.getCategories().pipe(
        map(allCategories => ({ post, allCategories })),
      )),
      map(({ post, allCategories }) => {

        post.dateLastModified = new Date(post.dateLastModified);
        post.datePublished = new Date(post.datePublished);

        const group = this.formBuilder.nonNullable.group({
          title: [post.title, [Validators.required]],
          categories: [post.categories.map(x => x.slug)],
          publishDate: [post.datePublished],
          contentMarkdown: [post.contentMarkdown],
        });

        return { initialPost: post, publishForm: group, categories: allCategories, isLoading: false };
      }),
      switchMap(vm => this.isSubmittingSubject.pipe(
        map(isSubmitting => ({ ...vm, isLoading: vm.isLoading || isSubmitting })),
      )),
      startWith({ categories: [], isLoading: true }),
      catchError((err: unknown) => {
        let msg = 'An error occurred.';
        if (err instanceof HttpErrorResponse) {
          msg = err.message;
        } else {
          console.error(msg);
        }
        return of({ categories: [], isLoading: false, error: msg });
      }),
    );
  }

  onPublish(v: PublishFormGroup['value']): void {
    console.log(JSON.stringify(v));
    this.isSubmittingSubject.next(true);
    // TODO: build request using initialPost and updated values, send request to backend.
    return;
  }

}
