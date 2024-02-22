import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { SiteBannerService } from './site-banner.service';

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
