import { NgModule } from '@angular/core';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminIndexComponent } from './pages/admin-index/admin-index.component';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [
    AdminIndexComponent,
  ],
  imports: [
    SharedModule,

    AdminRoutingModule,
  ],
})
export class AdminModule { }
