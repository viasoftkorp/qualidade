/* eslint-disable max-len */
import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import {
  ServicosNaoConformidadesInput,
  ServicosNaoConformidadesModel,
  ServicosNaoConformidadesOutput,
} from '../../../api-clients/Nao-Conformidades';
import { Observable } from 'rxjs';
import { ServicoValidationResult } from '../../../api-clients/Nao-Conformidades/Servicos-Solucoes-Nao-Conformidades/model/serviceValidationResult';

@Injectable({
  providedIn: 'root',
})
export class ServicosNaoConformidadesService {
  private endpoint: string;

  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/core/nao-conformidades`;
  }

  public get(id: string, idNaoConformidade: string): Observable<ServicosNaoConformidadesModel> {
    return this.httpClient.get<ServicosNaoConformidadesModel>(`${this.endpoint}/${idNaoConformidade}/servicos/${id}`);
  }

  public getList(
    input: VsGridGetInput,
    idNaoConformidade: string
  ): Observable<IPagedResultOutputDto<ServicosNaoConformidadesModel>> {
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
    return this.httpClient.get<IPagedResultOutputDto<ServicosNaoConformidadesOutput>>(
      `${this.endpoint}/${idNaoConformidade}/servicos`,
      { params: httpParams }
    );
  }

  public create(
    input: ServicosNaoConformidadesInput,
    idNaoConformidade: string
  ): Observable<ServicoValidationResult> {
    return this.httpClient.post<ServicoValidationResult>(`${this.endpoint}/${idNaoConformidade}/servicos`, input);
  }

  public update(
    id: string,
    idNaoConformidade: string,
    input: ServicosNaoConformidadesInput
  ): Observable<ServicoValidationResult> {
    return this.httpClient.put<ServicoValidationResult>(
      `${this.endpoint}/${idNaoConformidade}/servicos/${id}`,
      input
    );
  }

  public delete(id: string, idNaoConformidade: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.endpoint}/${idNaoConformidade}/servicos/${id}`);
  }
}
