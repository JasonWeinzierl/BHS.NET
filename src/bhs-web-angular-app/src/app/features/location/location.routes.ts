import { Routes } from '@angular/router';
import { LocationComponent } from './pages/location.component';

export default [
  {
    path: '',
    component: LocationComponent,
    data: { title: 'Location' },
  },
] satisfies Routes;
