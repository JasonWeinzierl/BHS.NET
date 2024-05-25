import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { MockProvider } from 'ng-mocks';
import { EMPTY } from 'rxjs';
import { EntryNewComponent } from './entry-new.component';
import { AuthorService } from '@data/authors';
import { BlogService } from '@data/blog';

describe('EntryNewComponent', () => {
  let component: EntryNewComponent;
  let fixture: ComponentFixture<EntryNewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterModule.forRoot([]),
        EntryNewComponent,
      ],
      providers: [
        MockProvider(BlogService, {
          getCategories: () => EMPTY,
        }),
        MockProvider(AuthService, {
          user$: EMPTY,
        }),
        MockProvider(AuthorService, {
          getAuthors: () => EMPTY,
        }),
      ],
    });

    fixture = TestBed.createComponent(EntryNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
