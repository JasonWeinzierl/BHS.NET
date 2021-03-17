// angular
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { paths } from './app-paths';
import { NotFoundComponent } from './core/component/not-found/not-found.component';
import { PathResolveService } from './core/service/path-resolve.service';

// components
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
        path: paths.location,
        loadChildren: () =>
          import('@modules/location/location.module').then(m => m.LocationModule)
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
          path: PathResolveService
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
