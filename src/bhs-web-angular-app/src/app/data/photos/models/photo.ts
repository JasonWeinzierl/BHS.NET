import { z } from 'zod';
import { authorSchema } from '@data/authors';

export const photoSchema = z.object({
  id: z.string(),
  name: z.string().nullish(),
  imagePath: z.string(),
  datePosted: z.coerce.date(),
  author: authorSchema.nullish(),
  description: z.string().nullish(),
});

export type Photo = z.infer<typeof photoSchema>;
