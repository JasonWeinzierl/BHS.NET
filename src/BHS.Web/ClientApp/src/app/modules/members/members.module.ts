import { CommonModule } from '@angular/common';
import { MembersListComponent } from './page/members-list.component';
import { MembersRoutingModule } from './members-routing.module';
import { NgModule } from '@angular/core';

@NgModule({
  declarations: [MembersListComponent],
  imports: [
    CommonModule,

    MembersRoutingModule,
  ]
})
export class MembersModule { }
