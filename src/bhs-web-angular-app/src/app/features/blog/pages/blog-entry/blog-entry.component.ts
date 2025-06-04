import { AsyncPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { ToastrService } from 'ngx-toastr';
import { catchError, map, of, startWith, switchMap } from 'rxjs';
import { BlogService } from '@data/blog';
import { PhotosService } from '@data/photos';
import { EntryAlbumComponent } from '@features/blog/components/entry-album/entry-album.component';
import { JsonLdBlogPostingComponent } from '@features/blog/components/json-ld-blog-posting/json-ld-blog-posting.component';
import { DateComponent } from '@shared/components/date/date.component';
import { MarkdownComponent } from '@shared/components/markdown/markdown.component';

@Component({
  selector: 'app-blog-entry',
  templateUrl: './blog-entry.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    RouterLink,
    DateComponent,
    MarkdownComponent,
    EntryAlbumComponent,
    AsyncPipe,
    JsonLdBlogPostingComponent,
],
})
export class BlogEntryComponent {
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly blogService = inject(BlogService);
  private readonly photosService = inject(PhotosService);
  private readonly auth = inject(AuthService);
  private readonly toastr = inject(ToastrService);

  vm$ = this.activatedRoute.paramMap.pipe(
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
          map(album => ({ post, postAlbum: album, isLoading: false, error: null })),
        );
      } else {
        return of({ post, postAlbum: null, isLoading: false, error: null });
      }
    }),
    switchMap(vm => this.auth.isAuthenticated$.pipe(
      startWith(false),
      map(isAuthenticated => ({ ...vm, showEdit: isAuthenticated })),
    )),
    startWith({ post: null, postAlbum: null, isLoading: true, error: null }),
    catchError((err: unknown) => {
      let msg = 'An error occurred.';
      if (err instanceof HttpErrorResponse) {
        msg = err.message;
      } else {
        console.error(msg, err);
      }
      return of({ post: null, postAlbum: null, isLoading: false, error: msg });
    }),
  );
}
