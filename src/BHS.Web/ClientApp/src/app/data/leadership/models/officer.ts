import { z } from 'zod';

export const officerSchema = z.object({
  title: z.string(),
  name: z.string(),
  dateStarted: z.coerce.date(),
});

export type Officer = z.infer<typeof officerSchema>;
