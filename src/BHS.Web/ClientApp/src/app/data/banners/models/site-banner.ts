import { z } from 'zod';
import { alertThemeScheme } from './alert-theme';

export const siteBannerSchema = z.object({
  id: z.string(),
  theme: alertThemeScheme,
  lead: z.string().nullish(),
  body: z.string().nullish(),
});

export type SiteBanner = z.infer<typeof siteBannerSchema>;
