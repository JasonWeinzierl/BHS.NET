import { AsyncPipe, NgOptimizedImage } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { catchError, map, of, startWith } from 'rxjs';
import { PhotosService } from '@data/photos';
import parseErrorMessage from '@shared/parseErrorMessage';
import { SnippetPipe } from '@shared/pipes/snippet.pipe';

@Component({
  selector: 'app-photos-index',
  templateUrl: './photos-index.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    NgOptimizedImage,
    RouterLink,
    SnippetPipe,
    AsyncPipe,
  ],
  host: { 'data-testid': 'PhotosIndex' },
})
export class PhotosIndexComponent {
  private readonly photosService = inject(PhotosService);

  vm$ = this.photosService.getAlbums$().pipe(
    map(albums => ({ albums, isLoading: false, error: null })),
    startWith({ albums: [], isLoading: true, error: null }),
    catchError((error: unknown) => {
      const msg = parseErrorMessage(error) ?? 'An unknown error occurred.';
      return of({ albums: [], isLoading: false, error: msg });
    }),
  );
}
