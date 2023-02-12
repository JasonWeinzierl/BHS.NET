import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'snippet'
})
export class SnippetPipe implements PipeTransform {

  /**
   * Gets a snippet from a string.
   * @param value The string to get a snippet from.
   * @param length The desired length of the snippet.
   * @param fuzz How much leeway to use when cutting the end of the string instead of just truncating mid-word.
   * @returns A beginning snippet of the given string.
   */
  transform(value: string | null | unknown, length = 80, fuzz = 5): string | null | unknown {
    if (!value || !(typeof(value) === 'string')) {
      return value;
    }
    if (value.length <= length + fuzz) {
      return value;
    }

    // Find a space character near the end of length.
    const goodPlaceToSlice = value.slice(0, length + fuzz).lastIndexOf(' ');

    // If the nearest space is outside the fuzz range, just truncate.
    const end = goodPlaceToSlice < length - fuzz ? length : goodPlaceToSlice;

    return value.slice(0, end) + '...';
  }

}
