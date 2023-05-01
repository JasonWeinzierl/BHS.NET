import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Directive, Input } from '@angular/core';
import { RouterLinkActiveDirectiveStub, RouterLinkDirectiveStub } from '@app/mock-testing-objects';
import { AuthService } from '@auth0/auth0-angular';
import { HeaderComponent } from './header.component';
import { of } from 'rxjs';
import { SiteBannerService } from '@data/banners';
import { ToastrService } from 'ngx-toastr';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: '[collapse]',
})
// eslint-disable-next-line @angular-eslint/directive-class-suffix
class CollapseDirectiveStub {
  @Input() collapse = true;
  @Input() isAnimated = false;
}

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;

  beforeEach(async () => {
    const bannerService = jasmine.createSpyObj<SiteBannerService>('bannerService', {'getEnabled': of()});
    const toastr = jasmine.createSpyObj<ToastrService>('toastr', ['error']);
    const auth = jasmine.createSpyObj<AuthService>('auth', {}, {'isAuthenticated$': of()});

    await TestBed.configureTestingModule({
      declarations: [ HeaderComponent, RouterLinkDirectiveStub, RouterLinkActiveDirectiveStub, CollapseDirectiveStub ],
      providers: [
        {
          provide: SiteBannerService,
          useValue: bannerService,
        },
        {
          provide: ToastrService,
          useValue: toastr,
        },
        {
          provide: AuthService,
          useValue: auth,
        },
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
