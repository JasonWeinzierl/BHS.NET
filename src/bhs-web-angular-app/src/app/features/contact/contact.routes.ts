import { Routes } from '@angular/router';
import { ContactComponent } from './pages/contact.component';

export default [
  {
    path: '',
    component: ContactComponent,
    title: 'Contact Us',
  },
] satisfies Routes;
