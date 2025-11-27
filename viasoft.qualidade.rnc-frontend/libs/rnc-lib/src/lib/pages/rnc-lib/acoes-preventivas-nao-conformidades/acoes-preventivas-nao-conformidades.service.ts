import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import {
  AcoesPreventivasNaoConformidadesInput,
  AcoesPreventivasNaoConformidadesModel
} from '../../../api-clients/Nao-Conformidades';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AcoesPreventivasNaoConformidadesService {
  private endpoint: string;

  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/core/nao-conformidades`;
  }

  public get(id: string, idNaoConformidade:string): Observable<AcoesPreventivasNaoConformidadesModel> {
    return this.httpClient.get<AcoesPreventivasNaoConformidadesModel>(`${this.endpoint}/${idNaoConformidade}/acoes-preventivas/${id}`);
  }

  public getList(input: VsGridGetInput, idNaoConformidade:string, idDefeito:string): Observable<IPagedResultOutputDto<AcoesPreventivasNaoConformidadesModel>> {
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

    return this.httpClient.get<IPagedResultOutputDto<AcoesPreventivasNaoConformidadesModel>>(`${this.endpoint}/${idNaoConformidade}/defeitos/${idDefeito}/acoes-preventivas`, { params: httpParams });
  }

  public create(input: AcoesPreventivasNaoConformidadesInput, idNaoConformidade: string): Observable<AcoesPreventivasNaoConformidadesModel> {
    return this.httpClient.post<AcoesPreventivasNaoConformidadesModel>(`${this.endpoint}/${idNaoConformidade}/acoes-preventivas`, input);
  }

  public update(id: string, idNaoConformidade: string, input: AcoesPreventivasNaoConformidadesInput): Observable<AcoesPreventivasNaoConformidadesModel> {
    return this.httpClient.put<AcoesPreventivasNaoConformidadesModel>(`${this.endpoint}/${idNaoConformidade}/acoes-preventivas/${id}`, input);
  }

  public delete(id: string, idNaoConformidade: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.endpoint}/${idNaoConformidade}/acoes-preventivas/${id}`);
  }
}
