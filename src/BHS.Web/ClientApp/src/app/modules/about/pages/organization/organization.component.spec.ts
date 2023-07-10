import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EMPTY } from 'rxjs';
import { LeadershipService } from '@data/leadership/services/leadership.service';
import { MockProvider } from 'ng-mocks';
import { OrganizationComponent } from './organization.component';

describe('OrganizationComponent', () => {
  let component: OrganizationComponent;
  let fixture: ComponentFixture<OrganizationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        OrganizationComponent,
      ],
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
