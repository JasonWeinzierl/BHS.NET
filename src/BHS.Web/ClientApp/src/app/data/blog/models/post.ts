import { Author } from '@data/authors';
import { Category } from './category';

export interface Post {
  slug: string;
  title: string;
  contentMarkdown: string;
  filePath: string | null;
  photosAlbumSlug: string | null;
  author: Author | null;
  datePublished: Date; // TODO: all dates need to be parsed from string when deserialized in the data layer.
  dateLastModified: Date;
  categories: Category[];
}
