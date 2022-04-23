import { MembersListComponent } from './pages/members-list.component';
import { MembersRoutingModule } from './members-routing.module';
import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [MembersListComponent],
  imports: [
    SharedModule,

    MembersRoutingModule,
  ]
})
export class MembersModule { }
