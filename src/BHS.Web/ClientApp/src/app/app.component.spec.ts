import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from '@app/app.component';
import { AuthService } from '@auth0/auth0-angular';
import { InsightsService } from '@core/services/insights.service';
import { of } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;

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

    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the app', () => {
      expect(component).toBeTruthy();
    },
  );

  it('should not show loading indicator', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(element.querySelector('.router-load-indicator'))
      .withContext('no loading')
      .toBeNull();
  });
});
