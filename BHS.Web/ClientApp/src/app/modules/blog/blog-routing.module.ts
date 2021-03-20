import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CategoryPostsComponent } from './page/category-posts/category-posts.component';
import { EntryComponent } from './page/entry/entry.component';
import { BlogIndexComponent } from './page/blog-index/blog-index.component';

const routes: Routes = [
  {
    path: '',
    component: BlogIndexComponent,
    pathMatch: 'full',
    data: { title: 'News' }
  },
  {
    path: 'entry/:slug',
    component: EntryComponent,
    data: { title: 'Post' }
  },
  {
    path: 'category/:slug',
    component: CategoryPostsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogRoutingModule { }
