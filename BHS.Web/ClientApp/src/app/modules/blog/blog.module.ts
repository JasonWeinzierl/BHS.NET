import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BlogRoutingModule } from './blog-routing.module';
import { IndexComponent } from './page/index/index.component';
import { PostCardComponent } from './component/post-card/post-card.component';
import { EntryComponent } from './page/entry/entry.component';

@NgModule({
  declarations: [
    IndexComponent,
    PostCardComponent,
    EntryComponent
  ],
  imports: [
    CommonModule,

    BlogRoutingModule
  ]
})
export class BlogModule { }
