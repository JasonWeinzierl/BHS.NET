import { z } from 'zod/v4';
import { categorySchema } from './category';

export const categorySummarySchema = categorySchema.extend({
  postsCount: z.number(),
});

export type CategorySummary = z.infer<typeof categorySummarySchema>;
