import { AsyncPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, DebugElement, input } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { provideRouter, RouterLink } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { BehaviorSubject } from 'rxjs';
import { BlogIndexComponent } from './blog-index.component';
import { BlogService, CategorySummary } from '@data/blog';
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
  let categoriesSubject$: BehaviorSubject<Array<CategorySummary>>;
  let isAuthenticatedSubject$: BehaviorSubject<boolean>;

  beforeEach(async () => {
    categoriesSubject$ = new BehaviorSubject<Array<CategorySummary>>([]);
    isAuthenticatedSubject$ = new BehaviorSubject(false);

    await TestBed.configureTestingModule({
      imports: [
        BlogIndexComponent,
      ],
      providers: [
        provideRouter([]),
        MockProvider(BlogService, {
          getCategories: () => categoriesSubject$,
        }),
        MockProvider(AuthService, {
          isAuthenticated$: isAuthenticatedSubject$.asObservable(),
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

  it('should not show New Post when not authenticated', () => {
    const compiled = fixture.nativeElement as HTMLElement;

    expect(compiled.querySelector('[data-testid="BlogIndex-NewPostButton"]')).toBeNull();
  });

  it('should show New Post when authenticated', () => {
    isAuthenticatedSubject$.next(true);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;

    expect(compiled.querySelector('[data-testid="BlogIndex-NewPostButton"]')).not.toBeNull();
  });

  it('should display categories from the service', () => {
    const testCategories: Array<CategorySummary> = [
      { slug: 'cat1', name: 'Category 1', postsCount: 5 },
      { slug: 'cat2', name: 'Category 2', postsCount: 3 },
    ];
    categoriesSubject$.next(testCategories);
    fixture.detectChanges();

    const categoriesListView = fixture.debugElement.query(By.directive(CategoriesListViewStubComponent)) as DebugElement | null;
    const categoriesInstance = categoriesListView?.componentInstance as CategoriesListViewStubComponent;

    expect(categoriesInstance).toBeTruthy();
    expect(categoriesInstance.categories()).toEqual(testCategories);
  });

  it('should display error on failure to load categories', () => {
    categoriesSubject$.error(new HttpErrorResponse({}));
    fixture.detectChanges();

    const categoriesListView = fixture.debugElement.query(By.directive(CategoriesListViewStubComponent)) as DebugElement | null;
    const categoriesInstance = categoriesListView?.componentInstance as CategoriesListViewStubComponent;

    expect(categoriesInstance).toBeTruthy();
    expect(categoriesInstance.error()).toContain('Http failure');
  });
});
