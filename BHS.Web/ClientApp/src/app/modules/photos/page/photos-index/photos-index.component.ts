import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Album } from '@app/data/schema/album';
import { PhotosService } from '@app/data/service/photos.service';

@Component({
  selector: 'app-photos-index',
  templateUrl: './photos-index.component.html',
  styleUrls: ['./photos-index.component.scss']
})
export class PhotosIndexComponent implements OnInit {
  albums: Album[] = [];
  error: string;

  constructor(private photosService: PhotosService) { }

  ngOnInit(): void {
    this.photosService.getAlbums()
      .subscribe(response => this.albums = response,
        (error: HttpErrorResponse) => this.error = error.message);
  }

}