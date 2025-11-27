import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { SolucoesNaoConformidadesInput, SolucoesNaoConformidadesModel } from '../../../api-clients/Nao-Conformidades';

import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SolucoesNaoConformidadesService {
  private endpoint: string;

  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/core/nao-conformidades`;
  }

  public get(id: string, idNaoConformidade:string): Observable<SolucoesNaoConformidadesModel> {
    return this.httpClient.get<SolucoesNaoConformidadesModel>(`${this.endpoint}/${idNaoConformidade}/solucoes/${id}`);
  }

  public getList(input: VsGridGetInput, idNaoConformidade:string, idDefeito:string): Observable<IPagedResultOutputDto<SolucoesNaoConformidadesModel>> {
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
    return this.httpClient.get<IPagedResultOutputDto<SolucoesNaoConformidadesModel>>(`${this.endpoint}/${idNaoConformidade}/defeitos/${idDefeito}/solucoes`, { params: httpParams });
  }

  public create(input: SolucoesNaoConformidadesInput, idNaoConformidade: string): Observable<SolucoesNaoConformidadesModel> {
    return this.httpClient.post<SolucoesNaoConformidadesModel>(`${this.endpoint}/${idNaoConformidade}/solucoes`, input);
  }

  public update(id: string, idNaoConformidade: string, input: SolucoesNaoConformidadesInput): Observable<SolucoesNaoConformidadesModel> {
    return this.httpClient.put<SolucoesNaoConformidadesModel>(`${this.endpoint}/${idNaoConformidade}/solucoes/${id}`, input);
  }

  public delete(id: string, idNaoConformidade: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.endpoint}/${idNaoConformidade}/solucoes/${id}`);
  }
}
