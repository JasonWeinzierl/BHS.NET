import { Author } from '@data/authors';

export interface Photo {
  id: string;
  name?: string;
  imagePath: string;
  datePosted: Date;
  author?: Author;
  description?: string;
}
