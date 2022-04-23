import { Category } from './category';

export interface CategorySummary extends Category {
  postsCount: number;
}
