import { AsyncPipe } from '@angular/common';
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
import parseErrorMessage from '@shared/parse-error-message';

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
    map(parameters => {
      const slug = parameters.get('slug');
      if (!slug) {
        throw new Error('Failed to get entry slug from URL.');
      }
      return slug;
    }),
    switchMap(slug => this.blogService.getPost$(slug)),
    switchMap(post => {
      return post.photosAlbumSlug
        ? this.photosService.getAlbum$(post.photosAlbumSlug).pipe(
          // Alert but don't prevent the entire post from loading.
          catchError((error: unknown) => {
            const message = parseErrorMessage(error) ?? 'An unknown error occurred.';
            this.toastr.error(message, 'Failed to load photos.');
            console.warn(message, error);
            return of(undefined);
          }),
          map(album => ({ post, postAlbum: album, isLoading: false, error: undefined })),
        )
        : of({ post, postAlbum: undefined, isLoading: false, error: undefined });
    }),
    switchMap(vm => this.auth.isAuthenticated$.pipe(
      startWith(false),
      map(isAuthenticated => ({ ...vm, showEdit: isAuthenticated })),
    )),
    startWith({ post: undefined, postAlbum: undefined, isLoading: true, error: undefined }),
    catchError((error: unknown) => {
      const message = parseErrorMessage(error) ?? 'An unknown error occurred.';
      return of({ post: undefined, postAlbum: undefined, isLoading: false, error: message });
    }),
  );
}
