import { Author } from '@data/authors';
import { Photo } from './photo';

export interface Album {
  slug: string;
  name?: string;
  description?: string;
  bannerPhoto?: Photo;
  blogPostSlug?: string;
  author?: Author;
}
