import { AlertThemeZodType, zAlertTheme } from 'bhs-generated-models';

export type AlertTheme = AlertThemeZodType;
export const AlertTheme = zAlertTheme.enum;

export { zAlertTheme as alertThemeScheme } from 'bhs-generated-models';
