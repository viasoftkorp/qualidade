import { environment } from './environment';

export class UrlConsts {
  public static apiGateway(): string {
    return environment['settings']['backendUrl'];
  }
}