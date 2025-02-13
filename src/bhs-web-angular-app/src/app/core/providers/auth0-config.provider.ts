import { EnvironmentProviders, inject, provideAppInitializer } from '@angular/core';
import { AuthClientConfig, AuthConfig } from '@auth0/auth0-angular';
import { APP_ENVIRONMENT, AppEnvironment } from 'src/environments';

const auth0ConfigFactory = (env: AppEnvironment, cfg: AuthClientConfig) => {
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
            httpMethod: 'POST',
          },
          {
            uri: '/api/blog/posts/*',
            httpMethod: 'PUT',
          },
          {
            uri: '/api/blog/posts/*',
            httpMethod: 'DELETE',
          },
          {
            uri: '/api/museum/schedule',
            httpMethod: 'PUT',
          },
          {
            uri: '/api/banners/history',
            httpMethod: 'GET',
          },
        ],
      },
    });
  };
};

/**
 * Initialize our config for Auth0.
 */
export function provideBhsAuth0Config(): Array<EnvironmentProviders> {
  return [
    provideAppInitializer(() => {
      const initializerFn = auth0ConfigFactory(inject(APP_ENVIRONMENT), inject(AuthClientConfig));
      initializerFn();
    }),
  ];
}
