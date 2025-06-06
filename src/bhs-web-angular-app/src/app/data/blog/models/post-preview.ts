import { z } from 'zod/v4';
import { categorySchema } from './category';
import { authorSchema } from '@data/authors';

export const postPreviewSchema = z.object({
  slug: z.string(),
  title: z.string(),
  contentPreview: z.string(),
  author: authorSchema.nullish(),
  datePublished: z.coerce.date(),
  categories: categorySchema.array(),
});

export type PostPreview = z.infer<typeof postPreviewSchema>;
