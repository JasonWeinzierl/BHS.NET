import { NgOptimizedImage } from '@angular/common';
import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AlbumPhotos } from '@data/photos';

@Component({
  selector: 'app-entry-album',
  templateUrl: './entry-album.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, NgOptimizedImage],
})
export class EntryAlbumComponent {
  readonly album = input.required<AlbumPhotos>();
}
