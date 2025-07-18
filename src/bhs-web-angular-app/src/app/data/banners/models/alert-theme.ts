import { z } from 'zod/v4';

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
