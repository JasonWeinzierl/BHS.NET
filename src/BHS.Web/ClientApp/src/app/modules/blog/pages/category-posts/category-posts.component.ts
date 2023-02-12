import { BlogService, CategoryPosts } from '@data/blog';
import { catchError, filter, map, Observable, of, switchMap, tap } from 'rxjs';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-category-posts',
  templateUrl: './category-posts.component.html',
  styleUrls: ['./category-posts.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CategoryPostsComponent {
  category$: Observable<CategoryPosts>;
  error?: string;
  isLoading = true;

  constructor(
    private activatedRoute: ActivatedRoute,
    private blogService: BlogService,
  ) {
    this.category$ = this.activatedRoute.paramMap.pipe(
      map(params => {
        const slug = params.get('slug');
        if (!slug) {
          this.error = 'Failed to get category slug from URL.';
        }
        return slug;
      }),
      filter(slug => !!slug),
      switchMap(slug => this.blogService.getCategory(slug ?? '')),
      tap(() => this.isLoading = false),
      catchError((err: unknown) => {
        if (err instanceof HttpErrorResponse) {
          this.error = err.message;
        } else {
          this.error = 'An error occurred.';
        }
        return of();
      }),
    );
  }
}
