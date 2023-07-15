import { Author } from '@data/authors';
import { Category } from './category';

export interface PostPreview {
  slug: string;
  title: string;
  contentPreview: string;
  author?: Author;
  datePublished: Date;
  categories: Array<Category>;
}
