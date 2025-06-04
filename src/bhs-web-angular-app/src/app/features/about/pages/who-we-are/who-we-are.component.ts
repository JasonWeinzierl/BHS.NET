import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-who-we-are',
  templateUrl: './who-we-are.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class WhoWeAreComponent { }
