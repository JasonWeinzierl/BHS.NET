import { AlbumPhotos, PhotosService } from '@data/photos';
import { BlogService, Post } from '@data/blog';
import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-blog-entry',
  templateUrl: './blog-entry.component.html',
  styleUrls: ['./blog-entry.component.scss']
})
export class BlogEntryComponent implements OnInit {
  post?: Post;
  postAlbum$: Observable<AlbumPhotos> = of();
  error?: string;
  isLoading = false;

  constructor(
    private activatedRoute: ActivatedRoute,
    private blogService: BlogService,
    private photosService: PhotosService,
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
      .subscribe({
        next: response => {
          this.post = { ...response };
          if (this.post.photosAlbumSlug) {
            this.postAlbum$ = this.photosService.getAlbum(this.post.photosAlbumSlug);
          }
        },
        error: (error: unknown) => {
          if (error instanceof HttpErrorResponse) {
            this.error = error.message;
          }
        }
      })
      .add(() => this.isLoading = false);
  }
}
