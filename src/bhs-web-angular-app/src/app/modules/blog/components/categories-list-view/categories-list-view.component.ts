import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CategorySummary } from '@data/blog';

@Component({
  selector: 'app-categories-list-view',
  templateUrl: './categories-list-view.component.html',
  styleUrl: './categories-list-view.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CategoriesListViewComponent {
  @Input() isLoading = false;
  @Input() error?: string;
  @Input() categories: Array<CategorySummary> = [];
}
