import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BlogService } from '@data/service/blog.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Post } from '@data/schema/post';

@Component({
  selector: 'app-blog-entry',
  templateUrl: './blog-entry.component.html',
  styleUrls: ['./blog-entry.component.scss']
})
export class BlogEntryComponent implements OnInit {
  post?: Post;
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
        this.error = 'Failed to get entry slug from URL.';
        return;
      }

      this.isLoading = true;
      this.loadPost(slug);
    });
  }

  private loadPost(slug: string): void {
    this.blogService.getPost(slug)
      .subscribe(
        response => this.post = { ...response },
        (error: unknown) => {
          if (error instanceof HttpErrorResponse) {
            this.error = error.message;
          }
        })
      .add(() => this.isLoading = false);
  }
}
