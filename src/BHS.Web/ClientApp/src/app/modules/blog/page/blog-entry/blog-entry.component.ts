import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Post } from '@data/schema/post';
import { BlogService } from '@data/service/blog.service';

@Component({
  selector: 'app-blog-entry',
  templateUrl: './blog-entry.component.html',
  styleUrls: ['./blog-entry.component.scss']
})
export class BlogEntryComponent implements OnInit {
  post: Post;
  error: string;
  isLoading = false;

  constructor(
    private activatedRoute: ActivatedRoute,
    private blogService: BlogService,
  ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const slug = params.get('slug');

      this.isLoading = true;
      this.blogService.getPost(slug)
        .subscribe(response => this.post = { ... response },
          (error: HttpErrorResponse) => this.error = error.message,
          () => this.isLoading = false);
    });
  }
}
