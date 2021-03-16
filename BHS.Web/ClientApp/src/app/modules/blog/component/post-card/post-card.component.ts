import { Component, Input } from '@angular/core';
import { PostPreview } from '@data/schema/post-preview';

@Component({
  selector: 'app-post-card',
  templateUrl: './post-card.component.html',
  styleUrls: ['./post-card.component.scss']
})
export class PostCardComponent {
  @Input() post: PostPreview;

  constructor() { }

}
