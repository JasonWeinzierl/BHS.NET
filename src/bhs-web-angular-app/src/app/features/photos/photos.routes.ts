import { Routes } from '@angular/router';
import { AlbumComponent } from './pages/album/album.component';
import { AlbumPageComponent } from './pages/album-page/album-page.component';
import { PhotosIndexComponent } from './pages/photos-index/photos-index.component';

export default [
  {
    path: '',
    pathMatch: 'full',
    component: PhotosIndexComponent,
    data: { title: 'Photo Albums' },
  },
  {
    path: 'album/:slug',
    component: AlbumComponent,
    data: { title: 'Album' },
  },
  {
    path: 'album/:slug/photo/:id',
    component: AlbumPageComponent,
    data: { title: 'Photo' },
  },
] satisfies Routes;
