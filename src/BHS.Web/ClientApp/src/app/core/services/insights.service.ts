import { ErrorHandler, Injectable } from '@angular/core';
import { AngularPlugin } from '@microsoft/applicationinsights-angularplugin-js';
import { AppEnvironment } from 'src/environments';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class InsightsService {
  private angularPlugin = new AngularPlugin();
  private appInsights = new ApplicationInsights({
    config: {
      connectionString: this.env.appInsights?.connectionString,

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
    private router: Router,
    private env: AppEnvironment,
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
