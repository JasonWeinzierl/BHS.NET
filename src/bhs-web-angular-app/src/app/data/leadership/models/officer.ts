import { z } from 'zod/v4';

export const officerSchema = z.object({
  title: z.string(),
  name: z.string(),
  dateStarted: z.coerce.date(),
});

export type Officer = z.infer<typeof officerSchema>;
