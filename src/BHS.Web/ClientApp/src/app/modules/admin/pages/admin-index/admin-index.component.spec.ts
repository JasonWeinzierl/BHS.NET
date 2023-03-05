import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminIndexComponent } from './admin-index.component';
import { AuthService } from '@auth0/auth0-angular';
import { RouterLinkDirectiveStub } from '@app/mock-testing-objects';

describe('AdminIndexComponent', () => {
  let component: AdminIndexComponent;
  let fixture: ComponentFixture<AdminIndexComponent>;

  beforeEach(async () => { // TODO: investigate.  older tests use waitForAsync instead.
    const auth = jasmine.createSpyObj<AuthService>('auth', ['logout']);

    await TestBed.configureTestingModule({
      declarations: [ AdminIndexComponent, RouterLinkDirectiveStub ],
      providers: [
        {
          provide: AuthService,
          useValue: auth,
        },
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
