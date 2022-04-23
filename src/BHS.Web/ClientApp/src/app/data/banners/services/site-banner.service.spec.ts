import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SiteBannerService } from './site-banner.service';
import { TestBed } from '@angular/core/testing';

describe('SiteBannerService', () => {
  let service: SiteBannerService;

  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [HttpClientTestingModule] });
    service = TestBed.inject(SiteBannerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
