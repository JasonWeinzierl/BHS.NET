import { AsyncPipe, NgOptimizedImage } from '@angular/common';
import { ChangeDetectionStrategy, Component, DOCUMENT, inject } from '@angular/core';
import { catchError, map, of, startWith } from 'rxjs';
import { MuseumService } from '@data/museum';

function formatTime(militaryTime: string, formatter: Intl.DateTimeFormat): string {
  const [hours, minutes] = militaryTime.split(':').map(Number);
  return formatter.format(new Date(Date.UTC(1970, 0, 1, hours, minutes)));
}

function formatWeekday(zeroIndexedDay: number, formatter: Intl.DateTimeFormat): string {
  // 1970-01-04 is a Sunday in UTC.
  const referenceDate = new Date(Date.UTC(1970, 0, 4 + zeroIndexedDay));
  return formatter.format(referenceDate);
}

function formatMonth(oneIndexedMonth: number, formatter: Intl.DateTimeFormat): string {
  // 1970-(zero-indexed month)-01
  const referenceDate = new Date(Date.UTC(1970, oneIndexedMonth - 1, 1));
  return formatter.format(referenceDate);
}

@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    AsyncPipe,
    NgOptimizedImage,
  ],
  host: { 'data-testid': 'Location' },
})
export class LocationComponent {
  private readonly museumService = inject(MuseumService);
  private readonly documentLang = inject(DOCUMENT).documentElement.lang || 'en-US';

  private readonly timeFormatter = new Intl.DateTimeFormat(this.documentLang, {
    timeZone: 'UTC',
    hour: 'numeric',
    minute: '2-digit',
  });

  private readonly weekdayFormatter = new Intl.DateTimeFormat(this.documentLang, {
    timeZone: 'UTC',
    weekday: 'long',
  });

  private readonly monthFormatter = new Intl.DateTimeFormat(this.documentLang, {
    timeZone: 'UTC',
    month: 'long',
  });

  readonly schedule$ = this.museumService.getSchedule().pipe(
    map(schedule => schedule ? {
      days: schedule.days.map(day => ({
        dayOfWeek: formatWeekday(day.dayOfWeek, this.weekdayFormatter) + 's', // Assuming English, pluralizing is hard.
        fromTime: day.fromTime,
        fromTimeDisplay: formatTime(day.fromTime, this.timeFormatter),
        toTime: day.toTime,
        toTimeDisplay: formatTime(day.toTime, this.timeFormatter),
      })),
      months: {
        // Auto-generated types think this could be a string.
        startMonth: formatMonth(Number(schedule.months.startMonth), this.monthFormatter),
        endMonth: formatMonth(Number(schedule.months.endMonth), this.monthFormatter),
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
