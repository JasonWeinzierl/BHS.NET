import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { AboutRoutingModule } from './about-routing.module';

import { AboutComponent } from './page/about/about.component';
import { DonationsComponent } from './page/donations/donations.component';
import { OrganizationComponent } from './page/organization/organization.component';
import { PrivacyPolicyComponent } from './page/privacy-policy/privacy-policy.component';
import { TermsOfServiceComponent } from './page/terms-of-service/terms-of-service.component';
import { WhoWeAreComponent } from './page/who-we-are/who-we-are.component';

@NgModule({
  declarations: [
    AboutComponent,
    DonationsComponent,
    OrganizationComponent,
    PrivacyPolicyComponent,
    TermsOfServiceComponent,
    WhoWeAreComponent
  ],
  imports: [
    CommonModule,

    AboutRoutingModule,
    SharedModule
  ]
})
export class AboutModule { }
