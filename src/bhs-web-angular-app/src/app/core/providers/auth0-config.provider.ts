import { APP_INITIALIZER, FactoryProvider, inject } from '@angular/core';
import { AuthClientConfig, AuthConfig , HttpMethod } from '@auth0/auth0-angular';
import { APP_ENVIRONMENT } from 'src/environments';

const auth0ConfigFactory = () => {
  const env = inject(APP_ENVIRONMENT);
  const cfg = inject(AuthClientConfig);

  return () => {
    if (!env.auth0) {
      console.error('Auth0 config not provided via app environment. Authentication will not work.');
    }

    const baseConfig: AuthConfig = env.auth0 ?? {
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
  deps: [APP_ENVIRONMENT, AuthClientConfig],
  multi: true,
};
