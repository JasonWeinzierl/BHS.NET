import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { AlbumPhotos } from '@data/schema/album-photos';
import { Photo } from '@data/schema/photo';
import { PhotosService } from '@data/service/photos.service';

@Component({
  selector: 'app-album-page',
  templateUrl: './album-page.component.html',
  styleUrls: ['./album-page.component.scss']
})
export class AlbumPageComponent implements OnInit {
  album?: AlbumPhotos;
  currentPhoto?: Photo;

  previousPhotoId: number = 0;
  nextPhotoId: number = 0;

  error?: string;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private photosService: PhotosService
  ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const albumSlug = params.get('slug');
      if (!albumSlug) {
        this.error = 'Failed to get album slug from URL.';
        return;
      }

      const photoIdStr = params.get('id');
      if (!photoIdStr) {
        this.error = 'Failed to get photo id from URL.';
        return;
      }
      const photoId = +photoIdStr;

      this.loadAlbum(albumSlug, photoId);
    });
  }

  private loadAlbum(albumSlug: string, photoId: number): void {
    this.photosService.getAlbum(albumSlug)
      .subscribe(response => {
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
      }, (error: unknown) => {
        if (error instanceof HttpErrorResponse) {
          this.error = error.message;
        }
      });
  }
}
