import { AsyncPipe, NgOptimizedImage } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { catchError, map, of, startWith, switchMap } from 'rxjs';
import { AlbumPhotos, Photo, PhotosService } from '@data/photos';
import parseErrorMessage from '@shared/parse-error-message';

interface AlbumPageVm {
  album?: AlbumPhotos;
  currentPhoto?: Photo;
  previousPhotoId: string;
  nextPhotoId: string;
  isLoading: boolean;
  error?: string;
}

@Component({
  selector: 'app-album-page',
  templateUrl: './album-page.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    RouterLink,
    NgOptimizedImage,
    AsyncPipe,
  ],
})
export class AlbumPageComponent {
  private readonly router = inject(Router);
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly photosService = inject(PhotosService);

  vm$ = this.activatedRoute.paramMap.pipe(
    map(parameters => {
      const albumSlug = parameters.get('slug');
      if (!albumSlug) {
        throw new Error('Failed to get album slug from URL.');
      }

      const photoId = parameters.get('id');
      if (!photoId) {
        throw new Error('Failed to get photo id from URL.');
      }

      return { albumSlug, photoId };
    }),
    switchMap(({ albumSlug, photoId }) => this.photosService.getAlbum$(albumSlug).pipe(
      map(album => {
        const currentIndex = album.photos.findIndex(p => p.id === photoId);

        if (currentIndex === -1) {
          this.router.navigate(['not-found'], { replaceUrl: true })
            .catch((error: unknown) => { console.error(error); });
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
    startWith({ previousPhotoId: '', nextPhotoId: '', isLoading: true } as AlbumPageVm),
    catchError((error: unknown) => {
      const message = parseErrorMessage(error) ?? 'An unknown error occurred.';
      return of({ previousPhotoId: '', nextPhotoId: '', error: message, isLoading: false } as AlbumPageVm);
    }),
  );
}
