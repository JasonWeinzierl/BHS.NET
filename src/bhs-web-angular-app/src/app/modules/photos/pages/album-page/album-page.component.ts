import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, map, Observable, of, startWith, switchMap } from 'rxjs';
import { AlbumPhotos, Photo, PhotosService } from '@data/photos';

type AlbumPageVm = Observable<{ album?: AlbumPhotos, currentPhoto?: Photo, previousPhotoId: string, nextPhotoId: string, isLoading: boolean, error?: string }>;

@Component({
  selector: 'app-album-page',
  templateUrl: './album-page.component.html',
  styleUrl: './album-page.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlbumPageComponent {
  vm$: AlbumPageVm;

  constructor(
    private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute,
    private readonly photosService: PhotosService,
  ) {
    this.vm$ = this.activatedRoute.paramMap.pipe(
      map(params => {
        const albumSlug = params.get('slug');
        if (!albumSlug) {
          throw new Error('Failed to get album slug from URL.');
        }

        const photoId = params.get('id');
        if (!photoId) {
          throw new Error('Failed to get photo id from URL.');
        }

        return { albumSlug, photoId };
      }),
      switchMap(({ albumSlug, photoId }) => this.photosService.getAlbum(albumSlug).pipe(
        map(album => {
          const currentIndex = album.photos.findIndex(p => p.id === photoId);

          if (currentIndex < 0) {
            this.router.navigate(['not-found'], { replaceUrl: true })
              .catch((err: unknown) => { console.error(err); });
            return { previousPhotoId: '', nextPhotoId: '', error: 'Not found.', isLoading: false };
          } else {
            const currentPhoto = album.photos[currentIndex];

            const previousIndex = currentIndex - 1 < 0 ? album.photos.length - 1 : currentIndex - 1;
            const nextIndex = currentIndex + 1 >= album.photos.length ? 0 : currentIndex + 1;

            const previousPhotoId = album.photos[previousIndex].id;
            const nextPhotoId = album.photos[nextIndex].id;

            return { album, currentPhoto, previousPhotoId, nextPhotoId, isLoading: false };
          }
        }),
      )),
      startWith({ previousPhotoId: '', nextPhotoId: '', isLoading: true }),
      catchError((err: unknown) => {
        let msg = 'An error occurred';
        if (err instanceof HttpErrorResponse) {
          msg = err.message;
        }
        console.error(msg, err);
        return of({ previousPhotoId: '', nextPhotoId: '', error: msg, isLoading: false });
      }),
    );
  }
}
