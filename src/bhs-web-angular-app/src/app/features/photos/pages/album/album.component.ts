import { AsyncPipe, NgOptimizedImage } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { catchError, map, of, startWith, switchMap } from 'rxjs';
import { PhotosService } from '@data/photos';
import parseErrorMessage from '@shared/parseErrorMessage';
import { SnippetPipe } from '@shared/pipes/snippet.pipe';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    RouterLink,
    NgOptimizedImage,
    SnippetPipe,
    AsyncPipe,
  ],
})
export class AlbumComponent {
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly photosService = inject(PhotosService);

  vm$ = this.activatedRoute.paramMap.pipe(
    map(params => {
      const slug = params.get('slug');
      if (!slug) {
        throw new Error('Failed to get album slug from URL.');
      }
      return slug;
    }),
    switchMap(slug => this.photosService.getAlbum$(slug)),
    map(album => ({ album, isLoading: false, error: null })),
    startWith({ album: null, isLoading: true, error: null }),
    catchError((err: unknown) => {
      const msg = parseErrorMessage(err) ?? 'An unknown error occurred.';
      return of({ album: null, isLoading: false, error: msg });
    }),
  );
}
