import { HttpErrorResponse } from '@angular/common/http';
import { ZodError } from 'zod';

/**
 * Gets an error message from an `unknown`.
 */
export default function parseErrorMessage(
  error: unknown,
): string | undefined {
  if (error instanceof ZodError) {
    return 'Validation error: ' + error.issues.map(issue => issue.message).join(', ');
  }
  if (
    error instanceof Error
    || error instanceof HttpErrorResponse
  ) {
    return error.message;
  }
  if (
    typeof error === 'object'
    && error
    && 'message' in error
    && typeof error.message === 'string'
  ) {
    return error.message;
  }
  if (typeof error === 'string') {
    return error;
  }

  return undefined;
}
