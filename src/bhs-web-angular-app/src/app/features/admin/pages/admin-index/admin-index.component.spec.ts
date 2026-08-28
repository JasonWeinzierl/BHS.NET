import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { MockProvider } from 'ng-mocks';
import { BehaviorSubject, EMPTY } from 'rxjs';
import { PermissionsService } from '@core/services/permissions.service';
import { AdminIndexComponent } from './admin-index.component';

describe('AdminIndexComponent', () => {
  let component: AdminIndexComponent;
  let fixture: ComponentFixture<AdminIndexComponent>;
  let canWriteBlogSubject$: BehaviorSubject<boolean>;
  let canWriteBannersSubject$: BehaviorSubject<boolean>;

  beforeEach(async () => {
    canWriteBlogSubject$ = new BehaviorSubject(false);
    canWriteBannersSubject$ = new BehaviorSubject(false);

    await TestBed.configureTestingModule({
      imports: [
        RouterModule.forRoot([]),
        AdminIndexComponent,
      ],
      providers: [
        MockProvider(AuthService, {
          user$: EMPTY,
          getAccessTokenSilently: () => EMPTY,
        }),
        MockProvider(PermissionsService, {
          hasPermission$: permission => permission === 'write:blog'
            ? canWriteBlogSubject$.asObservable()
            : canWriteBannersSubject$.asObservable(),
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminIndexComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should only enable action links with their permissions', async () => {
    const element = fixture.nativeElement as HTMLElement;
    const blogLink = element.querySelector<HTMLAnchorElement>('[data-testid="AdminIndex-NewBlogPost"]');
    const bannersLink = element.querySelector<HTMLAnchorElement>('[data-testid="AdminIndex-Banners"]');

    expect(blogLink?.getAttribute('href')).toBeNull();
    expect(bannersLink?.getAttribute('href')).toBeNull();

    canWriteBlogSubject$.next(true);
    await fixture.whenStable();

    expect(blogLink?.getAttribute('href')).toBe('/apps/blog/new');
    expect(bannersLink?.getAttribute('href')).toBeNull();

    canWriteBannersSubject$.next(true);
    await fixture.whenStable();

    expect(bannersLink?.getAttribute('href')).toBe('/admin/banners');
  });
});
