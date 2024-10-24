import { AsyncPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertComponent } from 'ngx-bootstrap/alert';
import { catchError, map, of, startWith, switchMap } from 'rxjs';
import { PostCardComponent } from '../../components/post-card/post-card.component';
import { BlogService } from '@data/blog';

@Component({
  selector: 'app-category-posts',
  templateUrl: './category-posts.component.html',
  styleUrl: './category-posts.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    AlertComponent,
    PostCardComponent,
    AsyncPipe,
  ],
})
export class CategoryPostsComponent {
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly blogService = inject(BlogService);

  vm$ = this.activatedRoute.paramMap.pipe(
    map(params => {
      const slug = params.get('slug');
      if (!slug) {
        throw new Error('Failed to get category slug from URL.');
      }
      return slug;
    }),
    switchMap(slug => this.blogService.getCategory(slug)),
    map(category => ({ category, isLoading: false, error: null })),
    startWith({ category: null, isLoading: true, error: null }),
    catchError((err: unknown) => {
      let msg = 'An error occurred.';
      if (err instanceof HttpErrorResponse) {
        msg = err.message;
      } else {
        console.error(err);
      }
      return of({ category: null, isLoading: false, error: msg });
    }),
  );
}
