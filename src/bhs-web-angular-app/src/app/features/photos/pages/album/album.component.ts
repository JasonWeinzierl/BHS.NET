import { AsyncPipe, NgOptimizedImage } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AlertModule } from 'ngx-bootstrap/alert';
import { catchError, map, Observable, of, startWith, switchMap } from 'rxjs';
import { SnippetPipe } from '../../../../shared/pipes/snippet.pipe';
import { AlbumPhotos, PhotosService } from '@data/photos';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrl: './album.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    AlertModule,
    RouterLink,
    NgOptimizedImage,
    SnippetPipe,
    AsyncPipe,
  ],
})
export class AlbumComponent {
  vm$: Observable<{ album?: AlbumPhotos, isLoading: boolean, error?: string }>;

  constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly photosService: PhotosService,
  ) {
    this.vm$ = this.activatedRoute.paramMap.pipe(
      map(params => {
        const slug = params.get('slug');
        if (!slug) {
          throw new Error('Failed to get album slug from URL.');
        }
        return slug;
      }),
      switchMap(slug => this.photosService.getAlbum(slug)),
      map(album => ({ album, isLoading: false })),
      startWith({ isLoading: true }),
      catchError((err: unknown) => {
        let msg = 'An error occurred.';
        if (err instanceof HttpErrorResponse) {
          msg = err.message;
        } else {
          console.error(err);
        }
        return of({ isLoading: false, error: msg });
      }),
    );
  }
}
