import { Routes } from '@angular/router';
import { AdminIndexComponent } from './pages/admin-index/admin-index.component';

export default [
  {
    path: '',
    pathMatch: 'full',
    component: AdminIndexComponent,
    data: { title: 'Administration Tools' },
  },
] satisfies Routes;
