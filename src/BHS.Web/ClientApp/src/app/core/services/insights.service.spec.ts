import { InsightsService } from './insights.service';
import { TestBed } from '@angular/core/testing';

describe('InsightsService', () => {
  let service: InsightsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InsightsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
