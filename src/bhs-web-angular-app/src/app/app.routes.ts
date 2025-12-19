/* eslint-disable @typescript-eslint/promise-function-async */
import { Routes } from '@angular/router';
import { authGuardFn } from '@auth0/auth0-angular';
import { APP_PATHS } from './app-paths';
import { ContentLayoutComponent } from './core/components/content-layout/content-layout.component';
import { NotFoundComponent } from './core/components/not-found/not-found.component';
import { resolveClosestPath } from '@core/resolvers/closest-path.resolver';

export const APP_ROUTES: Routes = [
  {
    path: '',
    redirectTo: APP_PATHS.home,
    pathMatch: 'full',
  },
  {
    path: '',
    component: ContentLayoutComponent,
    children: [
      {
        path: APP_PATHS.home,
        loadChildren: () =>
          import('@features/home/home.routes'),
      },
      {
        path: APP_PATHS.blog,
        loadChildren: () =>
          import('@features/blog/blog.routes'),
      },
      {
        path: APP_PATHS.photos,
        loadChildren: () =>
          import('@features/photos/photos.routes'),
      },
      {
        path: APP_PATHS.location,
        loadChildren: () =>
          import('@features/location/location.routes'),
      },
      {
        path: APP_PATHS.profile,
        loadChildren: () =>
          import('@features/profile/profile.routes'),
      },
      {
        path: APP_PATHS.contact,
        loadChildren: () =>
          import('@features/contact/contact.routes'),
      },
      {
        path: APP_PATHS.about,
        loadChildren: () =>
          import('@features/about/about.routes'),
      },
      {
        path: 'admin',
        loadChildren: () =>
          import('@features/admin/admin.routes'),
        canActivate: [authGuardFn], // Must use canActivate because canLoad and canMatch don't seem to redirect to login properly.
      },
    ],
  },
  {
    path: '**',
    component: ContentLayoutComponent,
    children: [
      {
        path: '',
        component: NotFoundComponent,
        resolve: {
          closestPath: resolveClosestPath,
        },
        title: '404 Not Found',
      },
    ],
  },
];
