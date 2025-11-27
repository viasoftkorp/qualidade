import { enableProdMode, NgZone } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { NavigationStart, Router } from '@angular/router';
import { initAppSettings } from '@viasoft/common';
import * as LogRocket from 'logrocket';
import { getSingleSpaExtraProviders, singleSpaAngular } from 'single-spa-angular';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import { LOG_ROCKET_DEFAULT_APP_ID } from './log-rocket/log-rocket.consts';
import { ILogRocketOptions } from './log-rocket/log-rocket.interfaces';
import { singleSpaPropsSubject } from './single-spa/single-spa-props';

if (environment.production) {
  enableProdMode();
}

const lifecycles = singleSpaAngular({
  bootstrapFunction: singleSpaProps => {
    singleSpaPropsSubject.next(singleSpaProps);
    return initAppSettings(environment, true).then(() => {
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

      return platformBrowserDynamic(getSingleSpaExtraProviders()).bootstrapModule(AppModule)
        .catch(err => { throw err });
    });
  },
  template: '<app-root />',
  Router,
  NgZone,
  NavigationStart
});

export const bootstrap = lifecycles.bootstrap;
export const mount = lifecycles.mount;
export const unmount = lifecycles.unmount;
