import { z } from 'zod/v4';

export const museumDaySchema = z.object({
  dayOfWeek: z.number().min(0).max(6),
  fromTime: z.string(),
  toTime: z.string(),
});

export const museumMonthRangeSchema = z.object({
  startMonth: z.number(),
  endMonth: z.number(),
});

export const museumScheduleSchema = z.object({
  days: z.array(museumDaySchema),
  months: museumMonthRangeSchema,
});

export type MuseumDay = z.infer<typeof museumDaySchema>;
export type MuseumMonthRange = z.infer<typeof museumMonthRangeSchema>;
export type MuseumSchedule = z.infer<typeof museumScheduleSchema>;
