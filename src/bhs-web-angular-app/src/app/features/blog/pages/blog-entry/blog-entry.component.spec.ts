import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap, RouterModule } from '@angular/router';
import { ActiveToast, ToastrService } from '@openng/ngx-toastr';
import { MockProvider } from 'ng-mocks';
import { BehaviorSubject, of, throwError } from 'rxjs';
import { PermissionsService } from '@core/services/permissions.service';
import { BlogService } from '@data/blog';
import { PhotosService } from '@data/photos';
import { EntryAlbumComponent } from '@features/blog/components/entry-album/entry-album.component';
import { DateComponent } from '@shared/components/date/date.component';
import { BlogEntryComponent } from './blog-entry.component';

describe('BlogEntryComponent', () => {
  let component: BlogEntryComponent;
  let fixture: ComponentFixture<BlogEntryComponent>;
  let isAuthenticatedSubject$: BehaviorSubject<boolean>;
  let canWriteBlogSubject$: BehaviorSubject<boolean>;

  beforeEach(async () => {
    isAuthenticatedSubject$ = new BehaviorSubject(false);
    canWriteBlogSubject$ = new BehaviorSubject(false);

    await TestBed.configureTestingModule({
      imports: [
        RouterModule,
        DateComponent,
        BlogEntryComponent,
        EntryAlbumComponent,
      ],
      providers: [
        MockProvider(ActivatedRoute, {
          paramMap: of(convertToParamMap({
            slug: '123',
          })),
        }),
        MockProvider(BlogService, {
          getPost$: () => of({
            slug: '1-test',
            title: 'Hello!',
            contentMarkdown: '## Foo',
            photosAlbumSlug: 'does-not-exist',
            // eslint-disable-next-line unicorn/no-null
            filePath: null,
            // eslint-disable-next-line unicorn/no-null
            author: null,
            datePublished: new Date(),
            dateLastModified: new Date(),
            categories: [{ slug: 'newsletters', name: 'Newsletters' }],
          }),
        }),
        MockProvider(PhotosService, {
          getAlbum$: () => throwError(() => new Error('test 404 not found')),
        }),
        MockProvider(PermissionsService, {
          isAuthenticated$: isAuthenticatedSubject$.asObservable(),
          hasPermission$: () => canWriteBlogSubject$.asObservable(),
        }),
        MockProvider(ToastrService, {
          error: () => ({}) as ActiveToast<unknown>,
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(BlogEntryComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should not error if album load fails', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(element.querySelector('h1')?.textContent).toBe('Hello!'); // shows post title
  });

  it('should disable edit when authenticated without blog permission', async () => {
    isAuthenticatedSubject$.next(true);
    await fixture.whenStable();
    const editLink = (fixture.nativeElement as HTMLElement)
      .querySelector<HTMLAnchorElement>('[data-testid="BlogEntry-EditPost"]');

    expect(editLink).not.toBeNull();
    expect(editLink?.getAttribute('href')).toBeNull();
    expect(editLink?.getAttribute('aria-disabled')).toBe('true');
  });

  it('should enable edit when authenticated with blog permission', async () => {
    isAuthenticatedSubject$.next(true);
    canWriteBlogSubject$.next(true);
    await fixture.whenStable();
    const editLink = (fixture.nativeElement as HTMLElement)
      .querySelector<HTMLAnchorElement>('[data-testid="BlogEntry-EditPost"]');

    expect(editLink?.getAttribute('href')).toBe('/apps/blog/edit/1-test');
    expect(editLink?.getAttribute('aria-disabled')).toBe('false');
  });
});
