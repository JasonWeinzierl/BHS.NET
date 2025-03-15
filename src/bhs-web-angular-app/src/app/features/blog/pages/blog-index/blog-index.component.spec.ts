import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { MockComponent, MockProvider } from 'ng-mocks';
import { EMPTY } from 'rxjs';
import { BlogIndexComponent } from './blog-index.component';
import { BlogService } from '@data/blog';
import { CategoriesListViewComponent } from '@features/blog/components/categories-list-view/categories-list-view.component';
import { PostsSearchComponent } from '@features/blog/components/posts-search/posts-search.component';

describe('BlogIndexComponent', () => {
  let component: BlogIndexComponent;
  let fixture: ComponentFixture<BlogIndexComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        BlogIndexComponent,
        MockComponent(CategoriesListViewComponent),
        MockComponent(PostsSearchComponent),
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
    .compileComponents();

    fixture = TestBed.createComponent(BlogIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
