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
  {
    path: 'banners/create',
    loadComponent: () => import('./pages/admin-banner-create/admin-banner-create.component'),
    title: 'Create Banner',
  },
] satisfies Routes;
