import { AsyncPipe, NgOptimizedImage } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { map } from 'rxjs';
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
    map(schedule => {
      if (!schedule?.days.length) {return 'Not open';}

      // TODO: for accessibility, we really should wrap the times in a <time> element.
      // TODO: use endash for date ranges?
      const days = schedule.days.map(day => `${ZERO_INDEXED_DAYS[day.dayOfWeek]} from ${day.fromTime} to ${day.toTime}`);
      const months = `Open ${ONE_INDEXED_MONTHS[schedule.months.startMonth]} through ${ONE_INDEXED_MONTHS[schedule.months.endMonth]}`;

      return days.join('\n') + '\n' + months;
    }),
  );
}
