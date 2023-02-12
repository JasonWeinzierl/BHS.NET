import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { Directive, Input } from '@angular/core';
import { RouterLinkActiveDirectiveStub, RouterLinkDirectiveStub } from '@app/mock-testing-objects';
import { HeaderComponent } from './header.component';
import { of } from 'rxjs';
import { SiteBannerService } from '@data/banners/services/site-banner.service';

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

  beforeEach(waitForAsync(() => {
    const bannerService = jasmine.createSpyObj<SiteBannerService>('bannerService', {'getEnabled': of()});

    TestBed.configureTestingModule({
      declarations: [ HeaderComponent, RouterLinkDirectiveStub, RouterLinkActiveDirectiveStub, CollapseDirectiveStub ],
      providers: [
        {
          provide: SiteBannerService,
          useValue: bannerService,
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
