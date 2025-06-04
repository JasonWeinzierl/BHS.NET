import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-donations',
  templateUrl: './donations.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class DonationsComponent { }
