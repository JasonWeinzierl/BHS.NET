import { ActivatedRoute, Router } from '@angular/router';
import { AlbumPhotos, Photo, PhotosService } from '@data/photos';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-album-page',
  templateUrl: './album-page.component.html',
  styleUrls: ['./album-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default, // TODO: Refactor to OnPush
})
export class AlbumPageComponent implements OnInit {
  isLoading = true;
  album?: AlbumPhotos;
  currentPhoto?: Photo;

  previousPhotoId = '';
  nextPhotoId = '';

  error?: string;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private photosService: PhotosService,
  ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const albumSlug = params.get('slug');
      if (!albumSlug) {
        this.error = 'Failed to get album slug from URL.';
        return;
      }

      const photoId = params.get('id');
      if (!photoId) {
        this.error = 'Failed to get photo id from URL.';
        return;
      }

      this.loadAlbum(albumSlug, photoId);
    });
  }

  private loadAlbum(albumSlug: string, photoId: string): void {
    this.photosService.getAlbum(albumSlug)
      .subscribe({next: response => {
        this.album = response;
        this.currentPhoto = this.album.photos.find(p => p.id === photoId);

        if (!this.currentPhoto) {
          this.router.navigate(['not-found'], { replaceUrl: true });
        } else {
          const currentIndex = this.album.photos.indexOf(this.currentPhoto);

          const previousIndex = currentIndex - 1 < 0 ? this.album.photos.length - 1 : currentIndex - 1;
          const nextIndex = currentIndex + 1 >= this.album.photos.length ? 0 : currentIndex + 1;

          this.previousPhotoId = this.album.photos[previousIndex].id;
          this.nextPhotoId = this.album.photos[nextIndex].id;
        }
      }, error: (error: unknown) => {
        if (error instanceof HttpErrorResponse) {
          this.error = error.message;
        }
      }})
      .add(() => this.isLoading = false);
  }
}
