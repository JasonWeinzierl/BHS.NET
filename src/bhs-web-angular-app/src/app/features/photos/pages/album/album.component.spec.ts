import { NgOptimizedImage } from '@angular/common';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { MockDirective, MockProvider } from 'ng-mocks';
import { EMPTY, of } from 'rxjs';
import { AlbumComponent } from './album.component';
import { PhotosService } from '@data/photos/services/photos.service';

// TODO: re-enable when ng-mocks supports Angular 19.
describe.skip('AlbumComponent', () => {
  let component: AlbumComponent;
  let fixture: ComponentFixture<AlbumComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        AlbumComponent,
        MockDirective(NgOptimizedImage),
      ],
      providers: [
        MockProvider(PhotosService, {
          getAlbum: () => EMPTY,
        }),
        MockProvider(ActivatedRoute, {
          paramMap: of(convertToParamMap({
            slug: '3',
          })),
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(AlbumComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
