import { ChangeDetectionStrategy, Component } from '@angular/core';
import { appVersion } from 'src/environments';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FooterComponent {
  year: string;

  constructor() {
    this.year = appVersion.commitDate.split('-')[0];
  }
}
