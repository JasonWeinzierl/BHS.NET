import { Photo } from './photo';
import { Author } from '@data/authors';

export interface Album {
  slug: string;
  name?: string;
  description?: string;
  bannerPhoto?: Photo;
  blogPostSlug?: string;
  author?: Author;
}
