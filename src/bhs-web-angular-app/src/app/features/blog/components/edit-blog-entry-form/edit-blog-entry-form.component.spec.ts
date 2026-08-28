import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { provideRouter, RouterLink } from '@angular/router';
import { MockProvider } from 'ng-mocks';
import { BehaviorSubject } from 'rxjs';
import { PermissionsService } from '@core/services/permissions.service';
import { Author } from '@data/authors';
import { Category, Post } from '@data/blog';
import { EditBlogEntryFormComponent } from './edit-blog-entry-form.component';

const createPost = (): Post => ({
  slug: '1-post',
  title: 'Post',
  contentMarkdown: '# Title',
  datePublished: new Date(),
  dateLastModified: new Date(),
  categories: [] as Array<Category>,
} as Post);

describe('EditBlogEntryFormComponent', () => {
  let component: EditBlogEntryFormComponent;
  let fixture: ComponentFixture<EditBlogEntryFormComponent>;
  let canWriteBlogSubject$: BehaviorSubject<boolean>;

  beforeEach(async () => {
    canWriteBlogSubject$ = new BehaviorSubject(false);

    await TestBed.configureTestingModule({
      imports: [
        EditBlogEntryFormComponent,
      ],
      providers: [
        provideRouter([]),
        MockProvider(PermissionsService, {
          hasPermission$: () => canWriteBlogSubject$.asObservable(),
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditBlogEntryFormComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should only enable Publish with blog permission', async () => {
    const publishButton = (fixture.nativeElement as HTMLElement)
      .querySelector<HTMLButtonElement>('button[type="submit"]');

    expect(publishButton?.disabled).toBe(true);

    canWriteBlogSubject$.next(true);
    await fixture.whenStable();

    expect(publishButton?.disabled).toBe(false);
  });

  it('should default cancelRoute to /apps/blog', () => {
    const routerLinkDe = fixture.debugElement.query(By.directive(RouterLink));
    const routerLink = routerLinkDe.injector.get(RouterLink);

    // eslint-disable-next-line @typescript-eslint/no-deprecated -- only the setter is deprecated
    expect(routerLink.href).toBe('/apps/blog');
  });

  it('should populate cancelRoute with post url', async () => {
    const post = createPost();

    fixture.componentRef.setInput('initialPost', post);
    await fixture.whenStable();

    const routerLinkDe = fixture.debugElement.query(By.directive(RouterLink));
    const routerLink = routerLinkDe.injector.get(RouterLink);

    // eslint-disable-next-line @typescript-eslint/no-deprecated
    expect(routerLink.href).toBe(`/apps/blog/entry/${post.slug}`);
  });

  it('should not show warning if author is not changing', () => {
    const dangerElement = (fixture.nativeElement as HTMLElement).querySelector('.form-text.text-danger');

    expect(dangerElement).toBeFalsy();
  });

  it('should show warning when author is changing', async () => {
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
    await fixture.whenStable();

    const dangerElement = (fixture.nativeElement as HTMLElement).querySelector('.alert.alert-error');

    expect(dangerElement?.textContent).toBeTruthy();
  });
});
