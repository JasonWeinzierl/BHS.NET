import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { vi } from 'vitest';
import AdminBannerCreateComponent from './admin-banner-create.component';
import { SiteBannerService } from '@data/banners';

describe('AdminBannerCreateComponent', () => {
  let component: AdminBannerCreateComponent;
  let fixture: ComponentFixture<AdminBannerCreateComponent>;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        AdminBannerCreateComponent,
      ],
      providers: [
        provideRouter([]),
        { provide: SiteBannerService, useValue: {
          // eslint-disable-next-line rxjs-x/finnish
          createBanner$: vi.fn(),
        } },
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
});
