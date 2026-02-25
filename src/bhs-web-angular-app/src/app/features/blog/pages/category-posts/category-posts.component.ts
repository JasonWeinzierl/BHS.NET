import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { catchError, map, of, startWith, switchMap } from 'rxjs';
import { BlogService } from '@data/blog';
import parseErrorMessage from '@shared/parse-error-message';
import { PostCardComponent } from '../../components/post-card/post-card.component';

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
    map(parameters => {
      const slug = parameters.get('slug');
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
      error: undefined,
    })),
    startWith({ category: undefined, isLoading: true, error: undefined }),
    catchError((error: unknown) => {
      const message = parseErrorMessage(error) ?? 'An unknown error occurred.';
      return of({ category: undefined, isLoading: false, error: message });
    }),
  );
}
