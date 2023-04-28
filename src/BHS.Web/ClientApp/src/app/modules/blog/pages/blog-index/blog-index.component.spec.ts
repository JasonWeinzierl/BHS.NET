import { BlogService, CategorySummary } from '@data/blog';
import { Component, Input } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BlogIndexComponent } from './blog-index.component';
import { of } from 'rxjs';

@Component({
  selector: 'app-posts-search',
})
// eslint-disable-next-line @angular-eslint/component-class-suffix
class PostsSearchComponentStub { }

@Component({
  selector: 'app-categories-list-view',
})
// eslint-disable-next-line @angular-eslint/component-class-suffix
class CategoriesListViewComponentStub {
  @Input() isLoading = false;
  @Input() error?: string;
  @Input() categories: Array<CategorySummary> = [];
}

describe('BlogIndexComponent', () => {
  let component: BlogIndexComponent;
  let fixture: ComponentFixture<BlogIndexComponent>;

  beforeEach(async () => {
    const blogService = jasmine.createSpyObj<BlogService>('blogService', { 'getCategories': of() });

    await TestBed.configureTestingModule({
      declarations: [
        BlogIndexComponent,
        CategoriesListViewComponentStub,
        PostsSearchComponentStub,
      ],
      providers: [
        {
          provide: BlogService,
          useValue: blogService,
        },
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
