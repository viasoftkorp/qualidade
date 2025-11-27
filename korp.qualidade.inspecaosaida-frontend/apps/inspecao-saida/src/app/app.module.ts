import { BrowserModule } from '@angular/platform-browser';
import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { API_GATEWAY, HydraJwtProvider, VS_API_PREFIX, VsHttpModule, VsJwtProviderService } from '@viasoft/http';
import { DEFAULT_NAV_CONFIG, UserService } from '@viasoft/common';
import { VsAppCoreModule } from '@viasoft/app-core';
import { NAVIGATION_MENU_ITEMS } from '@viasoft/inspecao-saida/app/tokens/consts/navigation.const';
import { INSPECAO_SAIDA_I18N_EN } from '@viasoft/inspecao-saida/app/i18n/consts/inspecao-saida-i18n-en.const';
import { INSPECAO_SAIDA_I18N_PT } from '@viasoft/inspecao-saida/app/i18n/consts/inspecao-saida-i18n-pt.const';
import { AppConsts } from '@viasoft/inspecao-saida/app/tokens/consts/app-consts.const';
import { AppRoutingModule } from '@viasoft/inspecao-saida/app/app-routing.module';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { AuthorizationModule } from '@viasoft/manage-authorization';
import { VsButtonModule } from '@viasoft/components';
import { VsNavigationViewModule } from '@viasoft/navigation';
import * as LogRocket from 'logrocket';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';
import { AppComponent } from './app.component';
import { environment } from '../environments/environment';
import { IS_ON_PREMISE } from '../environments/is-on-premise.const';

registerLocaleData(localePt);
@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    VsHttpModule.forRoot({
      environment,
      customJwtProviderService: IS_ON_PREMISE ? HydraJwtProvider : undefined,
      isCompanyBased: true
    }),
    VsAppCoreModule.forRoot({
      portalConfig: {
        appId: 'QUA02_W',
        appName: 'inspecao-saida',
        domain: 'QualityAssurance',
        navbarTitle: 'InspecaoSaida.Navigation.Title'
      },
      navConfig: {
        ...DEFAULT_NAV_CONFIG,
        showPortalButton: !IS_ON_PREMISE,
        shortcutsPosition: 'right',
        showUserButton: !IS_ON_PREMISE
      },
      translates: {
        en: INSPECAO_SAIDA_I18N_EN,
        pt: INSPECAO_SAIDA_I18N_PT
      },
      navigation: NAVIGATION_MENU_ITEMS,
      environment,
      apiPrefix: '/qualidade/inspecao-saida'
    }),
    VsNavigationViewModule,
    VsButtonModule,
    AppRoutingModule,
    AuthorizationModule,
  ],
  providers: [
    { provide: API_GATEWAY, useFactory: AppConsts.apiGateway },
    { provide: VS_BACKEND_URL, useFactory: AppConsts.apiGateway },
    { provide: VS_API_PREFIX, useFactory: () => AppConsts.appVersion() + '/qualidade/inspecao-saida' },
    { provide: LOCALE_ID, useValue: 'pt' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(userService: UserService, jwt: VsJwtProviderService) {
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
