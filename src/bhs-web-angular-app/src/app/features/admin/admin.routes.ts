/* eslint-disable @typescript-eslint/promise-function-async */
import { Routes } from '@angular/router';
import { AdminIndexComponent } from './pages/admin-index/admin-index.component';

export default [
  {
    path: '',
    pathMatch: 'full',
    component: AdminIndexComponent,
    title: 'Administration Tools',
  },
  {
    path: 'banners',
    loadComponent: () => import('./pages/admin-banners/admin-banners.component'),
    title: 'Edit Banners',
  },
] satisfies Routes;
