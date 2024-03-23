import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NotFoundComponent {
  closestPath$: Observable<string | null>;

  constructor(
    private readonly route: ActivatedRoute,
  ) {
    this.closestPath$ = this.route.data.pipe(
      map(data => {
        const closestPath: unknown = data['closestPath'];
        return typeof closestPath === 'string' ? closestPath : null;
      }),
    );
  }
}
