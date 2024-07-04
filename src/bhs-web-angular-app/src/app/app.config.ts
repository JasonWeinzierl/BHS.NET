import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ApplicationConfig , ErrorHandler, importProvidersFrom } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter, TitleStrategy, withInMemoryScrolling } from '@angular/router';
import { authHttpInterceptorFn, provideAuth0 } from '@auth0/auth0-angular';
import { ApplicationinsightsAngularpluginErrorService } from '@microsoft/applicationinsights-angularplugin-js';
import { AlertModule } from 'ngx-bootstrap/alert';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { provideToastr } from 'ngx-toastr';
import { APP_ROUTES } from './app.routes';
import { provideBhsAuth0Config } from '@core/providers/auth0-config.provider';
import { provideBhsMarkdown } from '@core/providers/bootstrap-marked-options.provider';
import { BhsTitleStrategy } from '@core/services/bhs-title-strategy';

export const APP_CONFIG: ApplicationConfig = {
  providers: [
    importProvidersFrom(
      // ngx-bootstrap
      AlertModule.forRoot(),
      BsDatepickerModule.forRoot(),
      BsDropdownModule.forRoot(),
      CarouselModule.forRoot(),
      CollapseModule.forRoot(),
    ),
    provideRouter(
      APP_ROUTES,
      withInMemoryScrolling({
        anchorScrolling: 'enabled',
        scrollPositionRestoration: 'enabled',
      }),
    ),
    provideAnimations(),
    provideAuth0(),
    provideBhsAuth0Config(),
    provideHttpClient(
      withInterceptors([
        authHttpInterceptorFn,
      ]),
    ),
    provideBhsMarkdown(),
    provideToastr(),
    {
      provide: TitleStrategy,
      useClass: BhsTitleStrategy,
    },
    {
      provide: ErrorHandler,
      useClass: ApplicationinsightsAngularpluginErrorService,
    },
  ],
};
