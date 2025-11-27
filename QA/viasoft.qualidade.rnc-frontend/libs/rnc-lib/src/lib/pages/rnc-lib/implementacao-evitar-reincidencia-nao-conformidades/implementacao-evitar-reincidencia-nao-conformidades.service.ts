/* eslint-disable max-len */
import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { Observable } from 'rxjs';
import { ImplementacaoEvitarReincidenciaNaoConformidadesModel } from '../../../api-clients/Implementacao-Evitar-Reincidencia-Nao-Conformidades/model/implementacao-evitar-reincidencia-nao-conformidades-model';
import { ImplementacaoEvitarReincidenciaNaoConformidadesInput } from '../../../api-clients/Implementacao-Evitar-Reincidencia-Nao-Conformidades/model/implementacao-evitar-reincidencia-nao-conformidades-input';

@Injectable()
export class ImplementacaoEvitarReincidenciaNaoConformidadesService {
  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {}

  public get(id: string, idNaoConformidade:string): Observable<ImplementacaoEvitarReincidenciaNaoConformidadesModel> {
    return this.httpClient.get<ImplementacaoEvitarReincidenciaNaoConformidadesModel>(`${this.basePath(idNaoConformidade)}/${id}`);
  }

  public getList(input: VsGridGetInput, idNaoConformidade:string, idDefeito:string): Observable<IPagedResultOutputDto<ImplementacaoEvitarReincidenciaNaoConformidadesModel>> {
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
    httpParams = httpParams.append('idDefeito', idDefeito);

    return this.httpClient.get<IPagedResultOutputDto<ImplementacaoEvitarReincidenciaNaoConformidadesModel>>(`${this.basePath(idNaoConformidade)}`, { params: httpParams });
  }

  public create(input: ImplementacaoEvitarReincidenciaNaoConformidadesInput): Observable<ImplementacaoEvitarReincidenciaNaoConformidadesModel> {
    return this.httpClient.post<ImplementacaoEvitarReincidenciaNaoConformidadesModel>(`${this.basePath(input.idNaoConformidade)}`, input);
  }

  public update(input: ImplementacaoEvitarReincidenciaNaoConformidadesInput): Observable<ImplementacaoEvitarReincidenciaNaoConformidadesModel> {
    return this.httpClient.put<ImplementacaoEvitarReincidenciaNaoConformidadesModel>(`${this.basePath(input.idNaoConformidade)}/${input.id}`, input);
  }

  public delete(id: string, idNaoConformidade: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.basePath(idNaoConformidade)}/${id}`);
  }
  private basePath(idNaoConformidade:string):string {
    return `${ensureTrailingSlash(this.gateway)}qualidade/rnc/gateway/nao-conformidades/${idNaoConformidade}/implementacao-evitar-reincidencias`;
  }
}
