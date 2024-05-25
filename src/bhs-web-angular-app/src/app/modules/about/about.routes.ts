import { Routes } from '@angular/router';
import { AboutComponent } from './pages/about/about.component';
import { DonationsComponent } from './pages/donations/donations.component';
import { OrganizationComponent } from './pages/organization/organization.component';
import { PrivacyPolicyComponent } from './pages/privacy-policy/privacy-policy.component';
import { TermsOfServiceComponent } from './pages/terms-of-service/terms-of-service.component';
import { WhoWeAreComponent } from './pages/who-we-are/who-we-are.component';

export default [
  { path: '', pathMatch: 'full', redirectTo: 'this-site' },

  // TODO: use loadComponent for lazy loading in all routes.
  { path: 'who-we-are', component: WhoWeAreComponent, data: { title: 'Who We Are' } },
  { path: 'by-laws-and-leadership', component: OrganizationComponent, data: { title: 'By-laws and Leadership' } },
  { path: 'donate', component: DonationsComponent, data: { title: 'Donations' } },

  { path: 'this-site', component: AboutComponent, data: { title: 'About this Site' } },
  { path: 'terms-of-service', component: TermsOfServiceComponent, data: { title: 'Terms of Service' } },
  { path: 'privacy-policy', component: PrivacyPolicyComponent, data: { title: 'Privacy Policy' } },
] satisfies Routes;
