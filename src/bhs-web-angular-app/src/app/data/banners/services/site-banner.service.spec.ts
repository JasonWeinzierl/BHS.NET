import { provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { SiteBannerService } from './site-banner.service';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

describe('SiteBannerService', () => {
  let service: SiteBannerService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [],
      providers: [
        provideHttpClient(withInterceptorsFromDi()),
        provideHttpClientTesting(),
      ],
    });
    service = TestBed.inject(SiteBannerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
