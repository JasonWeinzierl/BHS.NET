import { Component, OnInit } from '@angular/core';
import { Category } from '@app/data/schema/category';

import { PostPreview } from '@data/schema/post-preview';
import { BlogService } from '@data/service/blog.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit {
  posts: PostPreview[];
  categories: Category[];

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

}
