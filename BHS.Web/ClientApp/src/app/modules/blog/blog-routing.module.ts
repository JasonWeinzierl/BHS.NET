import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EntryComponent } from './page/entry/entry.component';
import { IndexComponent } from './page/index/index.component';

const routes: Routes = [
  {
    path: '',
    component: IndexComponent,
    pathMatch: 'full',
    data: { title: 'News' }
  },
  {
    path: 'entry/:slug',
    component: EntryComponent,
    data: { title: 'Post' }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogRoutingModule { }
