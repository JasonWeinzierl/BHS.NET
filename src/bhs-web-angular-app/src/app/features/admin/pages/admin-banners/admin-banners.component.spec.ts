/* eslint-disable rxjs-x/finnish */
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MockProvider } from 'ng-mocks';
import { Subject } from 'rxjs';
import AdminBannersComponent from './admin-banners.component';
import { SiteBannerService } from '@data/banners';
import { SiteBannerHistory } from '@data/banners/models/site-banner-history';

describe('AdminBannersComponent', () => {
  let component: AdminBannersComponent;
  let fixture: ComponentFixture<AdminBannersComponent>;
  let bannersSubject$: Subject<Array<SiteBannerHistory>>;

  beforeEach(async () => {
    bannersSubject$ = new Subject();

    await TestBed.configureTestingModule({
      imports: [
        AdminBannersComponent,
      ],
      providers: [
        MockProvider(SiteBannerService, {
          getHistory$: () => bannersSubject$,
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
        statusChanges: [],
      },
      {
        id: 'def',
        lead: 'Test Banner 2',
        body: 'This is a test banner 2',
        theme: 'Warning',
        statusChanges: [],
      },
    ]);
    fixture.detectChanges();

    const itemElements = (fixture.nativeElement as HTMLElement).querySelectorAll('.list-group-item');

    expect(itemElements).toHaveLength(2);
    expect(itemElements[0].textContent).toContain('Test Banner');
  });

  it('should show the history of each banner', () => {
    bannersSubject$.next([
      {
        id: 'abc',
        lead: 'Test Banner',
        body: 'This is a test banner',
        theme: 'Info',
        statusChanges: [
          {
            dateModified: new Date('2023-01-01'),
            isEnabled: true,
          },
          {
            dateModified: new Date('2023-02-01'),
            isEnabled: false,
          },
          {
            dateModified: new Date('2023-03-01'),
            isEnabled: true,
          },
          {
            dateModified: new Date(new Date().getTime() + 1000), // Future, should not get honored.
            isEnabled: false,
          },
        ],
      },
    ]);
    fixture.detectChanges();

    const itemElement = (fixture.nativeElement as HTMLElement).querySelector('.list-group-item');
    const badgeElements = itemElement?.querySelectorAll('span.badge') ?? [];
    const isEnabledCellElements = itemElement?.querySelectorAll('table > tbody > tr > td:nth-child(2)') ?? [];

    expect(badgeElements).toHaveLength(2);
    expect(badgeElements[0].textContent).toContain('Info');
    expect(badgeElements[1].textContent).toContain('Enabled');
    expect(isEnabledCellElements).toHaveLength(4);
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
