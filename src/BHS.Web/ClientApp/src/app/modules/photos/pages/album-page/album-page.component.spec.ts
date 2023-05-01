import { ActivatedRoute, convertToParamMap, RouterLink } from '@angular/router';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AlbumPageComponent } from './album-page.component';
import { AlbumPhotos } from '@data/photos';
import { AlertComponent } from 'ngx-bootstrap/alert';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';
import { PhotosService } from '@data/photos/services/photos.service';
import { RouterTestingModule } from '@angular/router/testing';

const createAlbum = (): AlbumPhotos => ({
  slug: 'album-three',
  name: 'Album Three',
  blogPostSlug: '1-post',
  photos: [{
    id: 'photo-four',
    imagePath: '/assets/img/2017/oldcityhall.jpg',
    datePosted: new Date(),
  }, {
    id: 'photo-five',
    imagePath: '/assets/img/2017/oldcityhall.jpg',
    datePosted: new Date(),
  }, {
    id: 'photo-six',
    imagePath: '/assets/img/2017/oldcityhall.jpg',
    datePosted: new Date(),
  }],
});

describe('AlbumPageComponent', () => {
  let component: AlbumPageComponent;
  let fixture: ComponentFixture<AlbumPageComponent>;

  beforeEach(async () => {
    const photosService = jasmine.createSpyObj<PhotosService>('photosService', {'getAlbum': of(createAlbum())});

    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
      ],
      declarations: [
        AlertComponent,
        AlbumPageComponent,
      ],
      providers: [
        {
          provide: PhotosService,
          useValue: photosService,
        },
        {
          provide: ActivatedRoute,
          useValue: {
            'paramMap': of(convertToParamMap({
              slug: 'album-three',
              id: 'photo-four',
            })),
          },
        },
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(AlbumPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should finish loading', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(element.querySelector('.spinner-grow'))
      .withContext('no loading')
      .toBeNull();
  });

  it('should not show error', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(element.querySelector('.alert-danger')?.textContent)
      .withContext('no error message')
      .toBeUndefined();
  });

  it('should show album name', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(element.textContent)
      .toContain('Album Three');
  });

  it('should render router links', () => {
    const linkDebugElements = fixture.debugElement.queryAll(By.directive(RouterLink));
    const routerLinks = linkDebugElements.map(de => de.injector.get(RouterLink));

    expect(routerLinks.map(l => l.href)).toEqual([
      '/apps/photos/album/album-three/photo/photo-six',
      '/apps/photos/album/album-three',
      '/apps/blog/entry/1-post',
      '/apps/photos/album/album-three/photo/photo-five',
      '/apps/photos/album/album-three/photo/photo-six',
      '/apps/photos/album/album-three',
      '/apps/photos/album/album-three/photo/photo-five',
    ]);
  });
});
