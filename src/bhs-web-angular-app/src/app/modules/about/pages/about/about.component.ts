import { ChangeDetectionStrategy, Component } from '@angular/core';
import { appVersion } from 'src/environments';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrl: './about.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AboutComponent {
  version = appVersion;
}
