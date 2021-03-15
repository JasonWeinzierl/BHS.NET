import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '@shared/shared.module';

import { ContactRoutingModule } from './contact-routing.module';
import { ContactComponent } from './page/contact.component';
import { ContactFormComponent } from './page/contact-form/contact-form.component';

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
    SharedModule,
  ]
})
export class ContactModule { }
