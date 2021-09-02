import { AlbumComponent } from './page/album/album.component';
import { AlbumPageComponent } from './page/album-page/album-page.component';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { PhotosIndexComponent } from './page/photos-index/photos-index.component';
import { PhotosRoutingModule } from './photos-routing.module';

@NgModule({
  declarations: [
    PhotosIndexComponent,
    AlbumComponent,
    AlbumPageComponent,
  ],
  imports: [
    CommonModule,

    PhotosRoutingModule
  ]
})
export class PhotosModule { }
