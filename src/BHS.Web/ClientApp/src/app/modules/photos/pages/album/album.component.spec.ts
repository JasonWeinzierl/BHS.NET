import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { MockProvider } from 'ng-mocks';
import { EMPTY, of } from 'rxjs';
import { AlbumComponent } from './album.component';
import { PhotosService } from '@data/photos/services/photos.service';

describe('AlbumComponent', () => {
  let component: AlbumComponent;
  let fixture: ComponentFixture<AlbumComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        AlbumComponent,
      ],
      providers: [
        MockProvider(PhotosService, {
          getAlbum: () => EMPTY,
        }),
        MockProvider(ActivatedRoute, {
          'paramMap': of(convertToParamMap({
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
