import { Category } from './category';
import { PostPreview } from './post-preview';

export interface CategoryPosts extends Category {
  posts: PostPreview[];
}
