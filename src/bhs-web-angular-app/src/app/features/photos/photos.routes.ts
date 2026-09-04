import { Routes } from '@angular/router';
import { AlbumComponent } from './pages/album/album.component';
import { PhotosIndexComponent } from './pages/photos-index/photos-index.component';

export default [
  {
    path: '',
    pathMatch: 'full',
    component: PhotosIndexComponent,
    title: 'Photo Albums',
  },
  {
    path: 'album/:slug',
    component: AlbumComponent,
    title: 'Album',
    children: [
      {
        path: 'photo/:id',
        title: 'Photo',
        children: [],
      },
    ],
  },
] satisfies Routes;
