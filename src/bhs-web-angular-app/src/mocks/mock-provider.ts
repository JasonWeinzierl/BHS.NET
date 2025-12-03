import { Provider, Type, ValueProvider } from '@angular/core';

const defaultValue = {} as const;

/**
 * Poor man's MockProvider.
 */
export function MockProvider<I extends object>(
  provide: Type<I>,
  overrides: Partial<I> = defaultValue,
): Provider {
  return {
    provide,
    useValue: overrides,
  } satisfies ValueProvider;
}
