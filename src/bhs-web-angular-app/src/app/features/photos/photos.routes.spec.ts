import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { RouterTestingHarness } from '@angular/router/testing';
import { MockProvider } from 'ng-mocks';
import { of } from 'rxjs';
import { AlbumPhotos, PhotosService } from '@data/photos';
import { AlbumComponent } from './pages/album/album.component';
import photoRoutes from './photos.routes';

const album: AlbumPhotos = {
  slug: 'album-three',
  name: 'Album Three',
  photos: [{
    id: 'photo-four',
    imagePath: '/assets/img/2017/oldcityhall.jpg',
    datePosted: new Date(),
  }, {
    id: 'photo-five',
    imagePath: '/assets/img/2017/oldcityhall.jpg',
    datePosted: new Date(),
  }],
} as AlbumPhotos;

describe('photo routes', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideRouter([{
          path: 'apps/photos',
          children: photoRoutes,
        }]),
        MockProvider(PhotosService, {
          getAlbum$: () => of(album),
        }),
      ],
    });
  });

  it('should keep the album mounted while navigating photo URLs', async () => {
    const harness = await RouterTestingHarness.create();
    const albumComponent = await harness.navigateByUrl(
      '/apps/photos/album/album-three/photo/photo-four',
      AlbumComponent,
    );
    await harness.fixture.whenStable();

    expect(albumComponent.previewSignal()?.currentPhoto.id).toBe('photo-four');
    expect(harness.routeNativeElement?.querySelector('dialog.modal[open]')).toBeTruthy();

    const nextAlbumComponent = await harness.navigateByUrl(
      '/apps/photos/album/album-three/photo/photo-five',
      AlbumComponent,
    );
    await harness.fixture.whenStable();

    expect(nextAlbumComponent).toBe(albumComponent);
    expect(albumComponent.previewSignal()?.currentPhoto.id).toBe('photo-five');

    const closedAlbumComponent = await harness.navigateByUrl(
      '/apps/photos/album/album-three',
      AlbumComponent,
    );

    expect(closedAlbumComponent).toBe(albumComponent);
    expect(albumComponent.previewSignal()).toBeUndefined();
  });
});
