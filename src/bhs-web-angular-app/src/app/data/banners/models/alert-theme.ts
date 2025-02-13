import { z } from 'zod';

export const alertThemeScheme = z.enum([
  'None',
  'Primary',
  'Secondary',
  'Success',
  'Danger',
  'Warning',
  'Info',
]);

export type AlertTheme = z.infer<typeof alertThemeScheme>;

/**
 * Gets the Bootstrap alert type for a given alert theme.
 */
export function getBootstrapAlertType(theme: AlertTheme): string {
  if (alertThemeScheme.safeParse(theme).success && theme !== 'None') {
    return theme.toLowerCase();
  } else {
    return 'light';
  }
}
