import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { ensureTrailingSlash, VS_API_PREFIX } from '@viasoft/http';

import { SessionService } from '../../services/session.service';
import { ProdutoOutput } from './produto-autocomplete-select.component';

@Injectable()
export class ProdutoAutocompleteSelectService {
  constructor(protected httpClient: HttpClient, @Inject(VS_BACKEND_URL) private apiBaseUrl: string,
              @Inject(VS_API_PREFIX) private apiPrefix: string, private sessionService: SessionService) {
  }

  public getList(filter: string, skipCount: number, maxResultCount: number): Observable<IPagedResultOutputDto<ProdutoOutput>> {
    let queryParameters = new HttpParams();

    skipCount = skipCount ?? 0;

    if (filter !== null && filter !== undefined) {
      queryParameters = queryParameters.set('filter', filter);
    }
    if (skipCount !== null && skipCount !== undefined) {
      queryParameters = queryParameters.set('skip', skipCount.toString());
    }
    if (maxResultCount !== null && maxResultCount !== undefined) {
      queryParameters = queryParameters.set('pageSize', maxResultCount.toString());
    }

    return this.httpClient.get<IPagedResultOutputDto<ProdutoOutput>>(this.basePath(), {
      params: queryParameters,
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  public get(codigoProduto: string): Observable<ProdutoOutput> {
    const regex = new RegExp("/", 'g');
    codigoProduto = codigoProduto.replace(regex, "%3A");
    return this.httpClient.get<ProdutoOutput>(`${this.basePath()}/${codigoProduto}`, { headers: this.sessionService.defaultHttpHeaders });
  }

  private basePath(): string {
    return `${ensureTrailingSlash(this.apiBaseUrl)}${this.apiPrefix}/produtos`;
  }
}
