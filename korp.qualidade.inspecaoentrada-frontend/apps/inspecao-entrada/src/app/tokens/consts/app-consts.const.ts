import { environment } from '@viasoft/inspecao-entrada/environments/environment';

export class AppConsts {
  public static apiGateway(): string {
    return environment['settings']['backendUrl'];
  }

  public static apiBaseUrl(): string {
    return environment['settings']['apiBaseUrl'];
  }

  public static appVersion(): string {
    return environment['settings']['appVersion'];
  }
}
