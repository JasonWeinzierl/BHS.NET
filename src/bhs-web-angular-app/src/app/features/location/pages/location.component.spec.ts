import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MockProvider } from 'ng-mocks';
import { Subject } from 'rxjs';
import { MuseumSchedule, MuseumService } from '@data/museum';
import { LocationComponent } from './location.component';

describe('LocationComponent', () => {
  let component: LocationComponent;
  let fixture: ComponentFixture<LocationComponent>;
  let nativeElement: HTMLElement;
  const scheduleSubject$ = new Subject<MuseumSchedule>();

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LocationComponent],
      providers: [
        MockProvider(MuseumService, {
          getSchedule$: () => scheduleSubject$,
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(LocationComponent);
    nativeElement = fixture.nativeElement as HTMLElement;
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show loading if schedule is not loaded yet', () => {
    expect(nativeElement.querySelector('.loading')).toBeTruthy();
  });

  it('should show schedule if loaded', async () => {
    scheduleSubject$.next({
      days: [
        { dayOfWeek: 0, fromTime: '09:00', toTime: '17:00' },
      ],
      months: {
        startMonth: 1,
        endMonth: 12,
      },
    });

    await fixture.whenStable();
    const scheduleElement = nativeElement.querySelector('#location-museum-schedule');
    const timeEls = scheduleElement?.querySelectorAll('time');

    expect(nativeElement.querySelector('.spinner-grow')).toBeFalsy();
    expect(scheduleElement).toBeTruthy();
    expect(scheduleElement?.textContent).toContain('Sundays from 9:00 AM\u20135:00 PM');
    expect(scheduleElement?.textContent).toContain('Open January to December');

    expect(timeEls?.length).toBe(4);
    expect(timeEls?.[0].dateTime).toBe('09:00');
    expect(timeEls?.[1].dateTime).toBe('17:00');
  });

  it('should show unavailable on error', async () => {
    scheduleSubject$.error(new Error('Test failure to load schedule.'));

    await fixture.whenStable();
    const errorElement = nativeElement.querySelector('#location-museum-schedule-error');

    expect(nativeElement.querySelector('.spinner-grow')).toBeFalsy();
    expect(errorElement?.textContent).toContain('Hours of operation are not available');
  });
});
