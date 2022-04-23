import { HttpClientTestingModule } from '@angular/common/http/testing';
import { LeadershipService } from './leadership.service';
import { TestBed } from '@angular/core/testing';

describe('LeadershipService', () => {
  let service: LeadershipService;

  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [HttpClientTestingModule] });
    service = TestBed.inject(LeadershipService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
