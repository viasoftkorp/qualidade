/* eslint-disable import/no-cycle */
import { HttpParams, HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { API_GATEWAY, ensureTrailingSlash } from '@viasoft/http';
import { Observable } from 'rxjs';
import { CausaOutput } from '@viasoft/rnc/api-clients/Causas/model/causa-output';
import { CAUSAS_PROXY_URL } from './tokens';

@Injectable({
  providedIn: 'root'
})
export class CausaAutocompleteSelectService {
  constructor(
    @Inject(API_GATEWAY) protected gateway: string, @Inject(CAUSAS_PROXY_URL) protected proxy: string,
    protected httpClient: HttpClient
  ) {
    this.baseUrl = `${ensureTrailingSlash(this.gateway)}`;
    if (this.proxy) {
      this.proxyUrl = this.proxy;
    }
  }
  private readonly baseUrl: string;
  private proxyUrl = 'qualidade/rnc/gateway/causas';

  public getList(input:VsGridGetInput)
    : Observable<IPagedResultOutputDto<CausaOutput>> {
    let params = new HttpParams();
    if (input.filter) {
      params = params.set('filter', input.filter);
    }
    if (input.advancedFilter) {
      params = params.set('advancedFilter', input.advancedFilter);
    }
    if (input.sorting) {
      params = params.set('sorting', input.sorting);
    }
    if (input.skipCount) {
      params = params.set('skipCount', input.skipCount as any);
    }
    if (input.maxResultCount) {
      params = params.set('maxResultCount', input.maxResultCount as any);
    }


    return this.httpClient.get<IPagedResultOutputDto<CausaOutput>>(`${this.baseUrl}${this.proxyUrl}`, { params });
  }

  public get(id: string): Observable<CausaOutput> {
    return this.httpClient.get<CausaOutput>(`${this.baseUrl}${this.proxyUrl}/${id}`);
  }
}
