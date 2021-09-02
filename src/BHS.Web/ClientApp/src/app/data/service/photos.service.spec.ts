import { HttpClientTestingModule } from '@angular/common/http/testing';
import { PhotosService } from './photos.service';
import { TestBed } from '@angular/core/testing';

describe('PhotosService', () => {
  let service: PhotosService;

  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [HttpClientTestingModule] });
    service = TestBed.inject(PhotosService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
