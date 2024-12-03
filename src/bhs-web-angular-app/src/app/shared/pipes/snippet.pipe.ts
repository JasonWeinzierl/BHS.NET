import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'snippet',
})
export class SnippetPipe implements PipeTransform {
  /**
   * Gets a snippet from a string.
   * @param value The string to get a snippet from.
   * @param length The desired length of the snippet.
   * @param fuzz How much leeway to use when cutting the end of the string instead of just truncating mid-word.
   * @returns A beginning snippet of the given string.
   */
  transform(value: string | null, length = 80, fuzz = 5): string | null {
    if (!value) {
      return value;
    }
    if (value.length <= length + fuzz) {
      return value;
    }

    // Find a space character near the end of desired length.
    const goodPlaceToSlice = value.lastIndexOf(' ', length + fuzz);

    // If the nearest space is below the fuzz range, just truncate.
    const end = goodPlaceToSlice < length - fuzz ? length : goodPlaceToSlice;

    return value.slice(0, end) + '...';
  }
}
