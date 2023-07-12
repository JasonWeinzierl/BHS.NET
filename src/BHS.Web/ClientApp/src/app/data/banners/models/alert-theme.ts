export const ALERT_THEMES = [
  'None',
  'Primary',
  'Secondary',
  'Success',
  'Danger',
  'Warning',
  'Info',
] as const;

export type AlertTheme = typeof ALERT_THEMES[number];
