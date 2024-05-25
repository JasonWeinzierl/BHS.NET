import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { MockProvider } from 'ng-mocks';
import { AppComponent } from '@app/app.component';
import { InsightsService } from '@core/services/insights.service';

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
        MockProvider(InsightsService),
        MockProvider(AuthService),
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
