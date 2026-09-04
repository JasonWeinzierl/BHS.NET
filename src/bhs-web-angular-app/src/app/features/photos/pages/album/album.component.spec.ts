import { NgOptimizedImage } from '@angular/common';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, ActivatedRouteSnapshot, convertToParamMap, RouterLink } from '@angular/router';
import { MockProvider } from 'ng-mocks';
import { of } from 'rxjs';
import { AlbumPhotos } from '@data/photos';
import { PhotosService } from '@data/photos/services/photos.service';
import { SnippetPipe } from '@shared/pipes/snippet.pipe';
import { AlbumPageComponent } from '../album-page/album-page.component';
import { AlbumComponent } from './album.component';

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
  }, {
    id: 'photo-six',
    imagePath: '/assets/img/2017/oldcityhall.jpg',
    datePosted: new Date(),
  }],
} as AlbumPhotos;

describe('AlbumComponent', () => {
  let component: AlbumComponent;
  let fixture: ComponentFixture<AlbumComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        AlbumComponent,
      ],
      providers: [
        MockProvider(PhotosService, {
          getAlbum$: () => of(album),
        }),
        MockProvider(ActivatedRoute, {
          paramMap: of(convertToParamMap({
            slug: album.slug,
          })),
          snapshot: {
            paramMap: convertToParamMap({
              slug: album.slug,
            }),
          } as ActivatedRouteSnapshot,
          firstChild: {
            snapshot: {
              paramMap: convertToParamMap({
                id: 'photo-four',
              }),
            } as ActivatedRouteSnapshot,
          } as ActivatedRoute,
        }),
      ],
    })
    .overrideComponent(AlbumComponent, {
      set: {
        imports: [
          RouterLink,
          NgOptimizedImage,
          SnippetPipe,
          AlbumPageComponent,
        ],
      },
    })
    .compileComponents();

    fixture = TestBed.createComponent(AlbumComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should derive the deep-linked photo preview', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(component.previewSignal()).toEqual({
      currentPhoto: album.photos[0],
      previousPhotoId: 'photo-six',
      nextPhotoId: 'photo-five',
    });
    expect(element.querySelector('app-album-page')).toBeTruthy();
  });
});
