import { NgOptimizedImage } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { appVersion } from 'src/environments';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [NgOptimizedImage],
})
export default class AboutComponent {
  readonly version: typeof appVersion;

  constructor() {
    this.version = appVersion;
  }
}
