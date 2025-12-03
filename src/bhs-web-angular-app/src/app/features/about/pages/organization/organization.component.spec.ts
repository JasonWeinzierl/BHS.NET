import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EMPTY } from 'rxjs';
import OrganizationComponent from './organization.component';
import { LeadershipService } from '@data/leadership/services/leadership.service';
import { MockProvider } from 'ng-mocks';

describe('OrganizationComponent', () => {
  let component: OrganizationComponent;
  let fixture: ComponentFixture<OrganizationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrganizationComponent],
      providers: [
        MockProvider(LeadershipService, {
          getDirectors: () => EMPTY,
          getOfficers: () => EMPTY,
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrganizationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
