import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { MockProvider } from 'ng-mocks';
import { ToastrService } from 'ngx-toastr';
import { EMPTY } from 'rxjs';
import { HeaderComponent } from './header.component';
import { SiteBannerService } from '@data/banners';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HeaderComponent,
      ],
      providers: [
        // eslint-disable-next-line @typescript-eslint/no-deprecated -- Needed by ngx-toastr for now.
        provideNoopAnimations(),
        provideRouter([]),
        MockProvider(SiteBannerService, {
          getEnabled$: () => EMPTY,
        }),
        MockProvider(ToastrService),
        MockProvider(AuthService),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
