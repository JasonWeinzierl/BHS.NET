import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CategorySummary } from '@data/blog';

@Component({
  selector: 'app-categories-list-view',
  templateUrl: './categories-list-view.component.html',
  styleUrl: './categories-list-view.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [RouterLink],
})
export class CategoriesListViewComponent {
  @Input() isLoading = false;
  @Input() error?: string;
  @Input() categories: Array<CategorySummary> = [];
}
