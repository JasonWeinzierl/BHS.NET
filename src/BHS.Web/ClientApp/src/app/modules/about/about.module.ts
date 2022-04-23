import { AboutComponent } from './pages/about/about.component';
import { AboutRoutingModule } from './about-routing.module';
import { DonationsComponent } from './pages/donations/donations.component';
import { NgModule } from '@angular/core';
import { OrganizationComponent } from './pages/organization/organization.component';
import { PrivacyPolicyComponent } from './pages/privacy-policy/privacy-policy.component';
import { SharedModule } from '@shared/shared.module';
import { TermsOfServiceComponent } from './pages/terms-of-service/terms-of-service.component';
import { WhoWeAreComponent } from './pages/who-we-are/who-we-are.component';

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
    SharedModule,

    AboutRoutingModule,
  ]
})
export class AboutModule { }
