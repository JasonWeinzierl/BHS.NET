import { NgModule } from '@angular/core';
import { ProfileIndexComponent } from './pages/profile-index.component';
import { ProfileRoutingModule } from './profile-routing.module';
import { BlogModule } from '@modules/blog/blog.module';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  imports: [
    ProfileIndexComponent,

    SharedModule,

    BlogModule,
    ProfileRoutingModule,
  ],
})
export class ProfileModule { }
