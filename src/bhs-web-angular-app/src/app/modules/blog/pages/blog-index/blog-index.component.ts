import { AsyncPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, map, startWith } from 'rxjs/operators';
import { CategoriesListViewComponent } from '../../components/categories-list-view/categories-list-view.component';
import { PostsSearchComponent } from '../../components/posts-search/posts-search.component';
import { BlogService, CategorySummary } from '@data/blog';

@Component({
  selector: 'app-blog-index',
  templateUrl: './blog-index.component.html',
  styleUrl: './blog-index.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    PostsSearchComponent,
    CategoriesListViewComponent,
    AsyncPipe,
  ],
})
export class BlogIndexComponent {
  categoriesVm$: Observable<{ categories: Array<CategorySummary>, isLoading: boolean, error?: string }>;

  constructor(
    private readonly blogService: BlogService,
  ) {
    this.categoriesVm$ = this.blogService.getCategories().pipe(
      map(categories => ({ categories, isLoading: false })),
      startWith({ categories: [], isLoading: true }),
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
  }
}
