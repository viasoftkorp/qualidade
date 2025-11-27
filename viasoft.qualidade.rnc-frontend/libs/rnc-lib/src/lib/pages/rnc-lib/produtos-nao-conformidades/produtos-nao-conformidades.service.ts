/* eslint-disable max-len */
import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { Observable } from 'rxjs';
import {
  ProdutosNaoConformidadesInput,
  ProdutosNaoConformidadesOutput
} from '../../../api-clients/Nao-Conformidades/Produtos-Nao-Conformidades/model';

@Injectable({
  providedIn: 'root'
})
export class ProdutosNaoConformidadesService {
  private endpoint: string;

  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/core/nao-conformidades`;
  }

  public get(id: string, idNaoConformidade:string): Observable<ProdutosNaoConformidadesOutput> {
    return this.httpClient.get<ProdutosNaoConformidadesOutput>(`${this.endpoint}/${idNaoConformidade}/produtos/${id}`);
  }

  public getList(input: VsGridGetInput, idNaoConformidade:string): Observable<IPagedResultOutputDto<ProdutosNaoConformidadesOutput>> {
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
    return this.httpClient.get<IPagedResultOutputDto<ProdutosNaoConformidadesOutput>>(`${this.endpoint}/${idNaoConformidade}/produtos`, { params: httpParams });
  }

  public create(input: ProdutosNaoConformidadesInput, idNaoConformidade: string): Observable<ProdutosNaoConformidadesOutput> {
    return this.httpClient.post<ProdutosNaoConformidadesOutput>(`${this.endpoint}/${idNaoConformidade}/produtos`, input);
  }

  public update(id: string, idNaoConformidade: string, input: ProdutosNaoConformidadesInput): Observable<ProdutosNaoConformidadesOutput> {
    return this.httpClient.put<ProdutosNaoConformidadesOutput>(`${this.endpoint}/${idNaoConformidade}/produtos/${id}`, input);
  }

  public delete(id: string, idNaoConformidade: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.endpoint}/${idNaoConformidade}/produtos/${id}`);
  }
}
