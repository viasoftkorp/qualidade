import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';

import { IPagedResultOutputDto } from '@viasoft/common';
import { API_GATEWAY, ensureTrailingSlash } from '@viasoft/http';

import { VsGridGetInput } from '@viasoft/components';
// eslint-disable-next-line import/no-cycle
import { ACOES_PREVENTIVAS_PROXY_URL } from './tokens';
import { AcaoPreventivaOutput } from '../../../api-clients/Nao-Conformidades/Acoes-Preventivas-Nao-Conformidades/model/acao-preventiva-output';

@Injectable()
export class AcaoPreventivaAutocompleteSelectService {
  constructor(@Inject(API_GATEWAY) protected gateway: string, @Inject(ACOES_PREVENTIVAS_PROXY_URL) protected proxy: string,
  protected httpClient: HttpClient) {
    this.baseUrl = `${ensureTrailingSlash(this.gateway)}`;
    if (this.proxy) {
      this.proxyUrl = this.proxy;
    }
  }
  private readonly baseUrl: string;
  private proxyUrl = 'qualidade/rnc/core/acoes-preventivas';

  public getList(input: VsGridGetInput)
    : Observable<IPagedResultOutputDto<AcaoPreventivaOutput>> {
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


    return this.httpClient.get<IPagedResultOutputDto<AcaoPreventivaOutput>>(`${this.baseUrl}${this.proxyUrl}`, { params });
  }

  public get(id: string): Observable<AcaoPreventivaOutput> {
    return this.httpClient.get<AcaoPreventivaOutput>(`${this.baseUrl}${this.proxyUrl}/${id}`);
  }
}
