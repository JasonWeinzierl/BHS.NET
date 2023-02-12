import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { PostPreview } from '@data/blog';

@Component({
  selector: 'app-post-card[post]',
  templateUrl: './post-card.component.html',
  styleUrls: ['./post-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PostCardComponent {
  @Input() post!: PostPreview;
}
