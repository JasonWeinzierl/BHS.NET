import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { AlbumPhotos } from '@data/photos';

@Component({
  selector: 'app-entry-album',
  templateUrl: './entry-album.component.html',
  styleUrl: './entry-album.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EntryAlbumComponent {
  @Input({ required: true }) album!: AlbumPhotos;
}
