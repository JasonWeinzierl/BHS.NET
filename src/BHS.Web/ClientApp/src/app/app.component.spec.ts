import { AppComponent } from '@app/app.component';
import { AuthService } from '@auth0/auth0-angular';
import { InsightsService } from '@core/services/insights.service';
import { of } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';
import { TestBed } from '@angular/core/testing';

describe('AppComponent', () => {
  beforeEach(async () => {
    const insightsService = jasmine.createSpyObj<InsightsService>('insightsService', ['init']);
    const auth = jasmine.createSpyObj<AuthService>('auth', {}, {'isLoading$': of()});

    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
      ],
      declarations: [
        AppComponent,
      ],
      providers: [
        {
          provide: InsightsService,
          useValue: insightsService,
        },
        {
          provide: AuthService,
          useValue: auth,
        },
      ],
    })
    .compileComponents();
  });

  it('should create the app', () => {
      const fixture = TestBed.createComponent(AppComponent);
      const app = fixture.componentInstance;

      expect(app).toBeTruthy();
    },
  );

  // it('should render title in an h1 tag', () => {
  //     const fixture = TestBed.createComponent(AppComponent);
  //     fixture.detectChanges();
  //     const compiled = fixture.nativeElement;
  //     expect(compiled.querySelector('h1').textContent).toContain(
  //       'Welcome!'
  //     );
  //   }
  // );
});
