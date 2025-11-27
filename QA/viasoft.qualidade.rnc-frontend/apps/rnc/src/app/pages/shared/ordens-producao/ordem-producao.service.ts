import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import {
  IPagedResultOutputDto, JQQBRule, JQQBRuleSet, JQQB_COND_OR, JQQB_OP_EQUAL, VsFilterManager, VsFilterTypeEnum
} from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { OrdemProducaoOutput } from '@viasoft/rnc/api-clients/Ordem-Producao/model';
import { Observable, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Injectable()
export class OrdemProducaoService {
  private readonly endpoint: string;
  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/gateway/ordens-producao`;
  }
  public getList(input: VsGridGetInput): Observable<IPagedResultOutputDto<OrdemProducaoOutput>> {
    let httpParams = new HttpParams();

    if (input.filter) {
      httpParams = httpParams.append('filter', input.filter);
    }
    if (input.advancedFilter) {
      httpParams = httpParams.append('advancedFilter', input.advancedFilter);
    }
    if (input.sorting) {
      httpParams = httpParams.append('sorting', input.sorting);
    }
    if (input.skipCount) {
      httpParams = httpParams.append('skipCount', input.skipCount.toString());
    }
    if (input.maxResultCount) {
      httpParams = httpParams.append('maxResultCount', input.maxResultCount.toString());
    }

    return this.httpClient.get<IPagedResultOutputDto<OrdemProducaoOutput>>(this.endpoint, { params: httpParams });
  }
  public get(id: string): Observable<OrdemProducaoOutput> {
    return this.httpClient.get<OrdemProducaoOutput>(`${this.endpoint}/${id}`);
  }
  public getByNumeroOdf(numeroOdf: number): Observable<OrdemProducaoOutput> {
    let httpParams = new HttpParams();

    httpParams = httpParams.append('numeroOdf', numeroOdf);
    httpParams = httpParams.append('skipCount', 0);
    httpParams = httpParams.append('maxResultCount', 1);
    const result = this.httpClient.get<IPagedResultOutputDto<OrdemProducaoOutput>>(this.endpoint, { params: httpParams });
    // eslint-disable-next-line max-len
    return result.pipe(switchMap((itensPaginados:IPagedResultOutputDto<OrdemProducaoOutput>) => {
      if (itensPaginados.items) {
        return of(itensPaginados.items[0]);
      } else {
        return of(null);
      }
    }));
  }
}
