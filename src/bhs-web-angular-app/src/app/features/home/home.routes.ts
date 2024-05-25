import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home.component';

export default [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full',
    data: { title: 'Home' },
  },
] satisfies Routes;
