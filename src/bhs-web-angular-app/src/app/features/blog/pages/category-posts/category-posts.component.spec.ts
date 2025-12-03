import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { EMPTY, of } from 'rxjs';
import { CategoryPostsComponent } from './category-posts.component';
import { BlogService } from '@data/blog';
import { MockProvider } from 'ng-mocks';

describe('CategoryPostsComponent', () => {
  let component: CategoryPostsComponent;
  let fixture: ComponentFixture<CategoryPostsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        CategoryPostsComponent,
      ],
      providers: [
        MockProvider(BlogService, {
          getCategory: () => EMPTY,
        }),
        MockProvider(ActivatedRoute, {
          paramMap: of(convertToParamMap({
            slug: '123',
          })),
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryPostsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
