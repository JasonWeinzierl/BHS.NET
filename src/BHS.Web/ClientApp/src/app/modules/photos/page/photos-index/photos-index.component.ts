import { Component, OnInit } from '@angular/core';
import { Album } from '@data/schema/album';
import { HttpErrorResponse } from '@angular/common/http';
import { PhotosService } from '@data/service/photos.service';

@Component({
  selector: 'app-photos-index',
  templateUrl: './photos-index.component.html',
  styleUrls: ['./photos-index.component.scss']
})
export class PhotosIndexComponent implements OnInit {
  albums: Album[] = [];
  error?: string;

  constructor(private photosService: PhotosService) { }

  ngOnInit(): void {
    this.photosService.getAlbums()
      .subscribe(response => this.albums = response,
        (error: unknown) => {
          if (error instanceof HttpErrorResponse) {
            this.error = error.message;
          }
        });
  }

}
