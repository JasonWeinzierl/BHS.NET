import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CategorySummary } from '@data/blog';

@Component({
  selector: 'app-categories-list-view',
  templateUrl: './categories-list-view.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink],
})
export class CategoriesListViewComponent {
  readonly isLoading = input(false);
  readonly error = input<string | null>();
  readonly categories = input<Array<CategorySummary>>([]);
}
