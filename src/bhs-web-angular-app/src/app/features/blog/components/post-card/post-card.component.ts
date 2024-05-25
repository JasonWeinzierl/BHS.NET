import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DateComponent } from '../../../../shared/components/date/date.component';
import { PostPreview } from '@data/blog';

@Component({
  selector: 'app-post-card',
  templateUrl: './post-card.component.html',
  styleUrl: './post-card.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [RouterLink, DateComponent],
})
export class PostCardComponent {
  @Input({ required: true }) post!: PostPreview;
}
