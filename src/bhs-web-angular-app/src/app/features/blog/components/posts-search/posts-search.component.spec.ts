import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MockPipe, MockProvider } from 'ng-mocks';
import { PostsSearchComponent } from './posts-search.component';
import { BlogService } from '@data/blog';
import { SortByPipe } from '@shared/pipes/sort-by.pipe';

// TODO: re-enable when ng-mocks supports Angular 19.
describe.skip('PostsSearchComponent', () => {
  let component: PostsSearchComponent;
  let fixture: ComponentFixture<PostsSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        PostsSearchComponent,
        MockPipe(SortByPipe),
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
