/* eslint-disable @typescript-eslint/promise-function-async */
import { Routes } from '@angular/router';

export default [
  { path: '', pathMatch: 'full', redirectTo: 'this-site' },

  { path: 'who-we-are', loadComponent: () => import('./pages/who-we-are/who-we-are.component'), title: 'Who We Are' },
  { path: 'by-laws-and-leadership', loadComponent: () => import('./pages/organization/organization.component'), title: 'By-laws and Leadership' },
  { path: 'donate', loadComponent: () => import('./pages/donations/donations.component'), title: 'Donations' },

  { path: 'this-site', loadComponent: () => import('./pages/about/about.component'), title: 'About this Site' },
  { path: 'terms-of-service', loadComponent: () => import('./pages/terms-of-service/terms-of-service.component'), title: 'Terms of Service' },
  { path: 'privacy-policy', loadComponent: () => import('./pages/privacy-policy/privacy-policy.component'), title: 'Privacy Policy' },
] satisfies Routes;
