import { Component, OnInit } from '@angular/core';
import { Author } from '@data/schema/author';
import { AuthorService } from '@data/service/author.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-members-list',
  templateUrl: './members-list.component.html',
  styleUrls: ['./members-list.component.scss']
})
export class MembersListComponent implements OnInit {
  authors: Author[] = [];
  error?: string;

  constructor(
    private authorService: AuthorService
  ) { }

  ngOnInit(): void {
    this.authorService.getAuthors().subscribe(response => this.authors = response,
      (error: unknown) => {
        if (error instanceof HttpErrorResponse) {
          this.error = error.message;
        }
      });
  }

}
