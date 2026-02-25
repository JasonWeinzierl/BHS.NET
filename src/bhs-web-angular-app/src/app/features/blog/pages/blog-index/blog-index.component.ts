import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { catchError, map, of, startWith } from 'rxjs';
import { BlogService } from '@data/blog';
import parseErrorMessage from '@shared/parse-error-message';
import { CategoriesListViewComponent } from '../../components/categories-list-view/categories-list-view.component';
import { PostsSearchComponent } from '../../components/posts-search/posts-search.component';

@Component({
  selector: 'app-blog-index',
  templateUrl: './blog-index.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    PostsSearchComponent,
    CategoriesListViewComponent,
    RouterLink,
  ],
  host: { 'data-testid': 'BlogIndex' },
})
export class BlogIndexComponent {
  private readonly blogService = inject(BlogService);
  private readonly auth = inject(AuthService);

  readonly categoriesVmSignal = toSignal(this.blogService.getCategories$().pipe(
    map(categories => ({ categories, isLoading: false, error: undefined })),
    startWith({ categories: [], isLoading: true, error: undefined }),
    catchError((error: unknown) => {
      const message = parseErrorMessage(error) ?? 'An unknown error occurred.';
      return of({ categories: [], isLoading: false, error: message });
    }),
  ));

  readonly isAuthenticatedSignal = toSignal(this.auth.isAuthenticated$);
}
