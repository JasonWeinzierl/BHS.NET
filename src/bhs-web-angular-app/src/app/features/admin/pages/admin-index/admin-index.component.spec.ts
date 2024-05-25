import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { MockProvider } from 'ng-mocks';
import { EMPTY } from 'rxjs';
import { AdminIndexComponent } from './admin-index.component';

describe('AdminIndexComponent', () => {
  let component: AdminIndexComponent;
  let fixture: ComponentFixture<AdminIndexComponent>;

  beforeEach(async () => {
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
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
