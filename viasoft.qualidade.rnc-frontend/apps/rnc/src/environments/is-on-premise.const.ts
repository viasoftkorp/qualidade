import { environment } from './environment';

export const IS_ON_PREMISE = (environment.onPremise as string | boolean) === true
  || String(environment.onPremise).toLowerCase() === String(true);
