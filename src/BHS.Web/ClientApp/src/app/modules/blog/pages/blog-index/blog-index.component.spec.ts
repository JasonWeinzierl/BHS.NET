import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BlogIndexComponent } from './blog-index.component';
import { BlogService } from '@data/blog';
import { Component } from '@angular/core';
import { of } from 'rxjs';

@Component({
  selector: 'app-posts-search',
})
// eslint-disable-next-line @angular-eslint/component-class-suffix
class PostsSearchComponentStub { }

describe('BlogIndexComponent', () => {
  let component: BlogIndexComponent;
  let fixture: ComponentFixture<BlogIndexComponent>;

  beforeEach(async () => {
    const blogService = jasmine.createSpyObj<BlogService>('blogService', { 'getCategories': of() });

    await TestBed.configureTestingModule({
      declarations: [
        BlogIndexComponent,
        PostsSearchComponentStub,
      ],
      providers: [
        {
          provide: BlogService,
          useValue: blogService,
        },
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(BlogIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
