import { Component, OnInit } from '@angular/core';
import { Category } from '@app/data/schema/category';

import { PostPreview } from '@data/schema/post-preview';
import { BlogService } from '@data/service/blog.service';
import { IsLoadingService } from '@service-work/is-loading';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-blog-index',
  templateUrl: './blog-index.component.html',
  styleUrls: ['./blog-index.component.scss']
})
export class BlogIndexComponent implements OnInit {
  posts: PostPreview[];
  categories: Category[];

  searchText: string;

  constructor(
    private blogService: BlogService,
    private isLoadingService: IsLoadingService,
  ) { }

  ngOnInit(): void {
    this.isLoadingService.add();
    this.isLoadingService.add();

    this.blogService.searchPosts()
      .pipe(finalize(() => this.isLoadingService.remove()))
      .subscribe(response => this.posts = response);

    this.blogService.getCategories()
      .pipe(finalize(() => this.isLoadingService.remove()))
      .subscribe(response => this.categories = response);
  }

  onSearch(searchText: string): void {
    this.isLoadingService.add();
    this.blogService.searchPosts(searchText)
      .pipe(finalize(() => this.isLoadingService.remove()))
      .subscribe(response => this.posts = response);
  }

  trackPostPreview(_index: number, item: PostPreview): string {
    return item.slug;
  }
}
