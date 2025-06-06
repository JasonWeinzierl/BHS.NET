import { z } from 'zod/v4';
import { photoSchema } from './photo';
import { authorSchema } from '@data/authors';

export const albumSchema = z.object({
  slug: z.string(),
  name: z.string().nullish(),
  description: z.string().nullish(),
  bannerPhoto: photoSchema.nullish(),
  blogPostSlug: z.string().nullish(),
  author: authorSchema.nullish(),
});

export type Album = z.infer<typeof albumSchema>;
