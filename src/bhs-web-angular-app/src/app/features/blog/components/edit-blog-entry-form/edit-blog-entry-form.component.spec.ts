import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { provideRouter, RouterLink } from '@angular/router';
import { EditBlogEntryFormComponent } from './edit-blog-entry-form.component';
import { Author } from '@data/authors';
import { Category, Post } from '@data/blog';

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
        EditBlogEntryFormComponent,
      ],
      providers: [
        provideRouter([]),
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

  it('should default cancelRoute to /apps/blog', () => {
    const routerLinkDe = fixture.debugElement.query(By.directive(RouterLink));
    const routerLink = routerLinkDe.injector.get(RouterLink);

    // eslint-disable-next-line @typescript-eslint/no-deprecated -- only the setter is deprecated
    expect(routerLink.href).toBe('/apps/blog');
  });

  it('should populate cancelRoute with post url', () => {
    const post = createPost();

    fixture.componentRef.setInput('initialPost', post);
    fixture.detectChanges();

    const routerLinkDe = fixture.debugElement.query(By.directive(RouterLink));
    const routerLink = routerLinkDe.injector.get(RouterLink);

    // eslint-disable-next-line @typescript-eslint/no-deprecated
    expect(routerLink.href).toBe(`/apps/blog/entry/${post.slug}`);
  });

  it('should not show warning if author is not changing', () => {
    const dangerEl = (fixture.nativeElement as HTMLElement).querySelector('.form-text.text-danger');

    expect(dangerEl).toBeFalsy();
  });

  it('should show warning when author is changing', () => {
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

    const dangerEl = (fixture.nativeElement as HTMLElement).querySelector('.form-text.text-danger');

    expect(dangerEl?.textContent).toBeTruthy();
  });
});
