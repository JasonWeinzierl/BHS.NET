import { NgOptimizedImage } from '@angular/common';
import { ChangeDetectionStrategy, Component, ElementRef, OnDestroy, OnInit, viewChild, viewChildren } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [NgOptimizedImage],
})
export class HomeComponent implements OnInit, OnDestroy {
  readonly carouselElement = viewChild.required<ElementRef<HTMLElement>>('carousel');
  readonly carouselItemElements = viewChildren<ElementRef<HTMLElement>>('carouselItem');

  private carouselInterval?: ReturnType<typeof setInterval>;
  private observer?: IntersectionObserver;
  private indexValue = 0;

  ngOnInit(): void {
    // eslint-disable-next-line @typescript-eslint/no-unnecessary-condition -- undefined in jsdom tests.
    this.observer = window.IntersectionObserver ? new IntersectionObserver(entries => {
      entries.forEach(entry => {
        // Keep index in sync with current viewport.
        if (entry.isIntersecting) {
          this.indexValue = this.carouselItemElements().map(e => e.nativeElement).indexOf(entry.target as HTMLElement);
          // Don't change too soon after user interaction.
          this.restartTimer();
        }
      });
    }, {
      // Relative to the carousel viewport.
      root: this.carouselElement().nativeElement,
      // Shrink the intersection bounds to a horizontally centered vertical line.
      rootMargin: '0% -50% 0% -50%',
      // Use the default threshold.
      threshold: 0,
    }) : undefined;

    this.carouselItemElements().forEach(item => {
      this.observer?.observe(item.nativeElement);
    });

    this.restartTimer();
  }

  ngOnDestroy(): void {
    clearInterval(this.carouselInterval);
    this.observer?.disconnect();
  }

  next(): void {
    this.indexValue = (this.indexValue + 1) % this.carouselItemElements().length;
    this.showCurrentItem();
  }

  prev(): void {
    this.indexValue = (this.indexValue - 1 + this.carouselItemElements().length) % this.carouselItemElements().length;
    this.showCurrentItem();
  }

  private restartTimer(): void {
    clearInterval(this.carouselInterval);
    this.carouselInterval = setInterval(() => {
      this.next();
    }, 10_000);
  }

  private showCurrentItem(): void {
    const items = this.carouselItemElements();
    // daisyUI carousel is just a horizontal scroll pane, so we need to scroll.
    this.carouselElement().nativeElement.scrollTo({
      left: items[this.indexValue].nativeElement.offsetLeft,
      behavior: 'smooth',
    });
  }
}
