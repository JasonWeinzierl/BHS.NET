import { Routes } from '@angular/router';
import { ProfileIndexComponent } from './pages/profile-index.component';

export default [
  {
    path: ':username',
    pathMatch: 'full',
    component: ProfileIndexComponent,
  },
] satisfies Routes;
