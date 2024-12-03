import { Pipe, PipeTransform } from '@angular/core';
import { orderBy } from 'lodash-es';

@Pipe({
  name: 'sortBy',
})
export class SortByPipe implements PipeTransform {
  /**
   * Sorts a collection.
   * @param value Array to be sorted.
   * @param order Sort order, `asc` by default.
   * @param column Property to sort by; uses default sort if not provided.
   * @returns The sorted array.
   */
  transform<T>(value: Array<T> | null, order: 'asc' | 'desc' = 'asc', column?: keyof T): Array<T> | null {
    if (!value) {
      return value;
    }
    if (value.length <= 1) {
      return value;
    }
    return orderBy(value, column, order);
  }
}
