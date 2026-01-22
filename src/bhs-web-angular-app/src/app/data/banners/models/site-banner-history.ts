import { SiteBannerHistoryZodType, SiteBannerStatusChangeZodType, zSiteBannerHistory, zSiteBannerStatusChange } from 'bhs-generated-models';

export const SITE_BANNER_STATUS_CHANGE_SCHEMA = zSiteBannerStatusChange;

export type SiteBannerStatusChange = SiteBannerStatusChangeZodType;

export const SITE_BANNER_HISTORY_SCHEMA = zSiteBannerHistory;

export type SiteBannerHistory = SiteBannerHistoryZodType;
