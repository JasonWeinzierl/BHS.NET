import { NgModule } from '@angular/core';
import { AlbumComponent } from './pages/album/album.component';
import { AlbumPageComponent } from './pages/album-page/album-page.component';
import { PhotosIndexComponent } from './pages/photos-index/photos-index.component';
import { PhotosRoutingModule } from './photos-routing.module';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [
    PhotosIndexComponent,
    AlbumComponent,
    AlbumPageComponent,
  ],
  imports: [
    SharedModule,

    PhotosRoutingModule,
  ],
})
export class PhotosModule { }
