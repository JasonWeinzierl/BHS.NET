import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MockProvider } from 'ng-mocks';
import { PostsSearchComponent } from './posts-search.component';
import { BlogService } from '@data/blog';

describe('PostsSearchComponent', () => {
  let component: PostsSearchComponent;
  let fixture: ComponentFixture<PostsSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        PostsSearchComponent,
      ],
      providers: [
        MockProvider(BlogService),
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
