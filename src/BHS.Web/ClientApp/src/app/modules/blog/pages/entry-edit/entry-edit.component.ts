import { AuthService, User } from '@auth0/auth0-angular';
import { BlogService, Category, Post, PostRequest } from '@data/blog';
import { catchError, exhaustMap, map, merge, Observable, of, startWith, Subject, switchMap } from 'rxjs';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Author } from '@data/authors';

interface EntryEditVm {
  post?: Post;
  categories: Array<Category>;
  isLoading: boolean;
  error?: string;
  currentAuthor: Author | null;
}

@Component({
  selector: 'app-entry-edit',
  templateUrl: './entry-edit.component.html',
  styleUrls: ['./entry-edit.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EntryEditComponent {
  vm$: Observable<EntryEditVm>;
  private readonly submittedRequestSubject = new Subject<{slug: string, body: PostRequest}>();

  constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly blogService: BlogService,
    private readonly auth: AuthService,
  ) {
    // Emit a merged combination of the initial post loaded from the URL, and the updated post after a submission.
    this.vm$ = merge(this.getInitialPost$(), this.getUpdatedPost$()).pipe(
      // Start with loading indicator.
      startWith({ categories: [], isLoading: true, currentAuthor: null }),
      // If an error occurs, populate the error property of the view model.
      catchError((err: unknown) => {
        console.error(err);
        let msg = 'An error occurred.';
        // TODO: this belongs in SharedModule
        if (typeof err === 'object' && err && 'message' in err && typeof err.message === 'string') {
          msg = err.message;
        }
        return of({ categories: [], isLoading: false, error: msg, currentAuthor: null });
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
      // Fetch current author.
      switchMap(({ post, allCategories }) => this.auth.user$.pipe(
        // Combine.
        map(user => ({ post, allCategories, user })),
      )),
      // Parse the results into a successful view model.
      map(({ post, allCategories, user }) => {

        post.dateLastModified = new Date(post.dateLastModified);
        post.datePublished = new Date(post.datePublished);

        const currentAuthor = this.getAuthor(user);

        return { post, categories: allCategories, isLoading: false, currentAuthor };
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
      // Re-fetch user too.
      switchMap(({ updatedPost, allCategories }) => this.auth.user$.pipe(
        map(user => ({ updatedPost, allCategories, user })),
      )),
      // Create the new view model.
      map(({ updatedPost, allCategories, user }) => {

        // TODO: duplicate logic from above initial post logic.
        updatedPost.dateLastModified = new Date(updatedPost.dateLastModified);
        updatedPost.datePublished = new Date(updatedPost.datePublished);

        const currentAuthor = this.getAuthor(user);

        return { isLoading: false, post: updatedPost, categories: allCategories, currentAuthor };
      }),
    );
  }

  private getAuthor(user?: User | null): Author | null {
    return user?.sub && user.name ? {
      username: user.sub,
      name: user.name,
    } : null;
  }

  onPublish(slug: string, request: PostRequest): void {
    this.submittedRequestSubject.next({ slug, body: request });
  }

}
