import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';
import { AppEnvironment } from './environments';

fetch('/api/client-app-environment')
  .then(async response => {
    const appEnv = await parseAppEnvironment(response) ?? new AppEnvironment();

    // Angular startup.
    await platformBrowserDynamic([{ provide: AppEnvironment, useValue: appEnv }]).bootstrapModule(AppModule);

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

    // TODO: Once ajv includes an ESM distribution, introduce JSON validation here.
    // This is risky to disable the linter.

    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
    const json: AppEnvironment = await response.json();

    return new AppEnvironment(json.appInsights, json.auth0);
  } catch (e) {
    console.error('Failed to read environment configuration as JSON.', e);
    return null;
  }
}
