/* eslint-disable @typescript-eslint/promise-function-async */
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { ContentLayoutComponent } from './core/components/content-layout/content-layout.component';
import { NgModule } from '@angular/core';
import { NotFoundComponent } from './core/components/not-found/not-found.component';
import { paths } from './app-paths';
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
          import('@modules/home/home.module').then(m => m.HomeModule),
      },
      {
        path: paths.blog,
        loadChildren: () =>
          import('@modules/blog/blog.module').then(m => m.BlogModule),
      },
      {
        path: paths.photos,
        loadChildren: () =>
          import('@modules/photos/photos.module').then(m => m.PhotosModule),
      },
      {
        path: paths.location,
        loadChildren: () =>
          import('@modules/location/location.module').then(m => m.LocationModule),
      },
      {
        path: paths.profile,
        loadChildren: () =>
          import('@modules/profile/profile.module').then(m => m.ProfileModule),
      },
      {
        path: paths.contact,
        loadChildren: () =>
          import('@modules/contact/contact.module').then(m => m.ContactModule),
      },
      {
        path: paths.about,
        loadChildren: () =>
          import('@modules/about/about.module').then(m => m.AboutModule),
      },
    ],
  },
  {
    path: 'admin',
    loadChildren: () =>
      import('@modules/admin/admin.module').then(m => m.AdminModule),
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
