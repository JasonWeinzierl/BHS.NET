/* eslint-disable @typescript-eslint/promise-function-async */
import { Routes } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { paths } from './app-paths';
import { ContentLayoutComponent } from './core/components/content-layout/content-layout.component';
import { NotFoundComponent } from './core/components/not-found/not-found.component';
import { resolveClosestPath } from '@core/resolvers/closest-path.resolver';

export const APP_ROUTES: Routes = [
  {
    path: '',
    redirectTo: paths.home,
    pathMatch: 'full',
  },
  {
    path: '',
    component: ContentLayoutComponent,
    children: [
      {
        path: paths.home,
        loadChildren: () =>
          import('@features/home/home.routes'),
      },
      {
        path: paths.blog,
        loadChildren: () =>
          import('@features/blog/blog.routes'),
      },
      {
        path: paths.photos,
        loadChildren: () =>
          import('@features/photos/photos.routes'),
      },
      {
        path: paths.location,
        loadChildren: () =>
          import('@features/location/location.routes'),
      },
      {
        path: paths.profile,
        loadChildren: () =>
          import('@features/profile/profile.routes'),
      },
      {
        path: paths.contact,
        loadChildren: () =>
          import('@features/contact/contact.routes'),
      },
      {
        path: paths.about,
        loadChildren: () =>
          import('@features/about/about.routes'),
      },
    ],
  },
  {
    path: 'admin',
    loadChildren: () =>
      import('@features/admin/admin.routes'),
    canActivate: [AuthGuard], // Must use canActivate because canLoad and canMatch don't seem to redirect to login properly.
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
        data: { title: '404 Not Found' },
      },
    ],
  },
];
