import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { ensureTrailingSlash } from '@viasoft/http';
import { Observable } from 'rxjs';
import { SessionService } from '../../services/session.service';
import { ProdutoOutput } from './produto-output-class';

@Injectable()
export class ProdutoAutocompleteSelectService {
  constructor(
    protected httpClient: HttpClient,
    private sessionService: SessionService
  ) {}

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

    return this.httpClient.get<IPagedResultOutputDto<ProdutoOutput>>(this.basePath, {
      params: queryParameters,
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  public get(productCode: string): Observable<ProdutoOutput> {
    const regex = new RegExp("/", 'g');
    productCode = productCode.replace(regex, "%3A");
    return this.httpClient.get<ProdutoOutput>(`${this.basePath}/${productCode}`, { headers: this.sessionService.defaultHttpHeaders });
  }

  private get basePath(): string {
    return `${ensureTrailingSlash(this.sessionService.currentBaseUrl)}produtos`;
  }
}
