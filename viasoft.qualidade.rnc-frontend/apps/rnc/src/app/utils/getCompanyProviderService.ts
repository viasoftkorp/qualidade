import { MockCompanies } from './mockCompanies/mock-companies';
import { LegacyCompanyProviderService } from '../services/sdk/legacy-company-provider.service';
import { IS_ON_PREMISE } from '../../environments/is-on-premise.const';
import { environment } from '../../environments/environment';

export function getCompanyProviderService() {
  if (environment.mock) {
    return MockCompanies;
  }

  if (IS_ON_PREMISE) {
    return (LegacyCompanyProviderService as any);
  }

  return undefined;
}
