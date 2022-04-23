import { BlogService, CategoryPosts } from '@data/blog';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-category-posts',
  templateUrl: './category-posts.component.html',
  styleUrls: ['./category-posts.component.scss']
})
export class CategoryPostsComponent implements OnInit {
  category?: CategoryPosts;
  error?: string;
  isLoading = false;

  constructor(
    private activatedRoute: ActivatedRoute,
    private blogService: BlogService,
  ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const slug = params.get('slug');

      if (!slug) {
        this.error = 'Failed to get category slug from URL.';
        return;
      }

      this.isLoading = true;
      this.loadCategory(slug);
    });
  }


  private loadCategory(slug: string): void {
    this.blogService.getCategory(slug)
      .subscribe(
        response => this.category = { ...response },
        (error: unknown) => {
          if (error instanceof HttpErrorResponse) {
            this.error = error.message;
          }
        })
      .add(() => this.isLoading = false);
  }
}
