import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NotFoundComponent {
  closestPath$: Observable<string | null>;

  constructor(
    private route: ActivatedRoute,
  ) {
    this.closestPath$ = this.route.data.pipe(
      map(data => {
        const closestPath: unknown = data['closestPath'];
        return typeof closestPath === 'string' ? closestPath : null;
      }),
    );
  }
}
