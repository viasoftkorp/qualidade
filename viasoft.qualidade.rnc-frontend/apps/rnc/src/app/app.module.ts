import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  API_GATEWAY, HydraJwtProvider, VsHttpModule, VsJwtProviderService
} from '@viasoft/http';
import {
  DEFAULT_NAV_CONFIG,
  VsUserHydraService,
  VsUserService,
  VsAuthService,
  VsHydraAuthService
} from '@viasoft/common';
import { VsAppCoreModule } from '@viasoft/app-core';
import { NAVIGATION_MENU_ITEMS } from '@viasoft/rnc/app/tokens/consts/navigation.const';
import { RNC_I18N_EN } from '@viasoft/rnc/app/i18n/consts/rnc-i18n-en.const';
import { RNC_I18N_PT } from '@viasoft/rnc/app/i18n/consts/rnc-i18n-pt.const';
import { AppConsts } from '@viasoft/rnc/app/tokens/consts/app-consts.const';
import { AppRoutingModule } from '@viasoft/rnc/app/app-routing.module';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import {
  AUTHORIZATION_PROVIDER,
  VsAuthorizationModule
} from '@viasoft/authorization-management';
import { AuthorizationService } from '@viasoft/rnc/app/services/authorizations/authorization.service';
import { VsButtonModule } from '@viasoft/components';
import { VsNavigationViewModule } from '@viasoft/navigation';
import * as LogRocket from 'logrocket';
import { FileProviderTagStartupModule } from '@viasoft/custom-file-provider-tag';
import { PersonLibStartupModule } from '@viasoft/person-lib';
import { NoopAuthorizationStorageService } from './services/sdk/noop-authorization-storage.service';
import { NoopAuthorizationService } from './services/sdk/noop-authorization.service';
import { AppComponent } from './app.component';
import { environment } from '../environments/environment';
import { IS_ON_PREMISE } from '../environments/is-on-premise.const';
import { getCompanyProviderService } from './utils/getCompanyProviderService';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    VsHttpModule.forRoot({
      environment,
      customJwtProviderService: IS_ON_PREMISE ? HydraJwtProvider : undefined,
      isCompanyBased: true,
      customServices: {
        company: {
          companyProviderService: getCompanyProviderService(),
        }
      }
    }),
    VsAppCoreModule.forRoot({
      apiPrefix: 'qualidade/rnc/gateway',
      portalConfig: {
        appId: 'QUA06_W',
        appName: 'rnc',
        domain: 'QualityAssurance',
        navbarTitle: 'RNC'
      },
      formLayout: 'vertical',
      customServices: {
        authorizationStorageService: IS_ON_PREMISE ? NoopAuthorizationStorageService : undefined,
        authorizationService: IS_ON_PREMISE ? NoopAuthorizationService : undefined,
        authentication: {
          authService: IS_ON_PREMISE ? VsHydraAuthService : undefined
        },
        userService: IS_ON_PREMISE ? VsUserHydraService : undefined,
      },
      navConfig: {
        ...DEFAULT_NAV_CONFIG,
        showPortalButton: !IS_ON_PREMISE,
        shortcutsPosition: 'right',
        showUserButton: !IS_ON_PREMISE
      },
      translates: {
        en: RNC_I18N_EN,
        pt: RNC_I18N_PT
      },
      navigation: IS_ON_PREMISE
        ? NAVIGATION_MENU_ITEMS
        : [...NAVIGATION_MENU_ITEMS],
      environment
    }),
    VsNavigationViewModule,
    VsButtonModule,
    AppRoutingModule,
    VsAuthorizationModule.forRoot(),
    PersonLibStartupModule,
    FileProviderTagStartupModule,

    // TODO: Import here your libs modules
  ],
  providers: [
    { provide: API_GATEWAY, useFactory: AppConsts.apiGateway },
    { provide: VS_BACKEND_URL, useFactory: AppConsts.apiGateway },
    { provide: AUTHORIZATION_PROVIDER, useClass: AuthorizationService }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(authService: VsAuthService, userService: VsUserService, jwt: VsJwtProviderService) {
    authService.runInitialLoginSequence(true);
    userService.userUpdatedSubject.subscribe((user) => {
      LogRocket.identify(
        user.id,
        {
          name: user.name,
          email: user.email,
          'Environment ID': jwt.getEnvironmentIdFromJwt(),
          'Environment Name': jwt.getEnvironmentNameFromJwt(),
          'Organization ID': jwt.getEnvironmentOrganizationIdFromJwt(),
          'Organization Unit ID': jwt.getEnvironmentOrganizationUnitIdFromJwt(),
          'Tenant ID': jwt.getTenantIdFromJwt()
        }
      );
    });
  }
}
