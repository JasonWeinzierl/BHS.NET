import { Author } from '@data/authors';
import { Category } from './category';

export interface PostRequest {
  title: string;
  contentMarkdown: string;
  filePath: string | null;
  photosAlbumSlug: string | null;
  author?: Author;
  datePublished: Date;
  categories: Array<Category>;
}