import { HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import {API_GATEWAY, VS_API_PREFIX, VsJwtProviderService} from '@viasoft/http';
import { ENVIRONMENT, MessageService } from '@viasoft/common';

@Injectable({ providedIn: 'root' })
export class SessionService {
  constructor(
    private jwt: VsJwtProviderService,
    private messageService: MessageService,
    @Inject(VS_API_PREFIX) private apiPrefix: string,
    @Inject(API_GATEWAY) private apiGateway: string,
    @Inject(ENVIRONMENT) private environment: any,
  ) { }

  public get currentTenant(): string {
    if (this.environment.mock) {
      return 'b8b3a962-5c24-4a91-67c1-85c27b76d0ac';
    }
    return this.jwt.getTenantIdFromJwt();
  }

  public get currentDb(): string {
    if (this.environment.mock) {
      return 'default_database';
    }
    return this.jwt.getJwtProperty('ext.EnvironmentDatabaseName') || this.jwt.getJwtProperty('ext.DatabaseName');
  }

  public get currentUserLogin(): string {
    if (this.environment.mock) {
      return 'MOCK.USER';
    }
    return this.jwt.getJwtProperty('ext.UserLogin') || this.jwt.getJwtProperty('ext.UserName');
  }

  public get currentCompany(): string {
    if (this.environment.mock) {
      return '1';
    }
    return sessionStorage.getItem('legacyCompanyId');
  }

  public get currentCompanyId(): string {
    if (this.environment.mock) {
      return 'b8b3a962-5c24-4a91-67c1-85c27b76d0ac';
    }
    return sessionStorage.getItem('companyId');
  }

  public get currentBaseUrl(): string {
    return `${this.apiGateway}/${this.apiPrefix}`;
  }

  public get defaultHttpHeaders(): HttpHeaders {
    let headers = new HttpHeaders();

    if (this.currentTenant) {
      headers = headers.set('TenantId', this.currentTenant);
    } else {
      console.error('CurrentTenant header missing');
      this.messageService.error('Picking.Global.Errors.CurrentTenantMissing');
    }

    if (this.currentDb) {
      headers = headers.set('DatabaseName', this.currentDb);
    } else {
      console.error('DatabaseName header missing');
      this.messageService.error('Picking.Global.Errors.DatabaseNameMissing');
    }

    if (this.currentCompany) {
      headers = headers.set('CompanyRecno', this.currentCompany);
    } else {
      console.error('CompanyRecno header missing');
      this.messageService.error('Picking.Global.Errors.CompanyRecnoMissing');
    }

    if (this.currentCompanyId) {
      headers = headers.set('CompanyId', this.currentCompanyId);
    } else {
      console.error('CompanyRecno header missing');
      this.messageService.error('Picking.Global.Errors.CompanyRecnoMissing');
    }

    if (this.currentUserLogin) {
      headers = headers.set('UserLogin', this.currentUserLogin);
    } else {
      console.error('UserLogin header missing');
      this.messageService.error('Picking.Global.Errors.UserLoginMissing');
    }

    return headers;
  }
}
