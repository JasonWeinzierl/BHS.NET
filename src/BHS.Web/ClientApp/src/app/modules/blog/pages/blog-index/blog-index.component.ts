import { BlogService, CategorySummary, PostPreview } from '@data/blog';
import { Component, OnInit, TrackByFunction } from '@angular/core';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-blog-index',
  templateUrl: './blog-index.component.html',
  styleUrls: ['./blog-index.component.scss']
})
export class BlogIndexComponent implements OnInit {
  posts: PostPreview[] = [];
  categories: CategorySummary[] = [];

  searchText: string = '';
  loading = false;
  loadingPosts = false;

  constructor(
    private blogService: BlogService,
  ) { }

  ngOnInit(): void {
    this.loading = true;
    this.blogService.searchPosts()
      .pipe(
        finalize(() => this.loading = false),
      )
      .subscribe(response => this.posts = response);

    this.loading = true;
    this.blogService.getCategories()
      .pipe(
        finalize(() => this.loading = false),
      )
      .subscribe(response => this.categories = response);
  }

  onSearch(searchText: string): void {
    this.loadingPosts = true;
    this.blogService.searchPosts(searchText)
      .pipe(
        finalize(() => this.loadingPosts = false),
      )
      .subscribe(response => this.posts = response);
  }

  trackPostPreview: TrackByFunction<PostPreview> = (_, item) => {
    return item.slug;
  };
}
