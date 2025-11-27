import { environment } from '@viasoft/rnc/environments/environment';

export class AppConsts {
  public static apiGateway(): string {
    return environment['settings']['backendUrl'];
  }
  public static appVersion(): string {
    return environment['settings']['appVersion'];
  }
}
export const APP_ID = '09CC1FEB-6CFA-4208-8AFA-5894EF7D51B6';
