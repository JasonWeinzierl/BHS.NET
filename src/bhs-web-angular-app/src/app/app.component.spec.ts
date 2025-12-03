import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { of } from 'rxjs';
import { AppComponent } from '@app/app.component';
import { InsightsService } from '@core/services/insights.service';
import { MockProvider } from 'ng-mocks';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterModule.forRoot([]),
        AppComponent,
      ],
      providers: [
        MockProvider(InsightsService, {
          init: () => { /* empty */ },
        }),
        MockProvider(AuthService, {
          isLoading$: of(false),
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it('should not show loading indicator', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(element.querySelector('.router-load-indicator')).toBeNull();
  });
});
