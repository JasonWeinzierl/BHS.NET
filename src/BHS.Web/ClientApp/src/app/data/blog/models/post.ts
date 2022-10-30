import { Author } from '@data/authors';
import { Category } from './category';

export interface Post {
  slug: string;
  title: string;
  contentMarkdown: string;
  filePath: string | null;
  photosAlbumSlug: string | null;
  author?: Author;
  datePublished: Date;
  dateLastModified: Date;
  categories: Category[];
}
