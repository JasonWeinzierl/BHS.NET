import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { PostPreview } from '@data/blog';

@Component({
  selector: 'app-post-card',
  templateUrl: './post-card.component.html',
  styleUrl: './post-card.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PostCardComponent {
  @Input({ required: true }) post!: PostPreview;
}
