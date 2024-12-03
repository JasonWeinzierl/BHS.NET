import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { map } from 'rxjs';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    RouterLink,
    AsyncPipe,
  ],
})
export class NotFoundComponent {
  private readonly route = inject(ActivatedRoute);

  closestPath$ = this.route.data.pipe(
    map(data => {
      const closestPath: unknown = data['closestPath'];
      return typeof closestPath === 'string' ? closestPath : null;
    }),
  );
}
