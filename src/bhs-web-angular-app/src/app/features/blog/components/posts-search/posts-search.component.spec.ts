import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EMPTY } from 'rxjs';
import { PostsSearchComponent } from './posts-search.component';
import { BlogService } from '@data/blog';
import { MockProvider } from 'ng-mocks';

describe('PostsSearchComponent', () => {
  let component: PostsSearchComponent;
  let fixture: ComponentFixture<PostsSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        PostsSearchComponent,
      ],
      providers: [
        MockProvider(BlogService, {
          searchPosts$: () => EMPTY,
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(PostsSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
