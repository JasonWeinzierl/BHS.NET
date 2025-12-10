import { HttpErrorResponse } from '@angular/common/http';
import parseErrorMessage from './parseErrorMessage';

describe('parseErrorMessage', () => {
  it('should return the message from an Error instance', () => {
    const error = new Error('Test error message');

    expect(parseErrorMessage(error)).toBe('Test error message');
  });

  it('should return the message from an HttpErrorResponse instance', () => {
    const error = new HttpErrorResponse({
      url: 'https://example.com/',
      status: 500,
      statusText: 'Internal Server Error',
    });

    expect(parseErrorMessage(error)).toBe('Http failure response for https://example.com/: 500 Internal Server Error');
  });

  it('should return the message from an object with a message property', () => {
    const error = { message: 'Object error message' };

    expect(parseErrorMessage(error)).toBe('Object error message');
  });

  it('should return the string if the error is a string', () => {
    const error = 'String error message';

    expect(parseErrorMessage(error)).toBe('String error message');
  });

  it.each([
    null,
    undefined,
    0,
    42,
    true,
    false,
    { notMessage: 'No message here' },
    [],
    { message: 123 },
  ])('should return undefined for unknown error types', error => {
    expect(parseErrorMessage(error)).toBeUndefined();
  });
});
