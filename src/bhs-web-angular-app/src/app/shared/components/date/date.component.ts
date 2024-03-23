import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'app-date',
  templateUrl: './date.component.html',
  styleUrl: './date.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DateComponent {
  @Input() datetime = new Date();
}
