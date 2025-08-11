import { z } from 'zod';

export const categorySchema = z.object({
  slug: z.string(),
  name: z.string(),
});

export type Category = z.infer<typeof categorySchema>;
