import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-donations',
  templateUrl: './donations.component.html',
  styleUrl: './donations.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class DonationsComponent { }
