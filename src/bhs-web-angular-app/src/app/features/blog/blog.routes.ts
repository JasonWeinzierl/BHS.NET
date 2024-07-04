import { Routes } from '@angular/router';
import { authGuardFn } from '@auth0/auth0-angular';
import { BlogEntryComponent } from './pages/blog-entry/blog-entry.component';
import { BlogIndexComponent } from './pages/blog-index/blog-index.component';
import { CategoryPostsComponent } from './pages/category-posts/category-posts.component';
import { EntryEditComponent } from './pages/entry-edit/entry-edit.component';
import { EntryNewComponent } from './pages/entry-new/entry-new.component';

export default [
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
    canActivate: [authGuardFn],
  },
  {
    path: 'category/:slug',
    component: CategoryPostsComponent,
  },
  {
    path: 'new',
    component: EntryNewComponent,
    data: { title: 'New Entry' },
    canActivate: [authGuardFn],
  },
] satisfies Routes;
