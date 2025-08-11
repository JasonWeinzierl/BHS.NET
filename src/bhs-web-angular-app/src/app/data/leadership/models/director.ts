import { z } from 'zod';

export const directorSchema = z.object({
  name: z.string(),
  year: z.string(),
});

export type Director = z.infer<typeof directorSchema>;
