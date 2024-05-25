import { mergeApplicationConfig } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { APP_ENVIRONMENT_VALIDATOR, AppEnvironment } from './environments';
import { APP_CONFIG } from '@app/app.config';

fetch('/api/client-app-environment')
  .then(async response => {
    const appEnv = await parseAppEnvironment(response) ?? new AppEnvironment();

    // Angular startup.
    await bootstrapApplication(AppComponent, mergeApplicationConfig(
      // TODO: Can we use APP_INITIALIZER instead?
      { providers: [{ provide: AppEnvironment, useValue: appEnv }] },
      APP_CONFIG,
    ));

  })
  .catch((err: unknown) => {
    console.error('Website failed to load!', err);

    // Use plain JS to update index.html with a helpful error message.
    const loadingElement = document.getElementById('bootstrappingIndicator');
    if (loadingElement) {
      const message = err && typeof err === 'object' && Object.hasOwn(err, 'message') ? (err as Error).message : null;

      loadingElement.innerText = 'Website failed to load: ' + (message ?? JSON.stringify(err));
      loadingElement.classList.add('bg-danger', 'text-white', 'fs-1');
    }
  });

async function parseAppEnvironment(response: Response): Promise<AppEnvironment | null> {
  try {
    if (!response.ok) {
      console.error(`Failed to fetch environment configuration: ${response.statusText}`);
      return null;
    }

    const json = APP_ENVIRONMENT_VALIDATOR.parse(await response.json());

    return new AppEnvironment(json.appInsights, json.auth0);
  } catch (e) {
    console.error('Failed to read environment configuration as JSON.', e);
    return null;
  }
}
