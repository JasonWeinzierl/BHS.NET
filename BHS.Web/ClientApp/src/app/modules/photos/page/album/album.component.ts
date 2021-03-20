import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PhotosService } from '@app/data/service/photos.service';
import { AlbumPhotos } from '@data/schema/album-photos';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrls: ['./album.component.scss']
})
export class AlbumComponent implements OnInit {
  album: AlbumPhotos;
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
    });
  }

}
