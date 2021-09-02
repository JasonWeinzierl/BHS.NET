import { CommonModule } from '@angular/common';
import { ContactComponent } from './page/contact.component';
import { ContactFormComponent } from './page/contact-form/contact-form.component';
import { ContactRoutingModule } from './contact-routing.module';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    ContactComponent,
    ContactFormComponent
  ],
  imports: [
    // angular
    CommonModule,
    ReactiveFormsModule,

    // app
    ContactRoutingModule,
  ]
})
export class ContactModule { }
