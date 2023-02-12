import { TestBed } from '@angular/core/testing';

import { InsightsService } from './insights.service';

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
