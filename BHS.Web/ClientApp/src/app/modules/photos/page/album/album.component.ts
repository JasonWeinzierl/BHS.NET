import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Album } from '@app/data/schema/album';
import { Photo } from '@app/data/schema/photo';
import { PhotosService } from '@app/data/service/photos.service';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrls: ['./album.component.scss']
})
export class AlbumComponent implements OnInit {
  album: Album;
  photos: Photo[] = [];
  errors: string[] = [];

  constructor(
    private activatedRoute: ActivatedRoute,
    private photosService: PhotosService
    ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const slug = params.get('slug');
      this.photosService.getAlbum(slug)
        .subscribe(response => this.album = response,
          (error: HttpErrorResponse) => this.errors.push(error.message));
      this.photosService.getPhotos(slug)
        .subscribe(response => this.photos = response,
          (error: HttpErrorResponse) => this.errors.push(error.message));
    });
  }

}
