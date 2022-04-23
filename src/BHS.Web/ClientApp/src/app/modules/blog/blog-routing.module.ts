import { RouterModule, Routes } from '@angular/router';
import { BlogEntryComponent } from './pages/blog-entry/blog-entry.component';
import { BlogIndexComponent } from './pages/blog-index/blog-index.component';
import { CategoryPostsComponent } from './pages/category-posts/category-posts.component';
import { NgModule } from '@angular/core';

const routes: Routes = [
  {
    path: '',
    component: BlogIndexComponent,
    pathMatch: 'full',
    data: { title: 'News' }
  },
  {
    path: 'entry/:slug',
    component: BlogEntryComponent,
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
