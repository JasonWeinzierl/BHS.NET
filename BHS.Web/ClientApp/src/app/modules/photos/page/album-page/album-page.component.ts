import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Album } from '@data/schema/album';
import { Photo } from '@data/schema/photo';
import { PhotosService } from '@data/service/photos.service';

@Component({
  selector: 'app-album-page',
  templateUrl: './album-page.component.html',
  styleUrls: ['./album-page.component.scss']
})
export class AlbumPageComponent implements OnInit {
  photo: Photo;
  album: Album;

  constructor(
    private activatedRoute: ActivatedRoute,
    private photosService: PhotosService
  ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const albumSlug = params.get('slug');
      const photoId = +params.get('id');
      this.photosService.getAlbum(albumSlug)
        .subscribe(response => this.album = response);
      this.photosService.getPhoto(albumSlug, photoId)
        .subscribe(response => this.photo = response);
    });
  }

}
