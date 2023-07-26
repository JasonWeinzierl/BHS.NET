import { Category } from './category';
import { Author } from '@data/authors';

export interface PostPreview {
  slug: string;
  title: string;
  contentPreview: string;
  author?: Author;
  datePublished: Date;
  categories: Array<Category>;
}
