import { AlertTheme } from './alert-theme';

export interface SiteBannerRequest {
  theme: AlertTheme;
  lead?: string;
  body?: string;
  endDate?: string;
}
