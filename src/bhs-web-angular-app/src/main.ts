import { mergeApplicationConfig } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { APP_CONFIG } from '@app/app.config';
import { AppComponent } from './app/app.component';
import { APP_ENVIRONMENT, APP_ENVIRONMENT_VALIDATOR, AppEnvironment } from './environments';

// We can't use provideAppInitializer because:
// - other app initializers (e.g. auth0) need AppEnvironment,
// - and AppEnvironment needs to be fetched asynchronously,
// - and AppEnvironment needs to be available as a provider too,
// - so it would be null until after initialization if injected from the provider.
try {
  const response = await fetch('/api/client-app-environment', {
    signal: AbortSignal.timeout(3000),
  });
  const appEnvironment: AppEnvironment = await parseAppEnvironment(response) ?? {};

  // Angular startup.
  await bootstrapApplication(AppComponent, mergeApplicationConfig(
    { providers: [{ provide: APP_ENVIRONMENT, useValue: appEnvironment }] },
    APP_CONFIG,
  ));
} catch (error: unknown) {
  console.error('Website failed to load!', error);

  // Use plain JS to update index.html with a helpful error message.
  const loadingElement = document.querySelector('#bootstrappingIndicator');
  if (loadingElement) {
    const message = error && typeof error === 'object' && Object.hasOwn(error, 'message') ? (error as Error).message : undefined;

    loadingElement.textContent = 'Website failed to load: ' + (message ?? JSON.stringify(error));
    loadingElement.classList.add('bg-error', 'text-error-content', 'text-4xl', 'p-2');
  }
}

async function parseAppEnvironment(response: Response): Promise<AppEnvironment | undefined> {
  try {
    if (!response.ok) {
      console.error(`Failed to fetch environment configuration: ${response.statusText}`);
      return undefined;
    }

    return APP_ENVIRONMENT_VALIDATOR.parse(await response.json());
  } catch (error) {
    console.error('Failed to read environment configuration as JSON.', error);
    return undefined;
  }
}
