import { map, OperatorFunction } from 'rxjs';
import { ZodType } from 'zod/v4';

/**
 * Parses each value emitted by the source Observable
 * through the given zod schema,
 * and emits the resulting values as an Observable.
 */
export const parseSchema = <T>(schema: ZodType<T>): OperatorFunction<unknown, T> => {
  return map(data => schema.parse(data));
};

/**
 * Parses each value emitted by the source Observable
 * through the given zod schema as an array,
 * and emits the resulting values as an Observable.
 */
export const parseSchemaArray = <T>(schema: ZodType<T>): OperatorFunction<unknown, Array<T>> => {
  return map(data => schema.array().parse(data));
};
