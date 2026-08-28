import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { MockProvider } from 'ng-mocks';
import { BehaviorSubject } from 'rxjs';
import { vi } from 'vitest';
import { PermissionsService } from '@core/services/permissions.service';
import { SiteBannerService } from '@data/banners';
import AdminBannerCreateComponent from './admin-banner-create.component';

describe('AdminBannerCreateComponent', () => {
  let component: AdminBannerCreateComponent;
  let fixture: ComponentFixture<AdminBannerCreateComponent>;
  let router: Router;
  let canWriteBannersSubject$: BehaviorSubject<boolean>;

  beforeEach(async () => {
    canWriteBannersSubject$ = new BehaviorSubject(false);

    await TestBed.configureTestingModule({
      imports: [
        AdminBannerCreateComponent,
      ],
      providers: [
        provideRouter([]),
        { provide: SiteBannerService, useValue: {

          createBanner$: vi.fn(),
        } },
        MockProvider(PermissionsService, {
          hasPermission$: () => canWriteBannersSubject$.asObservable(),
        }),
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(AdminBannerCreateComponent);
    component = fixture.componentInstance;

    router = TestBed.inject(Router);
    vi.spyOn(router, 'navigate').mockResolvedValue(true);

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show success alert when successSignal is true', async () => {
    component.successSignal.set(true);
    await fixture.whenStable();

    const compiled = fixture.nativeElement as HTMLElement;
    const successAlert = compiled.querySelector('.alert-success');

    expect(successAlert).toBeTruthy();
    expect(successAlert?.textContent).toContain('Banner created successfully! Redirecting...');
  });

  it('should show error alert when errorSignal has value', async () => {
    component.errorSignal.set('Test error message');
    await fixture.whenStable();

    const compiled = fixture.nativeElement as HTMLElement;
    const errorAlert = compiled.querySelector('.alert-error');

    expect(errorAlert).toBeTruthy();
    expect(errorAlert?.textContent).toContain('Test error message');
  });

  it('should initially disable submit button', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    const submitButton = compiled.querySelector<HTMLButtonElement>('button[type="submit"]');

    expect(submitButton?.disabled).toBe(true);
  });

  it('should only enable valid submission with banner permission', async () => {
    component.bannerModel.update(model => ({ ...model, lead: 'Test banner' }));
    await fixture.whenStable();
    const submitButton = (fixture.nativeElement as HTMLElement)
      .querySelector<HTMLButtonElement>('button[type="submit"]');

    expect(submitButton?.disabled).toBe(true);

    canWriteBannersSubject$.next(true);
    await fixture.whenStable();

    expect(submitButton?.disabled).toBe(false);
  });
});
