import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BlogService } from '@data/blog';
import { of } from 'rxjs';
import { PostsSearchComponent } from './posts-search.component';

describe('PostsSearchComponent', () => {
  let component: PostsSearchComponent;
  let fixture: ComponentFixture<PostsSearchComponent>;

  beforeEach(async () => {
    const blogService = jasmine.createSpyObj<BlogService>('blogService', { 'searchPosts': of() });

    await TestBed.configureTestingModule({
      declarations: [ PostsSearchComponent ],
      providers: [
        {
          provide: BlogService,
          useValue: blogService,
        },
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
