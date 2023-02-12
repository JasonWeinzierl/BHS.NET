import { AlbumPhotos, PhotosService } from '@data/photos';
import { BlogService, Post } from '@data/blog';
import { catchError, filter, map, Observable, of, switchMap, tap } from 'rxjs';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-blog-entry',
  templateUrl: './blog-entry.component.html',
  styleUrls: ['./blog-entry.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BlogEntryComponent {
  post$: Observable<Post>;
  postAlbum$: Observable<AlbumPhotos> = of();
  error?: string;
  isLoading = true;

  constructor(
    private activatedRoute: ActivatedRoute,
    private blogService: BlogService,
    private photosService: PhotosService,
  ) {
    this.post$ = this.activatedRoute.paramMap.pipe(
      map(params => {
        const slug = params.get('slug');
        if (!slug) {
          this.error = 'Failed to get entry slug from URL.';
          return null;
        }
        return slug;
      }),
      filter(slug => slug !== null),
      switchMap(slug => this.blogService.getPost(slug!)),
      tap(post => {
        if (post.photosAlbumSlug) {
          this.postAlbum$ = this.photosService.getAlbum(post.photosAlbumSlug);
        }
        this.isLoading = false;
      }),
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
