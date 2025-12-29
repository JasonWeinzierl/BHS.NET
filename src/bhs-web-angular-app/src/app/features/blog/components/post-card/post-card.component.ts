import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { PostPreview } from '@data/blog';
import { DateComponent } from '@shared/components/date/date.component';

@Component({
  selector: 'app-post-card',
  templateUrl: './post-card.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    RouterLink,
    DateComponent,
  ],
  host: { '[attr.data-testid]': `'PostCard-' + post().slug` },
})
export class PostCardComponent {
  readonly post = input.required<PostPreview>();
}
