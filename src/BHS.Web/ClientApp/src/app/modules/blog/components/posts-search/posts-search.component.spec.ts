import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Pipe, PipeTransform } from '@angular/core';
import { BlogService } from '@data/blog';
import { FormsModule } from '@angular/forms';
import { of } from 'rxjs';
import { PostsSearchComponent } from './posts-search.component';

@Pipe({name: 'sortBy'})
class MockSortByPipe implements PipeTransform {
  transform(value: unknown): unknown {
    return value;
  }
}

describe('PostsSearchComponent', () => {
  let component: PostsSearchComponent;
  let fixture: ComponentFixture<PostsSearchComponent>;

  beforeEach(async () => {
    const blogService = jasmine.createSpyObj<BlogService>('blogService', { 'searchPosts': of() });

    await TestBed.configureTestingModule({
      imports: [
        FormsModule,
      ],
      declarations: [
        PostsSearchComponent,
        MockSortByPipe,
      ],
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
