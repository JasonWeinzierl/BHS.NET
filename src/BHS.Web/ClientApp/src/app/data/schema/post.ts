import { Author } from './author';
import { Category } from './category';

export interface Post {
  slug: string;
  title: string;
  contentMarkdown: string;
  filePath: string;
  photosAlbumId?: number;
  author?: Author;
  datePublished: Date;
  dateLastModified: Date;
  categories: Category[];
}
