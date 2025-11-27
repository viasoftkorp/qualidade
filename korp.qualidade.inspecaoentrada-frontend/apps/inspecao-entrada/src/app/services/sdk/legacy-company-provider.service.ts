import { HttpClient, HttpParams } from '@angular/common/http';
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

    let params = new HttpParams();
    params = params.set('usuario', USER);

    return this.httpClient.get<VsCompaniesDetail>(ROUTE, { params })
      .pipe(map((response: VsCompaniesDetail) => {
        const result: VsCompaniesDetail = {
          companies: response?.companies.map((companyDetails: VsCompanyDetails) => {
            companyDetails.id = companyDetails.legacyCompanyId.toString();
            return companyDetails;
          })
        };
        return result;
      }));
  }
}
