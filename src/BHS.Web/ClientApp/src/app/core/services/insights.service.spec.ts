import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { InsightsService } from './insights.service';
import { AppEnvironment } from 'src/environments';

describe('InsightsService', () => {
  let service: InsightsService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        {
          provide: AppEnvironment,
          useValue: new AppEnvironment(),
        },
      ],
    });
    service = TestBed.inject(InsightsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
