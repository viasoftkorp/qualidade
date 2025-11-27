/* eslint-disable import/no-unresolved */
/* eslint-disable import/no-cycle */
import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';

import { IPagedResultOutputDto } from '@viasoft/common';
import { API_GATEWAY, ensureTrailingSlash } from '@viasoft/http';

import { VsGridGetInput } from '@viasoft/components';

import { PRODUTOS_PROXY_URL } from './tokens';
import { ProdutoOutput } from '../produto-autocomplete-select.component';
import { IPagelessResultDto } from '../../../api-clients/Utils/iPagelessResultDto';

@Injectable()
export class ProdutoAutocompleteSelectService {
  private readonly baseUrl: string;
private proxyUrl = 'qualidade/rnc/gateway/produtos';

constructor(@Inject(API_GATEWAY) protected gateway: string, @Inject(PRODUTOS_PROXY_URL) protected proxy: string,
  protected httpClient: HttpClient) {
  this.baseUrl = `${ensureTrailingSlash(this.gateway)}`;
  if (this.proxy) {
    this.proxyUrl = this.proxy;
  }
}

public getList(input: VsGridGetInput, codigoCategoria?: string)
  : Observable<IPagedResultOutputDto<ProdutoOutput>> {
  let params = new HttpParams();

  if (codigoCategoria) {
    params = params.set('codigoCategoria', codigoCategoria);
  }
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

  const output = this.httpClient.get<IPagedResultOutputDto<ProdutoOutput>>(`${this.baseUrl}${this.proxyUrl}`, { params });
  return output;
}

  public getListPageless(input: VsGridGetInput, codigoCategoria?: string)
    : Observable<IPagelessResultDto<ProdutoOutput>> {
    let params = new HttpParams();

    if (codigoCategoria) {
      params = params.set('codigoCategoria', codigoCategoria);
    }
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
    const output = this.httpClient
      .get<IPagelessResultDto<ProdutoOutput>>(`${this.baseUrl}${this.proxyUrl}/pageless`,
        { params });
    return output;
  }

public get(id: string): Observable<ProdutoOutput> {
  return this.httpClient.get<ProdutoOutput>(`${this.baseUrl}${this.proxyUrl}/${id}`);
}
}
