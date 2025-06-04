import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ApplicationConfig, ErrorHandler, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter, TitleStrategy, withInMemoryScrolling } from '@angular/router';
import { authHttpInterceptorFn, provideAuth0 } from '@auth0/auth0-angular';
import { ApplicationinsightsAngularpluginErrorService } from '@microsoft/applicationinsights-angularplugin-js';
import { provideToastr } from 'ngx-toastr';
import { APP_ROUTES } from './app.routes';
import { provideBhsAuth0Config } from '@core/providers/auth0-config.provider';
import { BhsTitleStrategy } from '@core/services/bhs-title-strategy';

export const APP_CONFIG: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
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
    provideToastr({
      // daisyUI classes.
      positionClass: 'toast',
      toastClass: 'alert',
      iconClasses: {
        error: 'alert-error',
        info: 'alert-info',
        success: 'alert-success',
        warning: 'alert-warning',
      },
    }),
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
