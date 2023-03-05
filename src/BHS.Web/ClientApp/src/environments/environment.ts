export const environment = {
  appInsights: {
    connectionString: '',
  },
  auth0: {
    domain: 'dev-wz656qr7u8q11d84.us.auth0.com',
    clientId: 'MEQJ4p2Y10CLMRebfZAEV2o2zxJVNFk7',
    authorizationParams: {
      audience: 'https://beltonhistoricalsociety.org/api/swagger/index.html',
      // eslint-disable-next-line @typescript-eslint/naming-convention
      redirect_uri: window.location.origin,
    },
  },
  version: {
    semver: '0.0.0',
    info: '0.0.0+vnext',
  },
};
