import { NgOptimizedImage } from '@angular/common';
import { Component, computed, effect, inject } from '@angular/core';
import { rxResource, toSignal } from '@angular/core/rxjs-interop';
import { ActivatedRoute, NavigationEnd, Router, RouterLink } from '@angular/router';
import { filter } from 'rxjs';
import { PhotosService } from '@data/photos';
import parseErrorMessage from '@shared/parse-error-message';
import { SnippetPipe } from '@shared/pipes/snippet.pipe';
import { AlbumPageComponent } from '../album-page/album-page.component';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  imports: [
    RouterLink,
    NgOptimizedImage,
    SnippetPipe,
    AlbumPageComponent,
  ],
})
export class AlbumComponent {
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly photosService = inject(PhotosService);

  private readonly routeParamMapSignal = toSignal(this.activatedRoute.paramMap, {
    initialValue: this.activatedRoute.snapshot.paramMap,
  });

  private readonly navigationEndSignal = toSignal(
    this.router.events.pipe(filter((event): event is NavigationEnd => event instanceof NavigationEnd)),
    { initialValue: undefined },
  );

  private readonly albumSlugSignal = computed(() => {
    const slug = this.routeParamMapSignal().get('slug');
    if (!slug) {
      throw new Error('Failed to get album slug from URL.');
    }
    return slug;
  });

  readonly selectedPhotoIdSignal = computed(() => {
    this.navigationEndSignal();
    return this.activatedRoute.firstChild?.snapshot.paramMap.get('id');
  });

  readonly albumResource = rxResource({
    params: () => this.albumSlugSignal(),
    stream: ({ params: slug }) => this.photosService.getAlbum$(slug),
  });

  readonly albumSignal = computed(() => this.albumResource.hasValue() ? this.albumResource.value() : undefined);

  readonly errorMessageSignal = computed(() => {
    const error = this.albumResource.error();
    return error ? parseErrorMessage(error) ?? 'An unknown error occurred.' : undefined;
  });

  readonly previewSignal = computed(() => {
    const album = this.albumSignal();
    const photoId = this.selectedPhotoIdSignal();
    if (!album || !photoId) {
      return undefined;
    }

    const currentIndex = album.photos.findIndex(photo => photo.id === photoId);
    if (currentIndex === -1) {
      return undefined;
    }

    const previousIndex = currentIndex === 0 ? album.photos.length - 1 : currentIndex - 1;
    const nextIndex = currentIndex === album.photos.length - 1 ? 0 : currentIndex + 1;
    return {
      currentPhoto: album.photos[currentIndex],
      previousPhotoId: album.photos[previousIndex].id,
      nextPhotoId: album.photos[nextIndex].id,
    };
  });

  constructor() {
    effect(() => {
      if (this.albumResource.hasValue() && this.selectedPhotoIdSignal() && !this.previewSignal()) {
        this.router.navigate(['/not-found'], { replaceUrl: true })
          .catch((error: unknown) => { console.error(error); });
      }
    });
  }
}
