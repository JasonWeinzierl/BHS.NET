import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { BlogEntryComponent } from './pages/blog-entry/blog-entry.component';
import { BlogIndexComponent } from './pages/blog-index/blog-index.component';
import { CategoryPostsComponent } from './pages/category-posts/category-posts.component';
import { EntryEditComponent } from './pages/entry-edit/entry-edit.component';
import { EntryNewComponent } from './pages/entry-new/entry-new.component';

const routes: Routes = [
  {
    path: '',
    component: BlogIndexComponent,
    pathMatch: 'full',
    data: { title: 'News' },
  },
  {
    path: 'entry/:slug',
    component: BlogEntryComponent,
    data: { title: 'Post' },
  },
  {
    path: 'edit/:slug', // TODO: move under entries/:slug/edit ?
    component: EntryEditComponent,
    data: { title: 'Edit Entry' },
    canActivate: [AuthGuard],
  },
  {
    path: 'category/:slug',
    component: CategoryPostsComponent,
  },
  {
    path: 'new',
    component: EntryNewComponent,
    data: { title: 'New Entry' },
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BlogRoutingModule { }
