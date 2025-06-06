import { z } from 'zod/v4';

export const authorSchema = z.object({
  username: z.string(),
  name: z.string().nullish(),
});

export type Author = z.infer<typeof authorSchema>;
