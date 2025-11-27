/* eslint-disable max-len */
import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { DefeitosNaoConformidadesModel } from '@viasoft/rnc/api-clients/Nao-Conformidades';
import { DefeitosNaoConformidadesInput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Defeitos-Nao-Conformidades/model/defeitos-nao-conformidades-input';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DefeitosNaoConformidadesService {
  public atualizarGridDefeitos = new Subject<void>();
  private endpoint: string;

  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/gateway/nao-conformidades`;
  }

  public get(id: string, idNaoConformidade:string): Observable<DefeitosNaoConformidadesModel> {
    return this.httpClient.get<DefeitosNaoConformidadesModel>(`${this.endpoint}/${idNaoConformidade}/defeitos/${id}`);
  }

  public getList(input: VsGridGetInput, idNaoConformidade:string): Observable<IPagedResultOutputDto<DefeitosNaoConformidadesModel>> {
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

    return this.httpClient.get<IPagedResultOutputDto<DefeitosNaoConformidadesModel>>(`${this.endpoint}/${idNaoConformidade}/defeitos`, { params: httpParams });
  }

  public create(input: DefeitosNaoConformidadesInput, idNaoConformidade: string): Observable<DefeitosNaoConformidadesModel> {
    return this.httpClient.post<DefeitosNaoConformidadesModel>(`${this.endpoint}/${idNaoConformidade}/defeitos`, input);
  }

  public update(id: string, idNaoConformidade: string, input: DefeitosNaoConformidadesInput): Observable<DefeitosNaoConformidadesModel> {
    return this.httpClient.put<DefeitosNaoConformidadesModel>(`${this.endpoint}/${idNaoConformidade}/defeitos/${id}`, input);
  }

  public delete(id: string, idNaoConformidade: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.endpoint}/${idNaoConformidade}/defeitos/${id}`);
  }
}
