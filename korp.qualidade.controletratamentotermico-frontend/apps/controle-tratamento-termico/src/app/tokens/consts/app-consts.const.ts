import { environment } from '../../../environments/environment';
export class AppConsts {
  public static legacyCompanyProviderBaseUrl(): string {
    return environment['settings'].legacyCompanyProviderBaseUrl;
  }
}
