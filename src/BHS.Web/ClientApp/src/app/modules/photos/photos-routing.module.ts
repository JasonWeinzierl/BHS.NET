import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AlbumPageComponent } from './page/album-page/album-page.component';
import { AlbumComponent } from './page/album/album.component';
import { PhotosIndexComponent } from './page/photos-index/photos-index.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: PhotosIndexComponent,
    data: { title: 'Photo Gallery' },
  },
  {
    path: 'album/:slug',
    component: AlbumComponent,
  },
  {
    path: 'album/:slug/photo/:id',
    component: AlbumPageComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PhotosRoutingModule { }
