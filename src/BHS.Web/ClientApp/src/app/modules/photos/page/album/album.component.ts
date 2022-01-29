import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlbumPhotos } from '@data/schema/album-photos';
import { HttpErrorResponse } from '@angular/common/http';
import { PhotosService } from '@data/service/photos.service';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrls: ['./album.component.scss']
})
export class AlbumComponent implements OnInit {
  album?: AlbumPhotos;
  errors: string[] = [];

  constructor(
    private activatedRoute: ActivatedRoute,
    private photosService: PhotosService
    ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const slug = params.get('slug');
      if (!slug) {
        this.errors.push('Failed to get album slug from URL.');
        return;
      }

      this.loadAlbum(slug);
    });
  }

  private loadAlbum(slug: string): void {
    this.photosService.getAlbum(slug)
      .subscribe({
        next: response => this.album = response,
        error: (error: unknown) => {
          if (error instanceof HttpErrorResponse) {
            this.errors.push(error.message);
          }
        }
      });
  }
}
