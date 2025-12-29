import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { catchError, map, of, startWith } from 'rxjs';
import { CategoriesListViewComponent } from '../../components/categories-list-view/categories-list-view.component';
import { PostsSearchComponent } from '../../components/posts-search/posts-search.component';
import { BlogService } from '@data/blog';
import parseErrorMessage from '@shared/parseErrorMessage';

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

  readonly categoriesVmSignal = toSignal(this.blogService.getCategories().pipe(
    map(categories => ({ categories, isLoading: false, error: null })),
    startWith({ categories: [], isLoading: true, error: null }),
    catchError((err: unknown) => {
      const msg = parseErrorMessage(err) ?? 'An unknown error occurred.';
      return of({ categories: [], isLoading: false, error: msg });
    }),
  ));

  readonly isAuthenticatedSignal = toSignal(this.auth.isAuthenticated$);
}
