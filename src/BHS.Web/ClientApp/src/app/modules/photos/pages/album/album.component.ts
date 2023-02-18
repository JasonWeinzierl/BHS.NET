import { AlbumPhotos, PhotosService } from '@data/photos';
import { catchError, filter, map, Observable, of, switchMap, tap } from 'rxjs';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrls: ['./album.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlbumComponent {
  album$: Observable<AlbumPhotos>;
  isLoading = true;
  error?: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private photosService: PhotosService,
    ) {
      this.album$ = this.activatedRoute.paramMap.pipe(
        map(params => {
          const slug = params.get('slug');
          if (!slug) {
            this.error = 'Failed to get album slug from URL.';
          }
          return slug;
        }),
        filter(slug => !!slug),
        switchMap(slug => this.photosService.getAlbum(slug ?? '')),
        tap(() => this.isLoading = false),
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
