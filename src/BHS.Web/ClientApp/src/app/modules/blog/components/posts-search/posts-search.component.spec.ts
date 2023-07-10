import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Pipe, PipeTransform } from '@angular/core';
import { BlogService } from '@data/blog';
import { FormsModule } from '@angular/forms';
import { MockProvider } from 'ng-mocks';
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
    await TestBed.configureTestingModule({
      imports: [
        FormsModule,
      ],
      declarations: [
        PostsSearchComponent,
        MockSortByPipe,
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
