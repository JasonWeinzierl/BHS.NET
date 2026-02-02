import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { catchError, map, of, startWith, switchMap } from 'rxjs';
import { PostCardComponent } from '../../components/post-card/post-card.component';
import { BlogService } from '@data/blog';
import parseErrorMessage from '@shared/parseErrorMessage';

@Component({
  selector: 'app-category-posts',
  templateUrl: './category-posts.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
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
    switchMap(slug => this.blogService.getCategory$(slug)),
    map(category => ({
      category: {
        ...category,
        posts: category.posts.toSorted((a, b) => b.datePublished.getTime() - a.datePublished.getTime()),
      },
      isLoading: false,
      error: null,
    })),
    startWith({ category: null, isLoading: true, error: null }),
    catchError((err: unknown) => {
      const msg = parseErrorMessage(err) ?? 'An unknown error occurred.';
      return of({ category: null, isLoading: false, error: msg });
    }),
  );
}
