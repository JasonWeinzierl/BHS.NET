import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Directive , Input } from '@angular/core';
import { of, throwError } from 'rxjs';
import { AuthService } from '@auth0/auth0-angular';
import { BlogEntryComponent } from './blog-entry.component';
import { BlogService } from '@data/blog';
import { DateComponent } from '@shared/components/date/date.component';
import { EntryAlbumComponent } from '@modules/blog/components/entry-album/entry-album.component';
import { PhotosService } from '@data/photos';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrService } from 'ngx-toastr';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'markdown',
})
// eslint-disable-next-line @angular-eslint/directive-class-suffix
class MarkdownDirectiveStub {
  @Input() data = '';
}

describe('BlogEntryComponent', () => {
  let component: BlogEntryComponent;
  let fixture: ComponentFixture<BlogEntryComponent>;

  let blogService: jasmine.SpyObj<BlogService>;
  let photosService: jasmine.SpyObj<PhotosService>;

  beforeEach(async () => {
    blogService = jasmine.createSpyObj<BlogService>('blogService', {
      'getPost': of({
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
    });
    photosService = jasmine.createSpyObj<PhotosService>('photosService', {
      'getAlbum': throwError(() => new Error('test 404 not found')),
    });
    const auth = jasmine.createSpyObj<AuthService>('auth', {}, {'isAuthenticated$': of(false)});
    const toastr = jasmine.createSpyObj<ToastrService>('toastr', ['error']);

    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
      ],
      declarations: [
        DateComponent,
        BlogEntryComponent,
        EntryAlbumComponent,
        MarkdownDirectiveStub,
      ],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            'paramMap': of(convertToParamMap({
              slug: '123',
            })),
          },
        },
        {
          provide: BlogService,
          useValue: blogService,
        },
        {
          provide: PhotosService,
          useValue: photosService,
        },
        {
          provide: AuthService,
          useValue: auth,
        },
        {
          provide: ToastrService,
          useValue: toastr,
        },
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

    expect(element.querySelector('h1')?.textContent)
      .withContext('shows post title')
      .toBe('Hello!');
  });
});
