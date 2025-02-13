import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MockProvider } from 'ng-mocks';
import { Subject } from 'rxjs';
import AdminBannersComponent from './admin-banners.component';
import { SiteBanner, SiteBannerService } from '@data/banners';

describe('AdminBannersComponent', () => {
  let component: AdminBannersComponent;
  let fixture: ComponentFixture<AdminBannersComponent>;
  let bannersSubject$: Subject<Array<SiteBanner>>;

  beforeEach(async () => {
    bannersSubject$ = new Subject();

    await TestBed.configureTestingModule({
      imports: [
        AdminBannersComponent,
      ],
      providers: [
        MockProvider(SiteBannerService, {
          getEnabled$: () => bannersSubject$,
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminBannersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show loading before data is loaded', () => {
    const loadingElement = (fixture.nativeElement as HTMLElement).querySelector('.spinner-grow');

    expect(loadingElement).toBeTruthy();
    expect(loadingElement?.textContent).toContain('Loading...');
  });

  it('should show banners after data is loaded', () => {
    bannersSubject$.next([
      {
        id: 'abc',
        lead: 'Test Banner',
        body: 'This is a test banner',
        theme: 'Info',
      },
      {
        id: 'def',
        lead: 'Test Banner 2',
        body: 'This is a test banner 2',
        theme: 'Warning',
      },
    ]);
    fixture.detectChanges();

    const itemElements = (fixture.nativeElement as HTMLElement).querySelectorAll('.list-group-item');

    expect(itemElements).toHaveLength(2);
    expect(itemElements[0].textContent).toContain('Test Banner');
  });

  it('should show no banners after data is loaded', () => {
    bannersSubject$.next([]);
    fixture.detectChanges();

    const itemElements = (fixture.nativeElement as HTMLElement).querySelectorAll('.list-group-item');

    expect(itemElements).toHaveLength(1);
    expect(itemElements[0].textContent).toContain('No banners are enabled.');
  });

  it('should show error message on error', () => {
    bannersSubject$.error(new Error('test error'));
    fixture.detectChanges();

    const errorElement = (fixture.nativeElement as HTMLElement).querySelector('.text-danger');

    expect(errorElement).toBeTruthy();
    expect(errorElement?.textContent).toContain('Failed to load banners.');
  });
});
