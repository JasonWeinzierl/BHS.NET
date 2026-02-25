import { SiteBannerHistoryZodType, SiteBannerStatusChangeZodType } from 'bhs-generated-models';

export type SiteBannerStatusChange = SiteBannerStatusChangeZodType;

export type SiteBannerHistory = SiteBannerHistoryZodType;

export { zSiteBannerHistory as SITE_BANNER_HISTORY_SCHEMA, zSiteBannerStatusChange as SITE_BANNER_STATUS_CHANGE_SCHEMA } from 'bhs-generated-models';
