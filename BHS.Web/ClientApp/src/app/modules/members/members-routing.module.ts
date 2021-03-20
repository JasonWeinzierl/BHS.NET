import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MembersListComponent } from './page/members-list.component';

const routes: Routes = [
  {
    path: '',
    component: MembersListComponent,
    pathMatch: 'full',
    data: { title: 'Members' }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MembersRoutingModule { }
