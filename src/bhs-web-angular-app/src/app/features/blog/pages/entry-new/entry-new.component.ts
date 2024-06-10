import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { AlertModule } from 'ngx-bootstrap/alert';
import { catchError, combineLatest, exhaustMap, map, merge, Observable, of, startWith, Subject, switchMap } from 'rxjs';
import { EditBlogEntryFormComponent } from '../../components/edit-blog-entry-form/edit-blog-entry-form.component';
import { Author, AuthorService } from '@data/authors';
import { BlogService, Category, Post, PostRequest } from '@data/blog';

interface EntryNewVm {
  currentAuthor?: Author | null;
  allCategories: Array<Category>;
  isLoading: boolean;
  error?: string;
}

@Component({
  selector: 'app-entry-new',
  templateUrl: './entry-new.component.html',
  styleUrl: './entry-new.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    AlertModule,
    EditBlogEntryFormComponent,
    AsyncPipe,
  ],
})
export class EntryNewComponent {
  private readonly blogService = inject(BlogService);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly authorService = inject(AuthorService);

  private readonly submittedRequestSubject = new Subject<PostRequest>();
  private readonly routeErrorSubject = new Subject<{ newPost?: Post, error: unknown }>();

  vm$ = merge(this.getInitialVm$(), this.getCreatedPost$(), this.getRouteError$()).pipe(
    startWith({ allCategories: [], isLoading: true } as EntryNewVm),
    catchError((err: unknown) => {
      let msg = 'An error occurred.';
      if (typeof err === 'object' && err && 'message' in err && typeof err.message === 'string') {
        msg = err.message;
      }
      return of({ allCategories: [], isLoading: false, error: msg } as EntryNewVm);
    }),
  );

  onPublish(request: PostRequest): void {
    this.submittedRequestSubject.next(request);
  }

  private getInitialVm$(): Observable<EntryNewVm> {
    // TODO: entry-edit can be simplified to use combineLatest too?
    return combineLatest([this.blogService.getCategories(), this.auth.user$]).pipe(
      switchMap(([ allCategories, user ]) => (user?.sub ? this.authorService.getAuthors(user.sub) : of([])).pipe(
        map(authors => ({ allCategories, authors })),
      )),
      map(({ allCategories, authors }) => {
        const currentAuthor = authors.length ? authors[0] : null; // TODO: support user picking from multiple authors.

        return { currentAuthor, allCategories, isLoading: false };
      }),
      switchMap(vm => this.submittedRequestSubject.pipe(
        map(() => true),
        startWith(false),
        map(isSubmitting => ({ ...vm, isLoading: isSubmitting })),
      )),
    );
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
