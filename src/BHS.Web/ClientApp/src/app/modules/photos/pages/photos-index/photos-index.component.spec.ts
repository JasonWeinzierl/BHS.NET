import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MockProvider } from 'ng-mocks';
import { EMPTY } from 'rxjs';
import { PhotosIndexComponent } from './photos-index.component';
import { PhotosService } from '@data/photos';

describe('PhotosIndexComponent', () => {
  let component: PhotosIndexComponent;
  let fixture: ComponentFixture<PhotosIndexComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        PhotosIndexComponent,
      ],
      providers: [
        MockProvider(PhotosService, {
          getAlbums: () => EMPTY,
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(PhotosIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
