import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-donations',
  templateUrl: './donations.component.html',
  styleUrls: ['./donations.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DonationsComponent { }