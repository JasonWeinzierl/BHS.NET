import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { Pipe, PipeTransform } from '@angular/core';
import { BlogIndexComponent } from './blog-index.component';
import { BlogService } from '@data/blog';
import { HttpClientTestingModule } from '@angular/common/http/testing';

@Pipe({name: 'sortBy'})
class MockSortByPipe implements PipeTransform {
  transform(value: any): any {
    return value;
  }
}

describe('BlogIndexComponent', () => {
  let component: BlogIndexComponent;
  let fixture: ComponentFixture<BlogIndexComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      declarations: [
        BlogIndexComponent,
        MockSortByPipe,
      ],
      providers: [
        BlogService,
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BlogIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});