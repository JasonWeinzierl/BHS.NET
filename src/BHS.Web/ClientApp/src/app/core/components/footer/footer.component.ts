import { ChangeDetectionStrategy, Component } from '@angular/core';
import { environment } from '@env';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FooterComponent {
  year: string;

  constructor() {
    this.year = environment.version.commitDate.split('-')[0];
  }
}
