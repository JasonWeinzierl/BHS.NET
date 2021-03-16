import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AboutComponent } from './page/about/about.component';
import { DonationsComponent } from './page/donations/donations.component';
import { OrganizationComponent } from './page/organization/organization.component';
import { PrivacyPolicyComponent } from './page/privacy-policy/privacy-policy.component';
import { TermsOfServiceComponent } from './page/terms-of-service/terms-of-service.component';
import { WhoWeAreComponent } from './page/who-we-are/who-we-are.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'this-site' },

  { path: 'who-we-are', component: WhoWeAreComponent, data: { title: 'Who We Are' } },
  { path: 'by-laws-and-leadership', component: OrganizationComponent, data: { title: 'By-laws and Leadership' } },
  { path: 'donate', component: DonationsComponent, data: { title: 'Donations' } },

  { path: 'this-site', component: AboutComponent, data: { title: 'About this Site' } },
  { path: 'terms-of-service', component: TermsOfServiceComponent, data: { title: 'Terms of Service' } },
  { path: 'privacy-policy', component: PrivacyPolicyComponent, data: { title: 'Privacy Policy' } },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AboutRoutingModule { }
