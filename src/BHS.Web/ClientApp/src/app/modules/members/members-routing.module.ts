import { RouterModule, Routes } from '@angular/router';
import { MembersListComponent } from './pages/members-list.component';
import { NgModule } from '@angular/core';

const routes: Routes = [
  {
    path: '',
    component: MembersListComponent,
    pathMatch: 'full',
    data: { title: 'Members' },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MembersRoutingModule { }
