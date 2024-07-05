import { AsyncPipe, NgOptimizedImage } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { catchError, map, of, startWith } from 'rxjs';
import { MuseumService } from '@data/museum';

const ZERO_INDEXED_DAYS = [
  'Sundays',
  'Mondays',
  'Tuesdays',
  'Wednesdays',
  'Thursdays',
  'Fridays',
  'Saturdays',
];

const ONE_INDEXED_MONTHS = [
  '',
  'January',
  'February',
  'March',
  'April',
  'May',
  'June',
  'July',
  'August',
  'September',
  'October',
  'November',
  'December',
];

function toStandardTime(militaryTime: string): string {
  const [hours, minutes] = militaryTime.split(':').map(Number);
  const period = hours < 12 ? 'AM' : 'PM';
  const standardHours = hours % 12 || 12;
  return `${standardHours.toString()}:${String(minutes).padStart(2, '0')} ${period}`;
}

@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  styleUrl: './location.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    AsyncPipe,
    NgOptimizedImage,
  ],
})
export class LocationComponent {
  private readonly museumService = inject(MuseumService);

  readonly schedule$ = this.museumService.getSchedule().pipe(
    map(schedule => schedule ? {
      days: schedule.days.map(day => ({
        dayOfWeek: ZERO_INDEXED_DAYS[day.dayOfWeek],
        fromTime: day.fromTime,
        fromTimeStandard: toStandardTime(day.fromTime),
        toTime: day.toTime,
        toTimeStandard: toStandardTime(day.toTime),
      })),
      months: {
        startMonth: ONE_INDEXED_MONTHS[schedule.months.startMonth],
        endMonth: ONE_INDEXED_MONTHS[schedule.months.endMonth],
      },
      isLoading: false as const,
    } : null),
    catchError((err: unknown) => {
      console.error('Failed to load schedule.', err);
      return of(null);
    }),
    startWith({
      isLoading: true as const,
    }),
  );
}
