import { AsyncPipe, NgOptimizedImage } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AlertComponent } from 'ngx-bootstrap/alert';
import { catchError, map, of, startWith, switchMap } from 'rxjs';
import { SnippetPipe } from '../../../../shared/pipes/snippet.pipe';
import { PhotosService } from '@data/photos';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrl: './album.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    AlertComponent,
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
    switchMap(slug => this.photosService.getAlbum(slug)),
    map(album => ({ album, isLoading: false, error: null })),
    startWith({ album: null, isLoading: true, error: null }),
    catchError((err: unknown) => {
      let msg = 'An error occurred.';
      if (err instanceof HttpErrorResponse) {
        msg = err.message;
      } else {
        console.error(err);
      }
      return of({ album: null, isLoading: false, error: msg });
    }),
  );
}
