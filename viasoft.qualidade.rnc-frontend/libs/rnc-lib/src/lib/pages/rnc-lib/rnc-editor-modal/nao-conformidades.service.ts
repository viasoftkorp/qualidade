import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { NaoConformidadeInput } from '../../../api-clients/Nao-Conformidades/model/nao-conformidade-input';
import { NaoConformidadeModel } from '../../../api-clients/Nao-Conformidades/model/nao-conformidade-model';
import { Observable } from 'rxjs';
import { NaoConformidadeAgregacaoOutput } from '../../../api-clients/Nao-Conformidades/model/nao-conformidade-agregacao-output';
import { NaoConformidadeValidationResult } from '../../../api-clients/Nao-Conformidades/model/nao-conformidade-validation-result';

@Injectable({
  providedIn: 'root'
})
export class NaoConformidadesService {
  private readonly endpoint: string;
  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/core/nao-conformidades`;
  }

  public getAgregacao(id: string): Observable<NaoConformidadeAgregacaoOutput> {
    return this.httpClient.get<NaoConformidadeAgregacaoOutput>(`${ensureTrailingSlash(this.gateway)}qualidade/rnc/gateway/nao-conformidades/agregacoes/${id}`);
  }

  public get(id: string): Observable<NaoConformidadeModel> {
    return this.httpClient.get<NaoConformidadeModel>(`${this.endpoint}/${id}`);
  }

  public getList(input: VsGridGetInput): Observable<IPagedResultOutputDto<NaoConformidadeModel>> {
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

    return this.httpClient.get<IPagedResultOutputDto<NaoConformidadeModel>>(this.endpoint, { params: httpParams });
  }

  public create(input: NaoConformidadeInput): Observable<NaoConformidadeValidationResult> {
    return this.httpClient.post<NaoConformidadeValidationResult>(this.endpoint, input);
  }

  public update(idNaoconformidade: string, input: NaoConformidadeInput): Observable<NaoConformidadeValidationResult> {
    return this.httpClient.put<NaoConformidadeValidationResult>(`${this.endpoint}/${idNaoconformidade}`, input);
  }

  public delete(idNaoconformidade: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.endpoint}/${idNaoconformidade}`);
  }
}
