import { mergeApplicationConfig } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { APP_ENVIRONMENT, APP_ENVIRONMENT_VALIDATOR, AppEnvironment } from './environments';
import { APP_CONFIG } from '@app/app.config';

// We can't use provideAppInitializer because other app initializers (e.g. auth0) need AppEnvironment and we can't guarantee order.
fetch('/api/client-app-environment')
  .then(async response => {
    const appEnv: AppEnvironment = await parseAppEnvironment(response) ?? {};

    // Angular startup.
    await bootstrapApplication(AppComponent, mergeApplicationConfig(
      { providers: [{ provide: APP_ENVIRONMENT, useValue: appEnv }] },
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
      loadingElement.classList.add('bg-error', 'text-error-content', 'text-4xl', 'p-2');
    }
  });

async function parseAppEnvironment(response: Response): Promise<AppEnvironment | null> {
  try {
    if (!response.ok) {
      console.error(`Failed to fetch environment configuration: ${response.statusText}`);
      return null;
    }

    return APP_ENVIRONMENT_VALIDATOR.parse(await response.json());
  } catch (e) {
    console.error('Failed to read environment configuration as JSON.', e);
    return null;
  }
}
