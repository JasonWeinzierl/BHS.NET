import { Component, OnInit } from '@angular/core';
import { AlertTheme } from '@data/banners/models/alert-theme';
import { SiteBanner } from '@data/banners/models/site-banner';
import { SiteBannerService } from '@data/banners/services/site-banner.service';

class SiteBannerStyled implements SiteBanner {
 constructor(
   public theme: AlertTheme,
   public alertType: string,
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
  isCollapsed = true;

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
      let alertType = 'light';

      switch (b.theme) {
        case AlertTheme.Primary:
          alertType = 'primary';
          break;
        case AlertTheme.Secondary:
          alertType = 'secondary';
          break;
        case AlertTheme.Success:
          alertType = 'success';
          break;
        case AlertTheme.Danger:
          alertType = 'danger';
          break;
        case AlertTheme.Warning:
          alertType = 'warning';
          break;
        case AlertTheme.Info:
          alertType = 'info';
          break;
      }

      return new SiteBannerStyled(b.theme, alertType, b.lead, b.body);
    });
  }

}
