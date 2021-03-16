import { Category } from './category';

export interface Post {
  slug: string;
  title: string;
  contentMarkdown: string;
  filePath: string;
  photosAlbumId?: number;
  authorId?: number;
  datePublished: Date;
  dateLastModified: Date;
  categories: Category[];
}
