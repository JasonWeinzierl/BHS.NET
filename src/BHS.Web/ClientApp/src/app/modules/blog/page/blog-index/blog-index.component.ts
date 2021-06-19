import { Component, OnInit, TrackByFunction } from '@angular/core';
import { finalize } from 'rxjs/operators';

import { IsLoadingService } from '@service-work/is-loading';

import { PostPreview } from '@data/schema/post-preview';
import { BlogService } from '@data/service/blog.service';
import { CategorySummary } from '@data/schema/category-summary';

@Component({
  selector: 'app-blog-index',
  templateUrl: './blog-index.component.html',
  styleUrls: ['./blog-index.component.scss']
})
export class BlogIndexComponent implements OnInit {
  posts: PostPreview[] = [];
  categories: CategorySummary[] = [];

  searchText: string = '';

  constructor(
    private blogService: BlogService,
    private isLoadingService: IsLoadingService,
  ) { }

  ngOnInit(): void {
    this.isLoadingService.add();
    this.blogService.searchPosts()
      .pipe(
        finalize(() => this.isLoadingService.remove()),
      )
      .subscribe(response => this.posts = response);

    this.isLoadingService.add();
    this.blogService.getCategories()
      .pipe(
        finalize(() => this.isLoadingService.remove()),
      )
      .subscribe(response => this.categories = response);
  }

  onSearch(searchText: string): void {
    this.isLoadingService.add({key: 'searching-posts'});
    this.blogService.searchPosts(searchText)
      .pipe(
        finalize(() => this.isLoadingService.remove({key: 'searching-posts'})),
      )
      .subscribe(response => this.posts = response);
  }

  trackPostPreview: TrackByFunction<PostPreview> = (_, item) => {
    return item.slug;
  }
}
