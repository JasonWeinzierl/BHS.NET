import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProfileIndexComponent } from './page/profile-index.component';

const routes: Routes = [
  {
    path: ':username',
    pathMatch: 'full',
    component: ProfileIndexComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProfileRoutingModule { }
