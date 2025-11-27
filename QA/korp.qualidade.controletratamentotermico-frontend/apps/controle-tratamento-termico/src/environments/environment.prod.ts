export const environment = {
  production: true,
  onPremise: '!ON_PREMISE_MODE',
  local: '/assets/app-settings/appsettings.json',
  gatewayUrl: 'http://host.docker.internal:9999',
  cdnUrl: '!CDN_URL',
  assetsUrl: '!CDN_URL/assets/',
  mock: false,
};
