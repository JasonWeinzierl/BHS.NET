import { ErrorHandler, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AngularPlugin } from '@microsoft/applicationinsights-angularplugin-js';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { AppEnvironment } from 'src/environments';

@Injectable({
  providedIn: 'root',
})
export class InsightsService {
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

  constructor(
    private readonly router: Router,
    private readonly env: AppEnvironment,
  ) { }

  init(): void {
    if (this.appInsights.config.connectionString) {
      this.appInsights.loadAppInsights();
    } else {
      console.info('Application Insights is not enabled because connection string was not provided.');
    }
  }

  // TODO: passthrough to appInsights as necessary.
}
