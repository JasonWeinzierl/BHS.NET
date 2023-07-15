import { Album } from './album';
import { Photo } from './photo';

export interface AlbumPhotos extends Album {
  photos: Array<Photo>;
}
