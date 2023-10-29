import { z } from 'zod';
import { categorySchema } from './category';
import { authorSchema } from '@data/authors';

export const postSchema = z.object({
  slug: z.string(),
  title: z.string(),
  contentMarkdown: z.string(),
  filePath: z.string().nullish(),
  photosAlbumSlug: z.string().nullish(),
  author: authorSchema.nullish(),
  datePublished: z.coerce.date(),
  dateLastModified: z.coerce.date(),
  categories: categorySchema.array(),
});

export type Post = z.infer<typeof postSchema>;
