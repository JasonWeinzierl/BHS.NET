import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { MockProvider } from 'ng-mocks';
import { of } from 'rxjs';
import { PermissionsService } from '@core/services/permissions.service';
import { LeadershipService } from '@data/leadership/services/leadership.service';
import { LeadershipEditorComponent } from '../../components/leadership-editor/leadership-editor.component';
import OrganizationComponent from './organization.component';

describe('OrganizationComponent', () => {
  let fixture: ComponentFixture<OrganizationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrganizationComponent],
      providers: [
        MockProvider(LeadershipService, {
          getDirectors$: () => of([{ name: 'Jane Doe', year: '2026' }]),
          getOfficers$: () => of([{ title: 'President', name: 'John Doe', dateStarted: new Date('2025-01-01') }]),
        }),
        MockProvider(PermissionsService, {
          isAuthenticated$: of(true),
          hasPermission$: () => of(true),
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrganizationComponent);
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('should show editor to users with leadership permission', async () => {
    const editButtonDe = fixture.debugElement.query(By.css('button.btn-primary'));
    const editButtonElement = editButtonDe.nativeElement as HTMLButtonElement;

    expect(editButtonElement.textContent).toContain('Edit');

    editButtonDe.triggerEventHandler('click');
    await fixture.whenStable();

    expect(fixture.debugElement.query(By.directive(LeadershipEditorComponent))).toBeTruthy();
  });
});
