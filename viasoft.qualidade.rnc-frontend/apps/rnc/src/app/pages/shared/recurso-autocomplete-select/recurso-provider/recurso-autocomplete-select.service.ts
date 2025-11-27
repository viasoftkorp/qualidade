import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';

import { IPagedResultOutputDto } from '@viasoft/common';
import { API_GATEWAY, ensureTrailingSlash } from '@viasoft/http';

import { VsGridGetInput } from '@viasoft/components';
// eslint-disable-next-line import/no-cycle
import { RecursoOutput } from '../recurso-autocomplete-select.component';
import { RECURSOS_PROXY_URL } from './tokens';

@Injectable()
export class RecursoAutocompleteSelectService {
  constructor(@Inject(API_GATEWAY) protected gateway: string, @Inject(RECURSOS_PROXY_URL) protected proxy: string,
  protected httpClient: HttpClient) {
    this.baseUrl = `${ensureTrailingSlash(this.gateway)}`;
    if (this.proxy) {
      this.proxyUrl = this.proxy;
    }
  }
  private readonly baseUrl: string;
  private proxyUrl = 'qualidade/rnc/gateway/recursos';

  public getList(input:VsGridGetInput)
    : Observable<IPagedResultOutputDto<RecursoOutput>> {
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

    return this.httpClient.get<IPagedResultOutputDto<RecursoOutput>>(`${this.baseUrl}${this.proxyUrl}`, { params });
  }

  public get(id: string): Observable<RecursoOutput> {
    return this.httpClient.get<RecursoOutput>(`${this.baseUrl}${this.proxyUrl}/${id}`);
  }
}
