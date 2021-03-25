import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { CategoryPosts } from '@data/schema/category-posts';
import { BlogService } from '@data/service/blog.service';

@Component({
  selector: 'app-category-posts',
  templateUrl: './category-posts.component.html',
  styleUrls: ['./category-posts.component.scss']
})
export class CategoryPostsComponent implements OnInit {
  category: CategoryPosts;
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
      this.blogService.getCategory(slug)
        .subscribe(response => this.category = { ... response },
          (error: HttpErrorResponse) => this.error = error.message,
          () => this.isLoading = false);
    });
  }

}
