import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EMPTY } from 'rxjs';
import { PhotosIndexComponent } from './photos-index.component';
import { PhotosService } from '@data/photos';
import { MockProvider } from 'ng-mocks';

describe('PhotosIndexComponent', () => {
  let component: PhotosIndexComponent;
  let fixture: ComponentFixture<PhotosIndexComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
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
