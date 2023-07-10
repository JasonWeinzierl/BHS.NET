import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MockComponent, MockProvider } from 'ng-mocks';
import { BlogIndexComponent } from './blog-index.component';
import { BlogService } from '@data/blog';
import { CategoriesListViewComponent } from '@modules/blog/components/categories-list-view/categories-list-view.component';
import { EMPTY } from 'rxjs';
import { PostsSearchComponent } from '@modules/blog/components/posts-search/posts-search.component';

describe('BlogIndexComponent', () => {
  let component: BlogIndexComponent;
  let fixture: ComponentFixture<BlogIndexComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        BlogIndexComponent,
        MockComponent(CategoriesListViewComponent),
        MockComponent(PostsSearchComponent),
      ],
      providers: [
        MockProvider(BlogService, {
          getCategories: () => EMPTY,
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
