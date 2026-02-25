import { MuseumDayZodType, MuseumMonthRangeZodType, MuseumScheduleZodType } from 'bhs-generated-models';

export type MuseumDay = MuseumDayZodType;
export type MuseumMonthRange = MuseumMonthRangeZodType;
export type MuseumSchedule = MuseumScheduleZodType;

export { zMuseumDay as museumDaySchema, zMuseumSchedule as museumScheduleSchema, zMuseumMonthRange as museumMonthRangeSchema } from 'bhs-generated-models';
