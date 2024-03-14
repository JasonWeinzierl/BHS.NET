import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { MockDirective, MockProvider } from 'ng-mocks';
import { CollapseDirective } from 'ngx-bootstrap/collapse';
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
        RouterModule.forRoot([]),
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
