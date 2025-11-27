import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import { initAppSettings } from '@viasoft/common';

import { ILogRocketOptions } from './log-rocket/log-rocket.interfaces';
import { LOG_ROCKET_DEFAULT_APP_ID } from './log-rocket/log-rocket.consts';
import * as LogRocket from 'logrocket';

initAppSettings(environment).finally(() => {
  if (environment['settings'].logRocket?.enabled) {
    const logRocketOptions: ILogRocketOptions = { dom: {} };
    const logRocketCdnUrl = environment['settings'].logRocket.cdnUrl;
    const logRocketAppVersion = environment['settings'].logRocket.appVersion;
    const logRocketAppId = environment['settings'].logRocket.appId
      ? environment['settings'].logRocket.appId
      : LOG_ROCKET_DEFAULT_APP_ID;

    if (logRocketCdnUrl) {
      logRocketOptions.dom.baseHref = logRocketCdnUrl;
    }

    if (logRocketAppVersion) {
      logRocketOptions.release = logRocketAppVersion;
    }

    LogRocket.init(logRocketAppId, logRocketOptions);
  };

  platformBrowserDynamic().bootstrapModule(AppModule)
    .catch(err => console.error(err));
});
