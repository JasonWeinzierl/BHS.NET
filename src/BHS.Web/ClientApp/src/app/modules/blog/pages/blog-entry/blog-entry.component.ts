import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { ToastrService } from 'ngx-toastr';
import { catchError, map, Observable, of, startWith, switchMap } from 'rxjs';
import { BlogService, Post } from '@data/blog';
import { AlbumPhotos, PhotosService } from '@data/photos';

@Component({
  selector: 'app-blog-entry',
  templateUrl: './blog-entry.component.html',
  styleUrls: ['./blog-entry.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BlogEntryComponent {
  vm$: Observable<{ post?: Post, postAlbum: AlbumPhotos | null, isLoading: boolean, error?: string, showEdit?: boolean }>;

  constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly blogService: BlogService,
    private readonly photosService: PhotosService,
    private readonly auth: AuthService,
    private readonly toastr: ToastrService,
  ) {
    this.vm$ = this.activatedRoute.paramMap.pipe(
      map(params => {
        const slug = params.get('slug');
        if (!slug) {
          throw new Error('Failed to get entry slug from URL.');
        }
        return slug;
      }),
      switchMap(slug => this.blogService.getPost(slug)),
      switchMap(post => {
        if (post.photosAlbumSlug) {
          return this.photosService.getAlbum(post.photosAlbumSlug).pipe(
            // Alert but don't prevent the entire post from loading.
            catchError((err: unknown) => {
              const msg = err instanceof HttpErrorResponse
                ? err.message
                : 'An error occurred';
              this.toastr.error(msg, 'Failed to load photos.');
              console.warn(msg, err);
              return of(null);
            }),
            map(album => ({ post, postAlbum: album, isLoading: false })),
          );
        } else {
          return of({ post, postAlbum: null, isLoading: false });
        }
      }),
      switchMap(vm => this.auth.isAuthenticated$.pipe(
        startWith(false),
        map(isAuthenticated => ({ ...vm, showEdit: isAuthenticated })),
      )),
      startWith({ postAlbum: null, isLoading: true }),
      catchError((err: unknown) => {
        let msg = 'An error occurred.';
        if (err instanceof HttpErrorResponse) {
          msg = err.message;
        } else {
          console.error(msg, err);
        }
        return of({ postAlbum: null, isLoading: false, error: msg });
      }),
    );
  }
}
