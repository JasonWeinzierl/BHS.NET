import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MembersListComponent } from './page/members-list.component';
import { MembersRoutingModule } from './members-routing.module';

@NgModule({
  declarations: [MembersListComponent],
  imports: [
    CommonModule,

    MembersRoutingModule,
  ]
})
export class MembersModule { }
