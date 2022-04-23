import { PathResolveService } from './path-resolve.service';
import { TestBed } from '@angular/core/testing';

describe('PathResolveService', () => {
  let service: PathResolveService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PathResolveService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
