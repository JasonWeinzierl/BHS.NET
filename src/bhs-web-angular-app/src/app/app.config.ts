import { HTTP_INTERCEPTORS, provideHttpClient } from '@angular/common/http';
import { ApplicationConfig , ErrorHandler, Provider, importProvidersFrom } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { AuthHttpInterceptor, provideAuth0 } from '@auth0/auth0-angular';
import { ApplicationinsightsAngularpluginErrorService } from '@microsoft/applicationinsights-angularplugin-js';
import { AlertModule } from 'ngx-bootstrap/alert';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { provideMarkdown } from 'ngx-markdown';
import { provideToastr } from 'ngx-toastr';
import { APP_ROUTES } from './app.routes';
import { auth0ConfigProvider } from '@core/providers/auth0-config.provider';
import { bootstrapMarkedOptionsProvider } from '@core/providers/bootstrap-marked-options.provider';

// A function so that main.ts can dynamically add providers during startup.
// TODO: Can we use APP_INITIALIZER instead?
export const getAppConfig = (additionalProviders: Provider[]): ApplicationConfig => ({
  providers: [
    additionalProviders,
    importProvidersFrom(
      // ngx-bootstrap
      AlertModule.forRoot(),
      BsDatepickerModule.forRoot(),
      BsDropdownModule.forRoot(),
      CarouselModule.forRoot(),
      CollapseModule.forRoot(),
    ),
    provideRouter(APP_ROUTES),
    provideAnimations(),
    provideHttpClient(),
    provideAuth0(),
    provideMarkdown({
      markedOptions: [bootstrapMarkedOptionsProvider],
    }),
    provideToastr(),
    Title,
    {
      provide: ErrorHandler,
      useClass: ApplicationinsightsAngularpluginErrorService,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthHttpInterceptor,
      multi: true,
    },
    auth0ConfigProvider,
  ],
});
