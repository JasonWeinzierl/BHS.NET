import { AlertThemeZodType, zAlertTheme } from 'bhs-generated-models';

export const alertThemeScheme = zAlertTheme;

export type AlertTheme = AlertThemeZodType;
export const AlertTheme = zAlertTheme.enum;
