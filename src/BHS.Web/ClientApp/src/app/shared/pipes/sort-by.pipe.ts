import { Pipe, PipeTransform } from '@angular/core';
import { orderBy } from 'lodash-es';

@Pipe({
  name: 'sortBy',
})
export class SortByPipe implements PipeTransform {

  /**
   * Sorts a collection.
   * @param value Array of any to be sorted.
   * @param order Sort order, `asc` by default.
   * @param column Property to sort by, array sorted with `.sort()` if empty.
   * @returns The sorted array.
   */
  transform<T>(value: T[], order: 'asc'|'desc' = 'asc', column?: string): T[] {
    if (!value || !order) {
      return value;
    }
    if (value.length <= 1) {
      return value;
    }
    if (!column || column === '') {
      if (order === 'asc') {
        return value.sort();
      } else {
        return value.sort().reverse();
      }
    }
    return orderBy(value, [column], [order]);
  }

}
