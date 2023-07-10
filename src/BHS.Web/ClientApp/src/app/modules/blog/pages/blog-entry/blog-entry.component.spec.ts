import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MockComponent, MockProvider } from 'ng-mocks';
import { of, throwError } from 'rxjs';
import { AuthService } from '@auth0/auth0-angular';
import { BlogEntryComponent } from './blog-entry.component';
import { BlogService } from '@data/blog';
import { DateComponent } from '@shared/components/date/date.component';
import { EntryAlbumComponent } from '@modules/blog/components/entry-album/entry-album.component';
import { MarkdownComponent } from 'ngx-markdown';
import { PhotosService } from '@data/photos';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrService } from 'ngx-toastr';

describe('BlogEntryComponent', () => {
  let component: BlogEntryComponent;
  let fixture: ComponentFixture<BlogEntryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
      ],
      declarations: [
        DateComponent,
        BlogEntryComponent,
        EntryAlbumComponent,
        MockComponent(MarkdownComponent),
      ],
      providers: [
        MockProvider(ActivatedRoute, {
          'paramMap': of(convertToParamMap({
            slug: '123',
          })),
        }),
        MockProvider(BlogService, {
          getPost: () => of({
            slug: '1-test',
            title: 'Hello!',
            contentMarkdown: '## Foo',
            photosAlbumSlug: 'does-not-exist',
            filePath: null,
            author: null,
            datePublished: new Date(),
            dateLastModified: new Date(),
            categories: [{ slug: 'newsletters', name: 'Newsletters' }],
          }),
        }),
        MockProvider(PhotosService, {
          getAlbum: () => throwError(() => new Error('test 404 not found')),
        }),
        MockProvider(AuthService, {
          isAuthenticated$: of(false),
        }),
        MockProvider(ToastrService),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(BlogEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should not error if album load fails', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(element.querySelector('h1')?.textContent).toBe('Hello!'); // shows post title
  });
});
