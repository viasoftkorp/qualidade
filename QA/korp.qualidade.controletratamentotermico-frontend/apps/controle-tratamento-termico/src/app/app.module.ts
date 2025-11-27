import * as LogRocket from 'logrocket';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { API_GATEWAY, HydraJwtProvider, VS_API_PREFIX, VsCompanyService, VsHttpModule } from '@viasoft/http';
import { VsAppCoreModule } from '@viasoft/app-core';
import { NAVIGATION } from './tokens/consts/navigation.const';
import { CONTROLE_TRATAMENTO_TERMICO_I18N_PT } from './i18n/consts/controle-tratamento-termico-i18n-pt.const';
import { UrlConsts } from '@viasoft/controle-tratamento-termico/environments/url-consts';
import { AppRoutingModule } from './app-routing.module';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import {
  AUTHORIZATION_PROVIDER,
  VsAuthorizationModule,
  authorizationNavigationConfig
} from '@viasoft/authorization-management';
import { VsButtonModule } from '@viasoft/components';
import { VsNavigationViewModule } from '@viasoft/navigation';
import { VsAuthorizationApiMockService } from './services/vs-authorization-api-mock.service';
import { AppComponent } from './app.component';
import { environment } from '@viasoft/controle-tratamento-termico/environments/environment';
import { IS_ON_PREMISE } from '../environments/is-on-premise.const';
import { NoopAuthorizationStorageService } from './services/sdk/noop-authorization-storage.service';
import { NoopAuthorizationService } from './services/sdk/noop-authorization.service';
import { VsHydraAuthService, VsUserHydraService, VsUserService } from '@viasoft/common';
import { LegacyCompanyProviderService } from './services/sdk/legacy-company-provider.service';
import { AuthorizationProvider } from './authorization/authorization-provider';
import { AppConsts, LEGACY_COMPANY_PROVIDER_BASE_URL } from './tokens';
import { SessionService } from './services/session.service';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    VsHttpModule.forRoot({
      environment,
      isCompanyBased: true,
      customJwtProviderService: IS_ON_PREMISE ? HydraJwtProvider : undefined,
      customServices: {
        company: {
          companyProviderService: IS_ON_PREMISE ? LegacyCompanyProviderService as any : undefined
        }
      }
    }),
    VsAppCoreModule.forRoot({
      environment,
      apiPrefix: 'producao/gateway',
      portalConfig: {
        appId: 'QUA37',
        appName: 'controle-tratamento-termico',
        domain: 'QualityAssurance',
        navbarTitle: 'ControleTratamentoTermico.Navigation.Title'
      },
      translates: {
        pt: CONTROLE_TRATAMENTO_TERMICO_I18N_PT
      },
      navigation: [...NAVIGATION, authorizationNavigationConfig],
      customServices: {
        authorizationStorageService: IS_ON_PREMISE
          ? NoopAuthorizationStorageService
          : null,
        authorizationService: IS_ON_PREMISE ? NoopAuthorizationService : undefined,
        authentication: {
          authService: IS_ON_PREMISE ? VsHydraAuthService : undefined
        },
        userService: IS_ON_PREMISE ? VsUserHydraService : undefined
      }
    }),
    VsNavigationViewModule,
    VsButtonModule,
    AppRoutingModule,
    VsAuthorizationModule.forRoot({
      customServices: {
        api: environment.mock ? VsAuthorizationApiMockService : undefined
      }
    })
  ],
  providers: [
    { provide: API_GATEWAY, useFactory: () => UrlConsts.apiGateway() },
    { provide: VS_API_PREFIX, useFactory: () => `producao/gateway` },
    { provide: VS_BACKEND_URL, useFactory: () => UrlConsts.apiGateway() },
    { provide: LEGACY_COMPANY_PROVIDER_BASE_URL, useFactory: AppConsts.legacyCompanyProviderBaseUrl },
    { provide: AUTHORIZATION_PROVIDER, useClass: AuthorizationProvider },
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(userService: VsUserService, session: SessionService, company: VsCompanyService) {
    userService.userUpdatedSubject.subscribe((user) => {
      LogRocket.identify(user?.id, {
        name: user?.name,
        email: user?.email,
        Database: session.currentDb,
        'Company ID': session.currentCompany,
        'Company Name': company.currentCompany.companyName,
        'Trading Name': company.currentCompany.tradingName,
      });
    });
  }
}
