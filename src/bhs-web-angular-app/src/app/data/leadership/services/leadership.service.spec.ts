import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { LeadershipService } from './leadership.service';

describe('LeadershipService', () => {
  let service: LeadershipService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [],
      providers: [
        provideHttpClient(withInterceptorsFromDi()),
        provideHttpClientTesting(),
      ],
    });
    service = TestBed.inject(LeadershipService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
