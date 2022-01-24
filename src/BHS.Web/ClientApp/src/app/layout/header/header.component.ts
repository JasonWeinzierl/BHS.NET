import { Component, OnInit } from '@angular/core';
import { AlertTheme } from '@data/schema/alert-theme';
import { SiteBanner } from '@data/schema/site-banner';
import { SiteBannerService } from '@data/service/site-banner.service';

class SiteBannerStyled implements SiteBanner {
 constructor(
   public theme: AlertTheme,
   public themeClass: string,
   public lead?: string,
   public body?: string,
 ) { }
}

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  isExpanded = false;

  banners: Array<SiteBannerStyled> = [];

  constructor(
    private bannerService: SiteBannerService
   ) { }

  ngOnInit(): void {
    this.bannerService.getEnabled()
      .subscribe(banners => this.setBanners(banners));
  }

  private setBanners(banners: Array<SiteBanner>): void {
    this.banners = banners.map(b => {
      let themeClass = 'alert-light';

      switch (b.theme) {
        case AlertTheme.Primary:
          themeClass = 'alert-primary';
          break;
        case AlertTheme.Secondary:
          themeClass = 'alert-secondary';
          break;
        case AlertTheme.Success:
          themeClass = 'alert-success';
          break;
        case AlertTheme.Danger:
          themeClass = 'alert-danger';
          break;
        case AlertTheme.Warning:
          themeClass = 'alert-warning';
          break;
        case AlertTheme.Info:
          themeClass = 'alert-info';
          break;
      }

      return new SiteBannerStyled(b.theme, themeClass, b.lead, b.body);
    });
  }

  collapse(): void {
    this.isExpanded = false;
  }

  toggle(): void {
    this.isExpanded = !this.isExpanded;
  }

}
