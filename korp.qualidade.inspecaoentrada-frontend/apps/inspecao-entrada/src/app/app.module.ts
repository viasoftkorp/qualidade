import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  API_GATEWAY,
  HydraJwtProvider,
  VsHttpModule,
  VsJwtProviderService,
  VS_API_PREFIX
} from '@viasoft/http';
import {
  DEFAULT_NAV_CONFIG,
  VsAuthService,
  UserService
} from '@viasoft/common';
import { VsAppCoreModule } from '@viasoft/app-core';
import { NAVIGATION_MENU_ITEMS } from '@viasoft/inspecao-entrada/app/tokens/consts/navigation.const';
import { INSPECAO_ENTRADA_I18N_EN } from '@viasoft/inspecao-entrada/app/i18n/consts/inspecao-entrada-i18n-en.const';
import { INSPECAO_ENTRADA_I18N_PT } from '@viasoft/inspecao-entrada/app/i18n/consts/inspecao-entrada-i18n-pt.const';
import { AppConsts } from '@viasoft/inspecao-entrada/app/tokens/consts/app-consts.const';
import { AppRoutingModule } from '@viasoft/inspecao-entrada/app/app-routing.module';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { VsButtonModule } from '@viasoft/components';
import { VsNavigationViewModule } from '@viasoft/navigation';
import * as LogRocket from 'logrocket';
import { AUTHORIZATION_PROVIDER, VsAuthorizationModule } from '@viasoft/authorization-management';
import { AppComponent } from './app.component';
import { environment } from '../environments/environment';
import { IS_ON_PREMISE } from '../environments/is-on-premise.const';
import { AuthorizationProvider } from './services/authorization-provider.service';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    VsAuthorizationModule.forRoot(),
    VsHttpModule.forRoot({
      environment,
      customJwtProviderService: IS_ON_PREMISE ? HydraJwtProvider : undefined,
      isCompanyBased: true
    }),
    VsAppCoreModule.forRoot({
      apiPrefix: 'qualidade/inspecao-entrada',
      portalConfig: {
        appId: 'QUA01_W',
        appName: 'inspecao-entrada',
        domain: 'QualityAssurance',
        navbarTitle: 'InspecaoEntrada.Navigation.Title'
      },
      navConfig: {
        ...DEFAULT_NAV_CONFIG,
        showPortalButton: !IS_ON_PREMISE,
        shortcutsPosition: 'right',
        showUserButton: !IS_ON_PREMISE
      },
      translates: {
        en: INSPECAO_ENTRADA_I18N_EN,
        pt: INSPECAO_ENTRADA_I18N_PT
      },
      navigation: NAVIGATION_MENU_ITEMS,
      environment,
    }),
    VsNavigationViewModule,
    VsButtonModule,
    AppRoutingModule,
  ],
  providers: [
    { provide: API_GATEWAY, useFactory: AppConsts.apiGateway },
    { provide: VS_API_PREFIX, useFactory: () => `${AppConsts.appVersion()}/qualidade/inspecao-entrada` },
    { provide: VS_BACKEND_URL, useFactory: AppConsts.apiGateway },
    { provide: AUTHORIZATION_PROVIDER, useClass: AuthorizationProvider },
    { provide: VS_API_PREFIX, useFactory: () => `${AppConsts.appVersion()}/qualidade/inspecao-entrada` },
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(authService: VsAuthService, userService: UserService, jwt: VsJwtProviderService) {
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
