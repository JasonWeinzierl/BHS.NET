import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Post } from '@data/schema/post';
import { BlogService } from '@data/service/blog.service';

@Component({
  selector: 'app-entry',
  templateUrl: './entry.component.html',
  styleUrls: ['./entry.component.scss']
})
export class EntryComponent implements OnInit {
  post: Post;
  error: string;

  constructor(
    private _activatedRoute: ActivatedRoute,
    private blogService: BlogService
  ) { }

  ngOnInit(): void {
    this._activatedRoute.paramMap.subscribe(params => {
      const slug = params.get('slug');
      this.blogService.getPost(slug)
        .subscribe(response => this.post = { ... response }, (error: HttpErrorResponse) => this.error = error.message);
    });
  }
}
