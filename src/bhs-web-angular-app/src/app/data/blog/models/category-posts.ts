import { z } from 'zod/v4';
import { categorySchema } from './category';
import { postPreviewSchema } from './post-preview';

export const categoryPostsSchema = categorySchema.extend({
  posts: postPreviewSchema.array(),
});

export type CategoryPosts = z.infer<typeof categoryPostsSchema>;
