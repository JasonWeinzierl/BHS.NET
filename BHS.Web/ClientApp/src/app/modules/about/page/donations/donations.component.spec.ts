import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { DonationsComponent } from './donations.component';

describe('DonationsComponent', () => {
  let component: DonationsComponent;
  let fixture: ComponentFixture<DonationsComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ DonationsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DonationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
