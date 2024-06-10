import { AsyncPipe, NgOptimizedImage } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AlertModule } from 'ngx-bootstrap/alert';
import { catchError, map, of, startWith } from 'rxjs';
import { SnippetPipe } from '../../../../shared/pipes/snippet.pipe';
import { PhotosService } from '@data/photos';

@Component({
  selector: 'app-photos-index',
  templateUrl: './photos-index.component.html',
  styleUrl: './photos-index.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    AlertModule,
    NgOptimizedImage,
    RouterLink,
    SnippetPipe,
    AsyncPipe,
  ],
})
export class PhotosIndexComponent {
  private readonly photosService = inject(PhotosService);

  vm$ = this.photosService.getAlbums().pipe(
    map(albums => ({ albums, isLoading: false, error: null })),
    startWith({ albums: [], isLoading: true, error: null }),
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
