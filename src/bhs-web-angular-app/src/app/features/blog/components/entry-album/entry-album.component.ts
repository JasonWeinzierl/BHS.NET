import { NgOptimizedImage } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AlbumPhotos } from '@data/photos';

@Component({
  selector: 'app-entry-album',
  templateUrl: './entry-album.component.html',
  styleUrl: './entry-album.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, NgOptimizedImage],
})
export class EntryAlbumComponent {
  @Input({ required: true }) album!: AlbumPhotos;
}
