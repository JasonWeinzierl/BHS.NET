import { Author } from './author';

export interface PostPreview {
  slug: string;
  title: string;
  contentPreview: string;
  author?: Author;
  datePublished: Date;
}
