import { z } from 'zod/v4';

export const categorySchema = z.object({
  slug: z.string(),
  name: z.string(),
});

export type Category = z.infer<typeof categorySchema>;
