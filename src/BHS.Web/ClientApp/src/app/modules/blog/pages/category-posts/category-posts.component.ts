import { BlogService, CategoryPosts } from '@data/blog';
import { catchError, map, Observable, of, startWith, switchMap } from 'rxjs';
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
  vm$: Observable<{ category?: CategoryPosts, isLoading: boolean, error?: string }>;

  constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly blogService: BlogService,
  ) {
    this.vm$ = this.activatedRoute.paramMap.pipe(
      map(params => {
        const slug = params.get('slug');
        if (!slug) {
          throw new Error('Failed to get category slug from URL.');
        }
        return slug;
      }),
      switchMap(slug => this.blogService.getCategory(slug)),
      map(category => ({ category, isLoading: false })),
      startWith({ isLoading: true }),
      catchError((err: unknown) => {
        let msg = 'An error occurred.';
        if (err instanceof HttpErrorResponse) {
          msg = err.message;
        } else {
          console.error(err);
        }
        return of({ isLoading: false, error: msg });
      }),
    );
  }
}
