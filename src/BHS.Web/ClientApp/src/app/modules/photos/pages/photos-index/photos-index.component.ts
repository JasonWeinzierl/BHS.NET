import { Album, PhotosService } from '@data/photos';
import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

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
      .subscribe({
        next: response => this.albums = response,
        error: (error: unknown) => {
          if (error instanceof HttpErrorResponse) {
            this.error = error.message;
          }
        }
      });
  }

}
