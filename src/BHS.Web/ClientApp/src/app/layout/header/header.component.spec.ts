import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { HeaderComponent } from './header.component';
import { of } from 'rxjs';
import { SiteBannerService } from '@data/service/site-banner.service';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;

  beforeEach(waitForAsync(() => {
    const bannerService = jasmine.createSpyObj<SiteBannerService>('bannerService', {'getEnabled': of()});

    TestBed.configureTestingModule({
      declarations: [ HeaderComponent ],
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
