import { ErrorHandler, inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AngularPlugin } from '@microsoft/applicationinsights-angularplugin-js';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { APP_ENVIRONMENT } from 'src/environments';

@Injectable({
  providedIn: 'root',
})
export class InsightsService {
  private readonly router = inject(Router);
  private readonly env = inject(APP_ENVIRONMENT);

  private readonly angularPlugin = new AngularPlugin();
  private readonly appInsights = new ApplicationInsights({
    config: {
      connectionString: this.env.appInsights?.connectionString ?? undefined,

      enableCorsCorrelation: true,
      enableRequestHeaderTracking: true,
      enableResponseHeaderTracking: true,

      extensions: [this.angularPlugin],
      extensionConfig: {
        [this.angularPlugin.identifier]: {
          router: this.router,
          errorServices: [new ErrorHandler()],
        },
      },
    },
  });

  init(): void {
    if (this.appInsights.config.connectionString) {
      this.appInsights.loadAppInsights();
    } else {
      console.info('Application Insights is not enabled because connection string was not provided.');
    }
  }

  submitContactForm(): void {
    try {
      this.appInsights.trackEvent({ name: 'ContactFormSubmitted' });
    } catch (error) {
      console.error('Failed to submit contact form event to Application Insights.', error);
    }
  }
}
