import { ActivatedRoute, Router } from '@angular/router';
import { AuthService, User } from '@auth0/auth0-angular';
import { BlogService, Category, Post, PostRequest } from '@data/blog';
import { catchError, combineLatest, exhaustMap, map, merge, Observable, of, startWith, Subject, switchMap } from 'rxjs';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Author } from '@data/authors';

interface EntryNewVm {
  currentAuthor: Author | null;
  allCategories: Array<Category>;
  isLoading: boolean;
  error?: string;
}

@Component({
  selector: 'app-entry-new',
  templateUrl: './entry-new.component.html',
  styleUrls: ['./entry-new.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EntryNewComponent {
  vm$: Observable<EntryNewVm>;
  private readonly submittedRequestSubject = new Subject<PostRequest>();
  private readonly routeErrorSubject = new Subject<{ newPost?: Post, error: unknown }>();

  constructor(
    private readonly blogService: BlogService,
    private readonly auth: AuthService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
  ) {
    this.vm$ = merge(this.getInitialVm$(), this.getCreatedPost$(), this.getRouteError$()).pipe(
      startWith({ allCategories: [], isLoading: true, currentAuthor: null }),
      catchError((err: unknown) => {
        let msg = 'An error occurred.';
        if (typeof err === 'object' && err && 'message' in err && typeof err.message === 'string') {
          msg = err.message;
        }
        return of({ currentAuthor: null, allCategories: [], isLoading: false, error: msg });
      }),
    );
  }

  onPublish(request: PostRequest): void {
    this.submittedRequestSubject.next(request);
  }

  private getInitialVm$(): Observable<EntryNewVm> {
    // TODO: entry-edit can be simplified to use combineLatest too?
    return combineLatest([this.blogService.getCategories(), this.auth.user$]).pipe(
      map(([ allCategories, user ]) => {
        const currentAuthor = this.getAuthor(user);

        return { currentAuthor, allCategories, isLoading: false };
      }),
      switchMap(vm => this.submittedRequestSubject.pipe(
        map(() => true),
        startWith(false),
        map(isSubmitting => ({ ...vm, isLoading: isSubmitting })),
      )),
    );
  }

  // TODO: reuse, entry-edit dupe
  private getAuthor(user?: User | null): Author | null {
    return user?.sub && user.name ? {
      username: user.sub,
      name: user.name,
    } : null;
  }

  private getCreatedPost$(): Observable<EntryNewVm> {
    return this.submittedRequestSubject.pipe(
      exhaustMap(request => this.blogService.createPost(request)),
      map(newPost => {
        this.router.navigate(['../entry', newPost.slug], { relativeTo: this.route })
          .catch((error: unknown) => { this.routeErrorSubject.next({ error, newPost }); });

        // Instead of mapping the post into the VM, just keep loading until the route changes.
        return { isLoading: true, allCategories: [], currentAuthor: null };
      }),
    );
  }

  private getRouteError$(): Observable<never> {
    return this.routeErrorSubject.pipe(
      map(({ error, newPost }) => {
        let msg = `An error occurred while navigating to new post '${newPost?.title ?? newPost?.slug ?? '(null)'}'.`;
        if (typeof error === 'object' && error && 'message' in error && typeof error.message === 'string') {
          msg = `${msg} ${error.message}`;
        }
        throw new Error(msg, { cause: error });
      }),
    );
  }
}
