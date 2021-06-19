import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { paths } from './app-paths';
import { NotFoundComponent } from './core/component/not-found/not-found.component';
import { PathResolveService } from './core/service/path-resolve.service';
import { ContentLayoutComponent } from './layout/content-layout/content-layout.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: paths.home,
    pathMatch: 'full'
  },
  {
    path: '',
    component: ContentLayoutComponent,
    children: [
      {
        path: paths.home,
        loadChildren: () =>
          import('@modules/home/home.module').then(m => m.HomeModule)
      },
      {
        path: paths.blog,
        loadChildren: () =>
          import('@modules/blog/blog.module').then(m => m.BlogModule)
      },
      {
        path: paths.photos,
        loadChildren: () =>
          import('@modules/photos/photos.module').then(m => m.PhotosModule)
      },
      {
        path: paths.location,
        loadChildren: () =>
          import('@modules/location/location.module').then(m => m.LocationModule)
      },
      {
        path: paths.members,
        loadChildren: () =>
          import('@modules/members/members.module').then(m => m.MembersModule)
      },
      {
        path: paths.profile,
        loadChildren: () =>
          import('@modules/profile/profile.module').then(m => m.ProfileModule)
      },
      {
        path: paths.contact,
        loadChildren: () =>
          import('@modules/contact/contact.module').then(m => m.ContactModule)
      },
      {
        path: paths.about,
        loadChildren: () =>
          import('@modules/about/about.module').then(m => m.AboutModule)
      }
    ]
  },
  {
    path: '**',
    component: ContentLayoutComponent,
    children: [
      {
        path: '',
        component: NotFoundComponent,
        resolve: {
          closestPath: PathResolveService
        },
        data: { title: '404 Not Found' }
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
