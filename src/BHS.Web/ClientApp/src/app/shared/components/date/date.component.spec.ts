import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DateComponent } from './date.component';

describe('DateComponent', () => {
  let component: DateComponent;
  let fixture: ComponentFixture<DateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DateComponent ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(DateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set title', () => {
    const element = fixture.nativeElement as HTMLElement;

    const timeElement = element.querySelector('time');
    const titleAttr = timeElement?.getAttribute('title');

    expect(titleAttr).toContain(component.datetime.getFullYear());
  });
});
