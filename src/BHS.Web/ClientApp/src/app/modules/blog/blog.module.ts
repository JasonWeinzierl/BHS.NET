import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MarkdownModule } from 'ngx-markdown';
import { IsLoadingDirectiveModule } from '@service-work/is-loading';

import { SharedModule } from '@shared/shared.module';

import { BlogRoutingModule } from './blog-routing.module';
import { BlogIndexComponent } from './page/blog-index/blog-index.component';
import { PostCardComponent } from './component/post-card/post-card.component';
import { BlogEntryComponent } from './page/blog-entry/blog-entry.component';
import { CategoryPostsComponent } from './page/category-posts/category-posts.component';

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
    IsLoadingDirectiveModule,

    SharedModule,
    BlogRoutingModule,
  ],
  exports: [
    PostCardComponent
  ]
})
export class BlogModule { }
