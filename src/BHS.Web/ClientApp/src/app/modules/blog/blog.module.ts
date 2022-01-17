import { BlogEntryComponent } from './page/blog-entry/blog-entry.component';
import { BlogIndexComponent } from './page/blog-index/blog-index.component';
import { BlogRoutingModule } from './blog-routing.module';
import { CategoryPostsComponent } from './page/category-posts/category-posts.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MarkdownModule } from 'ngx-markdown';
import { NgModule } from '@angular/core';
import { PostCardComponent } from './component/post-card/post-card.component';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [
    BlogIndexComponent,
    PostCardComponent,
    BlogEntryComponent,
    CategoryPostsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,

    MarkdownModule.forChild(),

    SharedModule,
    BlogRoutingModule,
  ],
  exports: [
    PostCardComponent
  ]
})
export class BlogModule { }
