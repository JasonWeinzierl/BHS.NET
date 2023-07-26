import { Category } from './category';
import { Author } from '@data/authors';

export interface PostRequest {
  title: string;
  contentMarkdown: string;
  filePath: string | null;
  photosAlbumSlug: string | null;
  author: Author | null;
  datePublished: Date;
  categories: Array<Category>;
}
