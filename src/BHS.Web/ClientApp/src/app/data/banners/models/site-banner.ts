import { AlertTheme } from './alert-theme';

export interface SiteBanner {
  theme: AlertTheme;
  lead?: string;
  body?: string;
}
