import { AppEnvironment } from './environments';
import { AppModule } from './app/app.module';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

fetch('/api/client-app-environment')
  .then(async response => {

    // Try to parse.
    let appEnv = new AppEnvironment();
    try {
      if (response.ok) {
        const json = await response.json();
        appEnv = new AppEnvironment(json?.appInsights, json?.auth0);
      } else {
        console.error('Failed to fetch environment configuration: ' + response.statusText);
      }
    } catch (err) {
      console.error('Failed to read environment configuration as JSON.', err);
    }

    // Angular startup.
    await platformBrowserDynamic([{ provide: AppEnvironment, useValue: appEnv }]).bootstrapModule(AppModule);

  })
  .catch((err: unknown) => {
    console.error('Website failed to load!', err);

    // Use plain JS to update index.html with a helpful error message.
    const loadingElement = document.getElementById('bootstrappingIndicator');
    if (loadingElement) {
      const message = err && typeof err === 'object' && Object.hasOwn(err, 'message') ? (err as Error).message : null;

      loadingElement.innerText = 'Website failed to load: ' + message ?? JSON.stringify(err);
      loadingElement.classList.add('bg-danger', 'text-white', 'fs-1');
    }
  });
