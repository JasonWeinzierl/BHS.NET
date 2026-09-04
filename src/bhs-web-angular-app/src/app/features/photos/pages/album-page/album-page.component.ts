import { NgOptimizedImage } from '@angular/common';
import { afterNextRender, Component, ElementRef, inject, input, viewChild } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AlbumPhotos, Photo } from '@data/photos';

@Component({
  selector: 'app-album-page',
  templateUrl: './album-page.component.html',
  imports: [
    RouterLink,
    NgOptimizedImage,
  ],
  host: {
    '(document:keydown.arrowleft)': 'onPreviousPhotoKeyboardShortcut()',
    '(document:keydown.arrowright)': 'onNextPhotoKeyboardShortcut()',
    '(document:keydown.escape)': 'onCloseKeyboardShortcut()',
  },
})
export class AlbumPageComponent {
  private readonly router = inject(Router);
  private readonly dialog = viewChild.required<ElementRef<HTMLDialogElement>>('dialog');
  private readonly closeButton = viewChild.required<ElementRef<HTMLAnchorElement>>('closeButton');

  readonly album = input.required<AlbumPhotos>();
  readonly currentPhoto = input.required<Photo>();
  readonly previousPhotoId = input.required<string>();
  readonly nextPhotoId = input.required<string>();

  constructor() {
    afterNextRender(() => {
      const dialog = this.dialog().nativeElement;
      if (typeof dialog.showModal === 'function') {
        dialog.showModal();
      } else {
        dialog.setAttribute('open', '');
      }
      this.closeButton().nativeElement.focus();
    });
  }

  onCloseKeyboardShortcut(): void {
    this.navigate();
  }

  onPreviousPhotoKeyboardShortcut(): void {
    this.navigate(this.previousPhotoId());
  }

  onNextPhotoKeyboardShortcut(): void {
    this.navigate(this.nextPhotoId());
  }

  private navigate(photoId?: string): void {
    const commands = ['/apps/photos/album', this.album().slug];
    if (photoId) {
      commands.push('photo', photoId);
    }

    this.router.navigate(commands)
      .catch((error: unknown) => { console.error(error); });
  }
}
