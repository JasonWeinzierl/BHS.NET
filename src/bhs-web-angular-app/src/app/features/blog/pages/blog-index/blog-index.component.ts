import { AsyncPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { catchError, map, of, startWith } from 'rxjs';
import { CategoriesListViewComponent } from '../../components/categories-list-view/categories-list-view.component';
import { PostsSearchComponent } from '../../components/posts-search/posts-search.component';
import { BlogService } from '@data/blog';

@Component({
  selector: 'app-blog-index',
  templateUrl: './blog-index.component.html',
  styleUrl: './blog-index.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    PostsSearchComponent,
    CategoriesListViewComponent,
    AsyncPipe,
    RouterLink,
  ],
})
export class BlogIndexComponent {
  private readonly blogService = inject(BlogService);
  private readonly auth = inject(AuthService);

  categoriesVm$ = this.blogService.getCategories().pipe(
    map(categories => ({ categories, isLoading: false, error: null })),
    startWith({ categories: [], isLoading: true, error: null }),
    catchError((err: unknown) => {
      let msg = 'An error occurred.';
      if (err instanceof HttpErrorResponse) {
        msg = err.message;
      } else {
        console.error(err);
      }
      return of({ categories: [], isLoading: false, error: msg });
    }),
  );

  isAuthenticated$ = this.auth.isAuthenticated$;
}
