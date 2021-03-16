import { Component, OnInit } from '@angular/core';

import { PostPreview } from '@data/schema/post-preview';
import { BlogService } from '@data/service/blog.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit {
  posts: PostPreview[];

  constructor(
    private blogService: BlogService
  ) { }

  ngOnInit() {
    this.blogService.searchPosts().subscribe(response => {
      this.posts = response;
    });
  }

}
