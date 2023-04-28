import { BlogService, Category, Post, PostRequest } from '@data/blog';
import { catchError, exhaustMap, map, merge, Observable, of, startWith, Subject, switchMap } from 'rxjs';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

type EntryEditVm = { post?: Post, categories: Array<Category>, isLoading: boolean, error?: string };

@Component({
  selector: 'app-entry-edit',
  templateUrl: './entry-edit.component.html',
  styleUrls: ['./entry-edit.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EntryEditComponent {
  vm$: Observable<EntryEditVm>;
  private submittedRequestSubject = new Subject<{slug: string, body: PostRequest}>();

  constructor(
    private activatedRoute: ActivatedRoute,
    private blogService: BlogService,
  ) {
    // Emit a merged combination of the initial post loaded from the URL, and the updated post after a submission.
    this.vm$ = merge(this.getInitialPost$(), this.getUpdatedPost$()).pipe(
      // Start with loading indicator.
      startWith({ categories: [], isLoading: true }),
      // If an error occurs, populate the error property of the view model.
      catchError((err: unknown) => {
        console.error(err);
        let msg = 'An error occurred.';
        // TODO: this belongs in SharedModule
        if (typeof err === 'object' && err && 'message' in err && typeof err.message === 'string') {
          msg = err.message;
        }
        return of({ categories: [], isLoading: false, error: msg });
      }),
    );
  }

  private getInitialPost$(): Observable<EntryEditVm> {
    return this.activatedRoute.paramMap.pipe(
      // Get the requested slug from the route.
      map(params => {
        const slug = params.get('slug');
        if (!slug) {
          throw new Error('Failed to get entry slug from URL.');
        }
        return slug;
      }),
      // Fetch the blog post.
      switchMap(slug => this.blogService.getPost(slug)),
      // Fetch all possible categories.
      switchMap(post => this.blogService.getCategories().pipe(
        // Combine the results of both fetches.
        map(allCategories => ({ post, allCategories })),
      )),
      // Parse the results into a successful view model.
      map(({ post, allCategories }) => {

        post.dateLastModified = new Date(post.dateLastModified);
        post.datePublished = new Date(post.datePublished);

        return { post, categories: allCategories, isLoading: false };
      }),
      // Handle when the user submits an update.
      switchMap(vm => this.submittedRequestSubject.pipe(
        // If any request is submitted, display loading.
        map(() => true),
        // Before any request is submitted, don't show loading.
        startWith(false),
        // Emit the updated view model.
        map(isSubmitting => ({ ...vm, isLoading: isSubmitting })),
      )),
    );
  }

  private getUpdatedPost$(): Observable<EntryEditVm> {
    // Listen to the stream of submitted requests.
    return this.submittedRequestSubject.pipe(
      // Submit the first update and wait. All other requests are discarded.
      exhaustMap(request => this.blogService.updatePost(request.slug, request.body)),
      // When update is complete, re-fetch all possible categories.
      switchMap(updatedPost => this.blogService.getCategories().pipe(
        map(allCategories => ({ updatedPost, allCategories })),
      )),
      // Create the new view model.
      map(({ updatedPost, allCategories }) => ({ isLoading: false, post: updatedPost, categories: allCategories })),
    );
  }

  onPublish(slug: string, request: PostRequest): void {
    this.submittedRequestSubject.next({ slug, body: request });
  }

}
