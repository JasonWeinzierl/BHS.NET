import { AsyncPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, DebugElement, input } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { provideRouter, RouterLink } from '@angular/router';
import { MockProvider } from 'ng-mocks';
import { BehaviorSubject } from 'rxjs';
import { PermissionsService } from '@core/services/permissions.service';
import { BlogService, CategorySummary } from '@data/blog';
import { BlogIndexComponent } from './blog-index.component';

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
  let canWriteBlogSubject$: BehaviorSubject<boolean>;

  beforeEach(async () => {
    categoriesSubject$ = new BehaviorSubject<Array<CategorySummary>>([]);
    isAuthenticatedSubject$ = new BehaviorSubject(false);
    canWriteBlogSubject$ = new BehaviorSubject(false);

    await TestBed.configureTestingModule({
      imports: [
        BlogIndexComponent,
      ],
      providers: [
        provideRouter([]),
        MockProvider(BlogService, {
          getCategories$: () => categoriesSubject$,
        }),
        MockProvider(PermissionsService, {
          isAuthenticated$: isAuthenticatedSubject$.asObservable(),
          hasPermission$: () => canWriteBlogSubject$.asObservable(),
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
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should not show New Post when not authenticated', () => {
    const compiled = fixture.nativeElement as HTMLElement;

    expect(compiled.querySelector('[data-testid="BlogIndex-NewPostButton"]')).toBeNull();
  });

  it('should disable New Post when authenticated without blog permission', async () => {
    isAuthenticatedSubject$.next(true);
    await fixture.whenStable();
    const newPostButton = (fixture.nativeElement as HTMLElement)
      .querySelector<HTMLAnchorElement>('[data-testid="BlogIndex-NewPostButton"]');

    expect(newPostButton).not.toBeNull();
    expect(newPostButton?.getAttribute('href')).toBeNull();
    expect(newPostButton?.getAttribute('aria-disabled')).toBe('true');
  });

  it('should enable New Post when authenticated with blog permission', async () => {
    isAuthenticatedSubject$.next(true);
    canWriteBlogSubject$.next(true);
    await fixture.whenStable();
    const newPostButton = (fixture.nativeElement as HTMLElement)
      .querySelector<HTMLAnchorElement>('[data-testid="BlogIndex-NewPostButton"]');

    expect(newPostButton?.getAttribute('href')).toBe('/apps/blog/new');
    expect(newPostButton?.getAttribute('aria-disabled')).toBe('false');
  });

  it('should display categories from the service', async () => {
    const testCategories: Array<CategorySummary> = [
      { slug: 'cat1', name: 'Category 1', postsCount: 5 },
      { slug: 'cat2', name: 'Category 2', postsCount: 3 },
    ];
    categoriesSubject$.next(testCategories);
    await fixture.whenStable();

    const categoriesListView = fixture.debugElement.query(By.directive(CategoriesListViewStubComponent)) as DebugElement | null;
    const categoriesInstance = categoriesListView?.componentInstance as CategoriesListViewStubComponent;

    expect(categoriesInstance).toBeTruthy();
    expect(categoriesInstance.categories()).toEqual(testCategories);
  });

  it('should display error on failure to load categories', async () => {
    categoriesSubject$.error(new HttpErrorResponse({}));
    await fixture.whenStable();

    const categoriesListView = fixture.debugElement.query(By.directive(CategoriesListViewStubComponent)) as DebugElement | null;
    const categoriesInstance = categoriesListView?.componentInstance as CategoriesListViewStubComponent;

    expect(categoriesInstance).toBeTruthy();
    expect(categoriesInstance.error()).toContain('Http failure');
  });
});
