import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Author } from '@data/schema/author';
import { PostPreview } from '@data/schema/post-preview';
import { AuthorService } from '@data/service/author.service';


@Component({
  selector: 'app-profile-index',
  templateUrl: './profile-index.component.html',
  styleUrls: ['./profile-index.component.scss']
})
export class ProfileIndexComponent implements OnInit {
  author: Author;
  posts: PostPreview[];
  errors: string[] = [];

  constructor(
    private activatedRoute: ActivatedRoute,
    private authorService: AuthorService,
  ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const username = params.get('username');
      this.authorService.getAuthor(username)
        .subscribe(authorResponse => {
          this.author = authorResponse;
        }, (authorError: HttpErrorResponse) => this.errors.push(authorError.message));
      this.authorService.getAuthorPosts(username)
        .subscribe(postsResponse => this.posts = postsResponse,
          (postsError: HttpErrorResponse) => this.errors.push(postsError.message));
    });
  }

}