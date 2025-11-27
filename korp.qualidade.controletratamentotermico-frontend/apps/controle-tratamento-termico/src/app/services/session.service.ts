import { Injectable, Inject } from "@angular/core";
import { MessageService, ENVIRONMENT } from "@viasoft/common";
import { IS_ON_PREMISE } from "@viasoft/controle-tratamento-termico/environments/is-on-premise.const";
import { VsJwtProviderService, API_GATEWAY } from "@viasoft/http";
import { AppConsts } from "../tokens";

@Injectable({ providedIn: 'root' })
export class SessionService {
  constructor(
    private jwt: VsJwtProviderService,
    @Inject(API_GATEWAY) private apiBaseUrl: string,
    @Inject(ENVIRONMENT) private environment: any
  ) { }

  public get currentTenant(): string {
    if (this.environment.mock) {
      return 'tenant';
    }
    return this.jwt.getTenantIdFromJwt();
  }

  public get currentEnvironment(): string {
    if (this.environment.mock) {
      return 'environment';
    }
    return this.jwt.getEnvironmentIdFromJwt();
  }

  public get currentDb(): string {
    if (this.environment.mock) {
      return 'BASE_ERP';
    }
    if (IS_ON_PREMISE) {
      return this.jwt.getJwtProperty('ext.EnvironmentDatabaseName');
    }
    return this.jwt.getJwtProperty('EnvironmentDatabaseName');
  }

  public get currentCompany(): string {
    if (this.environment.mock) {
      return '1';
    }

    const legacyCompanyId = sessionStorage.getItem('legacyCompanyId');
    if (legacyCompanyId) {
      return legacyCompanyId;
    } else {
      let attempts = 0;
      const checkInterval = setInterval(() => {
        const legacyCompanyId = sessionStorage.getItem('legacyCompanyId');
        if (legacyCompanyId) {
          clearInterval(checkInterval);
        } else if (++attempts === 5) {
          clearInterval(checkInterval);
        }
      }, 100);
      return '';
    }
  }

  public get currentBaseUrl() {
    return `${this.apiBaseUrl}`;
  }

  public get currentUser(): string {
    if (this.environment.mock) {
      return 'admin';
    }
    if (IS_ON_PREMISE) {
      return this.jwt.getJwtProperty('ext.UserLogin');
    }
    return this.jwt.getJwtProperty('UserLogin');
  }
}
