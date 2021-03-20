import { PostPreview } from './post-preview';

export interface CategoryPosts {
  slug: string;
  name: string;
  postsCount: number;
  posts: PostPreview[];
}
