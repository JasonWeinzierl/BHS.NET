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
  author?: Author;
  posts: PostPreview[] = [];
  errors: string[] = [];

  constructor(
    private activatedRoute: ActivatedRoute,
    private authorService: AuthorService,
  ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const username = params.get('username');
      if (!username) {
        this.errors.push('Failed to get username from URL.');
        return;
      }

      this.loadAuthor(username);
    });
  }


  private loadAuthor(username: string): void {
    this.authorService.getAuthor(username)
      .subscribe(authorResponse => {
        this.author = authorResponse;
      }, (authorError: unknown) => {
        if (authorError instanceof HttpErrorResponse) {
          this.errors.push(authorError.message);
        }
      });

    this.authorService.getAuthorPosts(username)
      .subscribe(postsResponse => this.posts = postsResponse,
        (postsError: unknown) => {
          if (postsError instanceof HttpErrorResponse) {
            this.errors.push(postsError.message);
          }
        });
  }
}
