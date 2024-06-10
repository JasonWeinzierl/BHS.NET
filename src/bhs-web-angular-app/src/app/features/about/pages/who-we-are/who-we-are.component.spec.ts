import { ComponentFixture, TestBed } from '@angular/core/testing';
import WhoWeAreComponent from './who-we-are.component';

describe('WhoWeAreComponent', () => {
  let component: WhoWeAreComponent;
  let fixture: ComponentFixture<WhoWeAreComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WhoWeAreComponent],
    })
    .compileComponents();

    fixture = TestBed.createComponent(WhoWeAreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
