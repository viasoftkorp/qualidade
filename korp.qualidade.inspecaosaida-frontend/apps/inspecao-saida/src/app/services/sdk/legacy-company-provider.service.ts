import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import {
  API_GATEWAY,
  VsCompaniesDetail,
  VsCompanyDetails,
  VsJwtProviderService
} from '@viasoft/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class LegacyCompanyProviderService {
  constructor(
    private httpClient: HttpClient,
    private jwt: VsJwtProviderService,
    @Inject(API_GATEWAY) private apiBaseUrl: string
  ) { }

  public getCompanies(): Observable<VsCompaniesDetail> {
    const TENANT = this.jwt.getTenantIdFromJwt();
    const DB = (this.jwt.getJwtProperty('ext.EnvironmentDatabaseName')
      ?? this.jwt.getJwtProperty('ext.DatabaseName')) as string;
    const USER = this.jwt.getJwtProperty('ext.UserLogin') as string;
    const ROUTE = `${this.apiBaseUrl}/${TENANT}/${DB}/administracao/empresas`;

    const headers = new HttpHeaders();
    let params = new HttpParams();
    params = params.set('usuario', USER);

    return this.httpClient
      .get<VsCompaniesDetail>(
        ROUTE,
        {
          headers,
          params,
          observe: 'body',
          reportProgress: false
        }
      )
      .pipe(map((response: VsCompaniesDetail) => {
        let result: VsCompaniesDetail;
        result.companies = response.companies.map((c: VsCompanyDetails) => {
          const fixedCompany = c;
          fixedCompany.id = c.legacyCompanyId.toString();
          return fixedCompany;
        });
        return result;
      }));
  }
}
