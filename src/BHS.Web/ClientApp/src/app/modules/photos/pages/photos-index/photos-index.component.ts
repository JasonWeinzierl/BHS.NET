import { Album, PhotosService } from '@data/photos';
import { catchError, Observable, of, tap } from 'rxjs';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-photos-index',
  templateUrl: './photos-index.component.html',
  styleUrls: ['./photos-index.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PhotosIndexComponent {
  albums$: Observable<Array<Album>>;
  isLoading = true;
  error?: string;

  constructor(
    private photosService: PhotosService,
  ) {
    this.albums$ = this.photosService.getAlbums()
      .pipe(
        tap(() => this.isLoading = false),
        catchError((error: unknown) => {
          if (error instanceof HttpErrorResponse) {
            this.error = error.message;
          } else {
            this.error = 'An error occurred.';
          }
          return of();
        }),
      );
  }
}
