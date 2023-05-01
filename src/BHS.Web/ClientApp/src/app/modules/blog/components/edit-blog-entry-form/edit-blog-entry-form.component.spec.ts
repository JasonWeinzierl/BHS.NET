import { Category, Post } from '@data/blog';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Author } from '@data/authors';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { EditBlogEntryFormComponent } from './edit-blog-entry-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';

const createPost = (): Post => ({
  slug: '1-post',
  title: 'Post',
  contentMarkdown: '# Title',
  filePath: null,
  photosAlbumSlug: null,
  author: null,
  datePublished: new Date(),
  dateLastModified: new Date(),
  categories: [],
});

describe('EditBlogEntryFormComponent', () => {
  let component: EditBlogEntryFormComponent;
  let fixture: ComponentFixture<EditBlogEntryFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        RouterTestingModule,
      ],
      declarations: [
        BsDatepickerDirective,
        EditBlogEntryFormComponent,
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditBlogEntryFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should populate cancelRoute with post url', () => {
    expect(component.cancelRoute)
      .withContext('default at first')
      .toEqual(['/apps/blog']);

    const post = createPost();

    fixture.componentRef.setInput('initialPost', post);
    fixture.detectChanges();

    expect(component.cancelRoute)
      .toEqual(['/apps/blog/entry', post.slug]);
  });

  it('should show warning when author is changing', () => {
    expect(component.authorWarning)
      .withContext('undefined at first')
      .toBeUndefined();

    const author: Author = {
      username: 'me',
      name: 'Me',
    };
    const categories: Array<Category> = [{
      slug: 'stories',
      name: 'Stories',
    }];

    fixture.componentRef.setInput('currentAuthor', author);
    fixture.componentRef.setInput('initialPost', createPost());
    fixture.componentRef.setInput('allCategories', categories);
    fixture.detectChanges();

    expect(component.authorWarning).toBeTruthy();
  });
});
