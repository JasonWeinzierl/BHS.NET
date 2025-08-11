import { z } from 'zod';

export const authorSchema = z.object({
  username: z.string(),
  name: z.string().nullish(),
});

export type Author = z.infer<typeof authorSchema>;
