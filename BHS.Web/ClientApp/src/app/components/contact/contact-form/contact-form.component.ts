import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ContactModel } from 'src/app/models/contact.model';
import { ContactService } from 'src/app/services/contact-service/contact.service';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.css']
})
export class ContactFormComponent implements OnInit {
  contactForm: FormGroup;
  isSubmitted: boolean = false;
  error: any = false;
  isSpam: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private contactService: ContactService
  ) {
    this.contactForm = this.formBuilder.group({
      name: '',
      emailAddress: '',
      message: '',
      body: ''
    });
  }

  ngOnInit() {
  }

  onSubmit(contactData: ContactModel) {
    this.contactForm.reset();
    this.isSpam = contactData.body != '' && contactData.body != null;
    contactData.dateRequested = new Date();
    this.contactService.sendMessage(contactData).subscribe(() => {
      this.isSubmitted = true;
    }, error => {
      this.error = error;
    });
  }
}
