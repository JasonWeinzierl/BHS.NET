import { ContactComponent } from './pages/contact.component';
import { ContactFormComponent } from './pages/contact-form/contact-form.component';
import { ContactRoutingModule } from './contact-routing.module';
import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [
    ContactComponent,
    ContactFormComponent
  ],
  imports: [
    SharedModule,
    ContactRoutingModule,
  ]
})
export class ContactModule { }
