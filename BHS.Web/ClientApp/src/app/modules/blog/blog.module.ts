import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BlogRoutingModule } from './blog-routing.module';
import { BlogIndexComponent } from './page/blog-index/blog-index.component';
import { PostCardComponent } from './component/post-card/post-card.component';
import { EntryComponent } from './page/entry/entry.component';
import { FormsModule } from '@angular/forms';
import { CategoryPostsComponent } from './page/category-posts/category-posts.component';

@NgModule({
  declarations: [
    BlogIndexComponent,
    PostCardComponent,
    EntryComponent,
    CategoryPostsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,

    BlogRoutingModule
  ],
  exports: [
    PostCardComponent
  ]
})
export class BlogModule { }
