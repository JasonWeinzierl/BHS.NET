import { APP_INITIALIZER, FactoryProvider } from '@angular/core';
import { AuthClientConfig, HttpMethod } from '@auth0/auth0-angular';
import { AppEnvironment } from 'src/environments';

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
            uri: '/api/blog/posts',
            httpMethod: HttpMethod.Post,
          },
          {
            uri: '/api/blog/posts/*',
            httpMethod: HttpMethod.Put,
          },
          {
            uri: '/api/blog/posts/*',
            httpMethod: HttpMethod.Delete,
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
