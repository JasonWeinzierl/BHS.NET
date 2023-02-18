import { BlogEntryComponent } from './pages/blog-entry/blog-entry.component';
import { BlogIndexComponent } from './pages/blog-index/blog-index.component';
import { BlogRoutingModule } from './blog-routing.module';
import { CategoryPostsComponent } from './pages/category-posts/category-posts.component';
import { EntryAlbumComponent } from './components/entry-album/entry-album.component';
import { MarkdownModule } from 'ngx-markdown';
import { NgModule } from '@angular/core';
import { PostCardComponent } from './components/post-card/post-card.component';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [
    BlogIndexComponent,
    PostCardComponent,
    BlogEntryComponent,
    CategoryPostsComponent,
    EntryAlbumComponent,
  ],
  imports: [
    SharedModule,

    MarkdownModule.forChild(),

    BlogRoutingModule,
  ],
  exports: [
    PostCardComponent,
  ],
})
export class BlogModule { }
