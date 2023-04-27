import { BlogEntryComponent } from './pages/blog-entry/blog-entry.component';
import { BlogIndexComponent } from './pages/blog-index/blog-index.component';
import { BlogRoutingModule } from './blog-routing.module';
import { CategoryPostsComponent } from './pages/category-posts/category-posts.component';
import { EntryAlbumComponent } from './components/entry-album/entry-album.component';
import { EntryEditComponent } from './pages/entry-edit/entry-edit.component';
import { MarkdownModule } from 'ngx-markdown';
import { NgModule } from '@angular/core';
import { PostCardComponent } from './components/post-card/post-card.component';
import { PostsSearchComponent } from './components/posts-search/posts-search.component';
import { SharedModule } from '@shared/shared.module';
import { EditBlogEntryFormComponent } from './components/edit-blog-entry-form/edit-blog-entry-form.component';

@NgModule({
  declarations: [
    BlogIndexComponent,
    PostCardComponent,
    BlogEntryComponent,
    CategoryPostsComponent,
    EntryAlbumComponent,
    PostsSearchComponent,
    EntryEditComponent,
    EditBlogEntryFormComponent,
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
