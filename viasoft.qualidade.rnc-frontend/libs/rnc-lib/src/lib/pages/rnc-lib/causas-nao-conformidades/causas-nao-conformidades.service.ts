import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { CausasNaoConformidadesModel, CausasNaoConformidadesInput } from '../../../api-clients/Nao-Conformidades';
import { Observable } from 'rxjs';
import { DefeitosNaoConformidadesService } from '../defeitos-nao-conformidades/defeitos-nao-conformidades.service';

@Injectable({
  providedIn: 'root'
})
export class CausasNaoConformidadesService {
  private endpoint: string;

  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient, private defeitoService:DefeitosNaoConformidadesService) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/core/nao-conformidades`;
  }

  public get(id: string, idNaoConformidade:string): Observable<CausasNaoConformidadesModel> {
    return this.httpClient.get<CausasNaoConformidadesModel>(`${this.endpoint}/${idNaoConformidade}/causas/${id}`);
  }

  public getList(input: VsGridGetInput, idNaoConformidade:string, idDefeito:string, usarIdDefeito: boolean): Observable<IPagedResultOutputDto<CausasNaoConformidadesModel>> {
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

    httpParams = httpParams.append('usarIdDefeito', String(usarIdDefeito));

    return this.httpClient.get<IPagedResultOutputDto<CausasNaoConformidadesModel>>(`${this.endpoint}/${idNaoConformidade}/defeitos/${idDefeito}/causas`, { params: httpParams });
  }

  public create(input: CausasNaoConformidadesInput, idNaoConformidade: string): Observable<CausasNaoConformidadesModel> {
    return this.httpClient.post<CausasNaoConformidadesModel>(`${this.endpoint}/${idNaoConformidade}/causas`, input);
  }

  public update(id: string, idNaoConformidade: string, input: CausasNaoConformidadesInput): Observable<CausasNaoConformidadesModel> {
    return this.httpClient.put<CausasNaoConformidadesModel>(`${this.endpoint}/${idNaoConformidade}/causas/${id}`, input);
  }

  public delete(id: string, idNaoConformidade: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.endpoint}/${idNaoConformidade}/causas/${id}`);
  }
}
