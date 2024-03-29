import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminIndexComponent } from './pages/admin-index/admin-index.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: AdminIndexComponent,
    data: { title: 'Administration Tools' },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule { }
