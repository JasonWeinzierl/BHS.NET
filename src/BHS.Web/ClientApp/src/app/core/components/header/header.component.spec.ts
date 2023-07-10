import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MockDirective, MockProvider } from 'ng-mocks';
import { AuthService } from '@auth0/auth0-angular';
import { CollapseDirective } from 'ngx-bootstrap/collapse';
import { EMPTY } from 'rxjs';
import { HeaderComponent } from './header.component';
import { RouterTestingModule } from '@angular/router/testing';
import { SiteBannerService } from '@data/banners';
import { ToastrService } from 'ngx-toastr';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
      ],
      declarations: [
        HeaderComponent,
        MockDirective(CollapseDirective),
      ],
      providers: [
        MockProvider(SiteBannerService, {
          getEnabled: () => EMPTY,
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
