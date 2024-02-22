import { NgModule } from '@angular/core';
import { ContactRoutingModule } from './contact-routing.module';
import { ContactFormComponent } from './pages/contact-form/contact-form.component';
import { ContactComponent } from './pages/contact.component';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [
    ContactComponent,
    ContactFormComponent,
  ],
  imports: [
    SharedModule,
    ContactRoutingModule,
  ],
})
export class ContactModule { }
