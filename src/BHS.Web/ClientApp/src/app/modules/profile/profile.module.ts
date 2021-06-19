import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProfileIndexComponent } from './page/profile-index.component';
import { ProfileRoutingModule } from './profile-routing.module';
import { BlogModule } from '@modules/blog/blog.module';

@NgModule({
  declarations: [ProfileIndexComponent],
  imports: [
    CommonModule,

    BlogModule,
    ProfileRoutingModule
  ]
})
export class ProfileModule { }
