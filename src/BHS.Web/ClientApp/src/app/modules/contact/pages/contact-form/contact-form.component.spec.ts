import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MockModule, MockProvider } from 'ng-mocks';
import { ContactFormComponent } from './contact-form.component';
import { ContactService } from '@data/contact-us';
import { ReactiveFormsModule } from '@angular/forms';

describe('ContactFormComponent', () => {
  let component: ContactFormComponent;
  let fixture: ComponentFixture<ContactFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        MockModule(ReactiveFormsModule),
      ],
      declarations: [
        ContactFormComponent,
      ],
      providers: [
        MockProvider(ContactService),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContactFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
