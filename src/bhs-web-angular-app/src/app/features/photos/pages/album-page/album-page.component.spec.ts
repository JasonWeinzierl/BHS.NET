import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { provideRouter, Router, RouterLink } from '@angular/router';
import { AlbumPhotos } from '@data/photos';
import { AlbumPageComponent } from './album-page.component';

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
} as AlbumPhotos);

describe('AlbumPageComponent', () => {
  let component: AlbumPageComponent;
  let fixture: ComponentFixture<AlbumPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        AlbumPageComponent,
      ],
      providers: [
        provideRouter([]),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(AlbumPageComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('album', createAlbum());
    fixture.componentRef.setInput('currentPhoto', createAlbum().photos[0]);
    fixture.componentRef.setInput('previousPhotoId', 'photo-six');
    fixture.componentRef.setInput('nextPhotoId', 'photo-five');
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render a fullscreen modal', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(element.querySelector('dialog.modal[open]')).toBeTruthy();
    expect(element.querySelector('.modal-box')?.classList).toContain('h-dvh');
  });

  it('should show album name', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(element.textContent).toContain('Album Three');
  });

  it('should render router links', () => {
    const linkDebugElements = fixture.debugElement.queryAll(By.directive(RouterLink));
    const routerLinks = linkDebugElements.map(de => de.injector.get(RouterLink));

    // eslint-disable-next-line @typescript-eslint/no-deprecated -- only the setter is deprecated
    expect(routerLinks.map(link => link.href)).toEqual([
      '/apps/photos/album/album-three',
      '/apps/photos/album/album-three/photo/photo-six',
      '/apps/photos/album/album-three/photo/photo-five',
    ]);
  });

  it('should navigate for keyboard shortcuts', () => {
    const router = TestBed.inject(Router);
    const navigateSpy = vi.spyOn(router, 'navigate').mockResolvedValue(true);

    component.onPreviousPhotoKeyboardShortcut();
    component.onNextPhotoKeyboardShortcut();
    component.onCloseKeyboardShortcut();

    expect(navigateSpy).toHaveBeenNthCalledWith(1, ['/apps/photos/album', 'album-three', 'photo', 'photo-six']);
    expect(navigateSpy).toHaveBeenNthCalledWith(2, ['/apps/photos/album', 'album-three', 'photo', 'photo-five']);
    expect(navigateSpy).toHaveBeenNthCalledWith(3, ['/apps/photos/album', 'album-three']);
  });
});
