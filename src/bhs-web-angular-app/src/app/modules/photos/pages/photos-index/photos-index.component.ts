import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { catchError, map, Observable, of, startWith } from 'rxjs';
import { Album, PhotosService } from '@data/photos';

@Component({
  selector: 'app-photos-index',
  templateUrl: './photos-index.component.html',
  styleUrl: './photos-index.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PhotosIndexComponent {
  vm$: Observable<{ albums: Array<Album>, isLoading: boolean, error?: string }>;

  constructor(
    private readonly photosService: PhotosService,
  ) {
    this.vm$ = this.photosService.getAlbums()
      .pipe(
        map(albums => ({ albums, isLoading: false })),
        startWith({ albums: [], isLoading: true }),
        catchError((error: unknown) => {
          let msg = 'An error occurred.';
          if (error instanceof HttpErrorResponse) {
            msg = error.message;
          } else {
            console.error(error);
          }
          return of({ albums: [], isLoading: false, error: msg });
        }),
      );
  }
}
