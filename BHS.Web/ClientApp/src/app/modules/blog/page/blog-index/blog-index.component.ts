import { Component, Input, OnInit } from '@angular/core';
import { Category } from '@app/data/schema/category';

import { PostPreview } from '@data/schema/post-preview';
import { BlogService } from '@data/service/blog.service';

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
    private blogService: BlogService
  ) { }

  ngOnInit(): void {
    this.blogService.searchPosts().subscribe(response => {
      this.posts = response;
    });
    this.blogService.getCategories().subscribe(response => {
      this.categories = response;
    });
  }

  onSearch(searchText: string): void {
    this.blogService.searchPosts(searchText).subscribe(response => {
      this.posts = response;
    });
  }

  trackPostPreview(_index: number, item: PostPreview): string {
    return item.slug;
  }
}
