/* eslint-disable @typescript-eslint/promise-function-async */
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { paths } from './app-paths';
import { ContentLayoutComponent } from './core/components/content-layout/content-layout.component';
import { NotFoundComponent } from './core/components/not-found/not-found.component';
import { resolveClosestPath } from '@core/resolvers/closest-path.resolver';

const routes: Routes = [
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
          import('@modules/home/home.routes'),
      },
      {
        path: paths.blog,
        loadChildren: () =>
          import('@modules/blog/blog.routes'),
      },
      {
        path: paths.photos,
        loadChildren: () =>
          import('@modules/photos/photos.routes'),
      },
      {
        path: paths.location,
        loadChildren: () =>
          import('@modules/location/location.routes'),
      },
      {
        path: paths.profile,
        loadChildren: () =>
          import('@modules/profile/profile.routes'),
      },
      {
        path: paths.contact,
        loadChildren: () =>
          import('@modules/contact/contact.routes'),
      },
      {
        path: paths.about,
        loadChildren: () =>
          import('@modules/about/about.routes'),
      },
    ],
  },
  {
    path: 'admin',
    loadChildren: () =>
      import('@modules/admin/admin.routes'),
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

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
