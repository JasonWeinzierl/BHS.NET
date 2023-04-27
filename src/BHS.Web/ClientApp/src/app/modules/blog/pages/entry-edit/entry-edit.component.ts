import { BehaviorSubject, catchError, map, Observable, of, startWith, switchMap } from 'rxjs';
import { BlogService, Category, Post, PostRequest } from '@data/blog';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

type EntryEditVm = { initialPost?: Post, categories: Array<Category>, isLoading: boolean, error?: string };

@Component({
  selector: 'app-entry-edit',
  templateUrl: './entry-edit.component.html',
  styleUrls: ['./entry-edit.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EntryEditComponent {
  vm$: Observable<EntryEditVm>;
  private isSubmittingSubject = new BehaviorSubject(false);

  constructor(
    private activatedRoute: ActivatedRoute,
    private blogService: BlogService,
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

        return { initialPost: post, categories: allCategories, isLoading: false };
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

  onPublish(request: PostRequest): void {
    console.log(JSON.stringify(request));
    this.isSubmittingSubject.next(true);
    // TODO: build request using initialPost and updated values, send request to backend.
    return;
  }

}
