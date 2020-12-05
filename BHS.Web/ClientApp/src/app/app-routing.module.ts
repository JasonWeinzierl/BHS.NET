// Angular
import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// Components
import { HomeComponent } from "./pages/home/home.component";
import { WhoWeAreComponent } from "./pages/who-we-are/who-we-are.component";
import { OrganizationComponent } from "./pages/organization/organization.component";
import { DonationsComponent } from "./pages/donations/donations.component";
import { AboutComponent } from "./pages/about/about.component";
import { TermsOfServiceComponent } from "./pages/terms-of-service/terms-of-service.component";
import { PrivacyPolicyComponent } from "./pages/privacy-policy/privacy-policy.component";
import { ContactComponent } from "./components/contact/contact.component";
import { LocationComponent } from "./location/location.component";
import { NotFoundComponent } from "./pages/not-found/not-found.component";

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full', data: { title: 'Home' } },
  { path: 'whoweare', component: WhoWeAreComponent, data: { title: 'Who We Are' } },
  { path: 'bylawsandleadership', component: OrganizationComponent, data: { title: 'By-laws and Leadership' } },
  { path: 'donate', component: DonationsComponent, data: { title: 'Donations' } },
  { path: 'about', component: AboutComponent, data: { title: 'About this Site' } },
  { path: 'termsofservice', component: TermsOfServiceComponent, data: { title: 'Terms of Service' } },
  { path: 'privacypolicy', component: PrivacyPolicyComponent, data: { title: 'Privacy Policy' } },
  { path: 'contact', component: ContactComponent, data: { title: 'Contact Us' } },
  { path: 'apps/location', component: LocationComponent, data: { title: 'Location' } },
  { path: 'not-found', component: NotFoundComponent, data: { title: '404 Not Found' } },
  { path: '**', redirectTo: '/not-found' }
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
