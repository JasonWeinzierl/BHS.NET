import { MuseumDayZodType, MuseumMonthRangeZodType, MuseumScheduleZodType, zMuseumDay, zMuseumMonthRange, zMuseumSchedule } from 'bhs-generated-models';

export const museumDaySchema = zMuseumDay;

export const museumMonthRangeSchema = zMuseumMonthRange;

export const museumScheduleSchema = zMuseumSchedule;

export type MuseumDay = MuseumDayZodType;
export type MuseumMonthRange = MuseumMonthRangeZodType;
export type MuseumSchedule = MuseumScheduleZodType;
