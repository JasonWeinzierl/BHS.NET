import { BlogModule } from '@modules/blog/blog.module';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ProfileIndexComponent } from './page/profile-index.component';
import { ProfileRoutingModule } from './profile-routing.module';

@NgModule({
  declarations: [ProfileIndexComponent],
  imports: [
    CommonModule,

    BlogModule,
    ProfileRoutingModule
  ]
})
export class ProfileModule { }
