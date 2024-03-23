import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-who-we-are',
  templateUrl: './who-we-are.component.html',
  styleUrl: './who-we-are.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WhoWeAreComponent { }
