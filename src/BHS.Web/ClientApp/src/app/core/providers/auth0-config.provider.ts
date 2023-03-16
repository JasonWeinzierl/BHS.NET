import { APP_INITIALIZER, FactoryProvider } from '@angular/core';
import { AppEnvironment } from 'src/environments';
import { AuthClientConfig } from '@auth0/auth0-angular';

const auth0ConfigFactory = (env: AppEnvironment, cfg: AuthClientConfig): () => void => {
  return () => {
    if (!env.auth0) {
      console.error('Auth0 config not provided via app environment. Authentication will not work.');
    }

    const baseConfig = env.auth0 ?? {
      domain: '',
      clientId: '',
    };

    baseConfig.authorizationParams = Object.assign({
      // eslint-disable-next-line @typescript-eslint/naming-convention
      redirect_uri: window.location.origin,
    }, baseConfig.authorizationParams);

    cfg.set({
      ...baseConfig,
      // Need to use local storage instead of in-memory login due to some browsers' privacy features.
      useRefreshTokens: true,
      cacheLocation: 'localstorage',
      // Specify which backend routes need auth.
      httpInterceptor: {
        allowedList: [
          {
            uri: '/api/admin/*', // TODO: For now, no routes will match this. Revisit when authenticated routes are implemented.
            allowAnonymous: true,
          },
        ],
      },
    });
  };
};

/**
 * Provider for initializing Auth0.
 */
export const auth0ConfigProvider: FactoryProvider = {
  provide: APP_INITIALIZER,
  useFactory: auth0ConfigFactory,
  deps: [AppEnvironment, AuthClientConfig],
  multi: true,
};
