import { z } from 'zod/v4';
import { siteBannerSchema } from './site-banner';

export const SITE_BANNER_STATUS_CHANGE_SCHEMA = z.object({
  dateModified: z.coerce.date(),
  isEnabled: z.boolean(),
});

export type SiteBannerStatusChange = z.infer<typeof SITE_BANNER_STATUS_CHANGE_SCHEMA>;

export const SITE_BANNER_HISTORY_SCHEMA = siteBannerSchema.extend({
  statusChanges: z.array(SITE_BANNER_STATUS_CHANGE_SCHEMA),
});

export type SiteBannerHistory = z.infer<typeof SITE_BANNER_HISTORY_SCHEMA>;
