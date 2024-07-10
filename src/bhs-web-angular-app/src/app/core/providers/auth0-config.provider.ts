import { APP_INITIALIZER, inject, Provider } from '@angular/core';
import { AuthClientConfig, AuthConfig } from '@auth0/auth0-angular';
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
            httpMethod: 'Post',
          },
          {
            uri: '/api/blog/posts/*',
            httpMethod: 'Put',
          },
          {
            uri: '/api/blog/posts/*',
            httpMethod: 'Delete',
          },
          {
            uri: '/api/museum/schedule',
            httpMethod: 'Put',
          },
        ],
      },
    });
  };
};

/**
 * Initialize our config for Auth0.
 */
export function provideBhsAuth0Config(): Array<Provider> {
  return [
    {
      provide: APP_INITIALIZER,
      useFactory: auth0ConfigFactory,
      deps: [APP_ENVIRONMENT, AuthClientConfig],
      multi: true,
    },
  ];
}
