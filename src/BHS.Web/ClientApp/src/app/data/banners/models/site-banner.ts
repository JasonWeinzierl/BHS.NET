import { AlertTheme } from './alert-theme';

export interface SiteBanner {
  theme: AlertTheme;
  lead: string | null;
  body: string | null;
}
