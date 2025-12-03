import { AsyncPipe } from '@angular/common';
import { Component, input } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, RouterLink } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { EMPTY } from 'rxjs';
import { BlogIndexComponent } from './blog-index.component';
import { BlogService } from '@data/blog';
import { MockProvider } from 'ng-mocks';

@Component({
  selector: 'app-categories-list-view',
  template: '',
})
class CategoriesListViewStubComponent {
  readonly isLoading = input(false);
  readonly categories = input([]);
  readonly error = input('');
}

@Component({
  selector: 'app-posts-search',
  template: '',
})
class PostsSearchStubComponent {}

describe('BlogIndexComponent', () => {
  let component: BlogIndexComponent;
  let fixture: ComponentFixture<BlogIndexComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        BlogIndexComponent,
      ],
      providers: [
        provideRouter([]),
        MockProvider(BlogService, {
          getCategories: () => EMPTY,
        }),
        MockProvider(AuthService, {
          isAuthenticated$: EMPTY,
        }),
      ],
    })
    .overrideComponent(BlogIndexComponent, {
      set: {
        imports: [
          CategoriesListViewStubComponent,
          PostsSearchStubComponent,
          AsyncPipe,
          RouterLink,
        ],
      },
    })
    .compileComponents();

    fixture = TestBed.createComponent(BlogIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
