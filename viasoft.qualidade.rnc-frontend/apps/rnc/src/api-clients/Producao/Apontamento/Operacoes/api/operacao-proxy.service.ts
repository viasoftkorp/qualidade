/* eslint-disable import/no-cycle */
import { HttpParams, HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { API_GATEWAY, ensureTrailingSlash } from '@viasoft/http';
import { Observable } from 'rxjs';
import { OPERACOES_PROXY_URL } from '../Tokens';
import { ApontamentoOperacaoOutput } from '../models/apontamento-operacao-output';

@Injectable()
export class OperacaoProxyService {
  constructor(
    @Inject(API_GATEWAY) protected gateway: string, @Inject(OPERACOES_PROXY_URL) protected proxy: string,
    protected httpClient: HttpClient
  ) {
    this.baseUrl = `${ensureTrailingSlash(this.gateway)}`;
    if (this.proxy) {
      this.proxyUrl = this.proxy;
    }
  }
  private readonly baseUrl: string;
  private proxyUrl = '';

  public getList(input:VsGridGetInput, numeroOdf: number)
    : Observable<IPagedResultOutputDto<ApontamentoOperacaoOutput>> {
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
      params = params.set('skipCount', input.skipCount);
    }
    if (input.maxResultCount) {
      params = params.set('maxResultCount', input.maxResultCount);
    }
    if (numeroOdf) {
      params = params.set('numeroOdf', numeroOdf);
    }

    return this.httpClient.get<IPagedResultOutputDto<ApontamentoOperacaoOutput>>(`${this.baseUrl}${this.proxyUrl}`, { params });
  }

  public get(id: string): Observable<ApontamentoOperacaoOutput> {
    return this.httpClient.get<ApontamentoOperacaoOutput>(`${this.baseUrl}${this.proxyUrl}/${id}`);
  }
}
