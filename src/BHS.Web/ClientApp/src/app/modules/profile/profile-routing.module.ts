import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { ProfileIndexComponent } from './pages/profile-index.component';

const routes: Routes = [
  {
    path: ':username',
    pathMatch: 'full',
    component: ProfileIndexComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProfileRoutingModule { }
