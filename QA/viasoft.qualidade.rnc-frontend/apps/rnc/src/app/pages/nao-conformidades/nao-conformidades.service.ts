import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto, JQQBRule, JQQBRuleSet, JQQB_COND_AND, JQQB_OP_EQUAL, VsFilterManager, VsFilterTypeEnum } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { NaoConformidadeInput } from '@viasoft/rnc/api-clients/Nao-Conformidades/model/nao-conformidade-input';
import { NaoConformidadeModel } from '@viasoft/rnc/api-clients/Nao-Conformidades/model/nao-conformidade-model';
import { NaoConformidadeValidationResult } from '@viasoft/rnc/api-clients/Nao-Conformidades/model/nao-conformidade-validation-result';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NaoConformidadesService {
  private readonly endpoint: string;
  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/gateway/nao-conformidades`;
  }

  public get(id: string): Observable<NaoConformidadeModel> {
    return this.httpClient.get<NaoConformidadeModel>(`${this.endpoint}/${id}`);
  }

  public getList(input: VsGridGetInput): Observable<IPagedResultOutputDto<NaoConformidadeModel>> {
    let httpParams = new HttpParams();

    input.advancedFilter = this.applyDefaultFilters(input.advancedFilter);

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
  private applyDefaultFilters(advancedFilter: string) {
    const defaultFilter = {
      condition: JQQB_COND_AND.condition,
      rules: [
        {
          field: 'incompleta',
          operator: JQQB_OP_EQUAL.operator,
          type: 'boolean',
          value: 'false'
        } as JQQBRule
      ]
    } as JQQBRuleSet;

    if (advancedFilter) {
      const deserializedAdvancedFilter = JSON.parse(advancedFilter) as JQQBRuleSet;
      deserializedAdvancedFilter.rules.push(defaultFilter);

      return JSON.stringify(deserializedAdvancedFilter);
    }

    const filter = new VsFilterManager();
    filter.currentFilter.defaultFilter = defaultFilter;

    return JSON.stringify(filter.getAdvancedFilter(VsFilterTypeEnum.JQQB));
  }
}
