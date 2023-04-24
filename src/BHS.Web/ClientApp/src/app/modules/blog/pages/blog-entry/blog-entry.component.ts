import { AlbumPhotos, PhotosService } from '@data/photos';
import { BlogService, Post } from '@data/blog';
import { catchError, map, Observable, of, startWith, switchMap } from 'rxjs';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-blog-entry',
  templateUrl: './blog-entry.component.html',
  styleUrls: ['./blog-entry.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BlogEntryComponent {
  vm$: Observable<{ post?: Post, postAlbum: AlbumPhotos | null, isLoading: boolean, error?: string, showEdit?: boolean }>;

  constructor(
    private activatedRoute: ActivatedRoute,
    private blogService: BlogService,
    private photosService: PhotosService,
    private auth: AuthService,
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
            map(album => ({ post, postAlbum: album, isLoading: false })),
          );
        } else {
          return of({ post, postAlbum: null, isLoading: false });
        }
      }),
      switchMap(vm => this.auth.isAuthenticated$.pipe(
        map(isAuthenticated => ({ ...vm, showEdit: isAuthenticated })),
      )),
      startWith({ postAlbum: null, isLoading: true }),
      catchError((err: unknown) => {
        let msg = 'An error occurred.';
        if (err instanceof HttpErrorResponse) {
          msg = err.message;
        } else {
          console.error(msg);
        }
        return of({ postAlbum: null, isLoading: false, error: msg });
      }),
    );
  }
}
