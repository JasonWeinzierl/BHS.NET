import { TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { InsightsService } from './insights.service';
import { APP_ENVIRONMENT } from 'src/environments';

describe('InsightsService', () => {
  let service: InsightsService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterModule],
      providers: [
        {
          provide: APP_ENVIRONMENT,
          useValue: {},
        },
      ],
    });
    service = TestBed.inject(InsightsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
