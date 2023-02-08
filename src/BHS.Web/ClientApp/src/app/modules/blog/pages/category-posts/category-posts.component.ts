import { BlogService, CategoryPosts } from '@data/blog';
import { catchError, filter, finalize, map, Observable, of, switchMap } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { Component } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-category-posts',
  templateUrl: './category-posts.component.html',
  styleUrls: ['./category-posts.component.scss']
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
          return null;
        }
        return slug;
      }),
      filter(slug => slug !== null),
      switchMap(slug => this.blogService.getCategory(slug!)),
      catchError((err: unknown) => {
        if (err instanceof HttpErrorResponse) {
          this.error = err.message;
        } else {
          this.error = 'An error occurred.';
        }
        return of();
      }),
      finalize(() => this.isLoading = false),
    );
  }
}
