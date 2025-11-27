import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IBaseGetInput, IPagedResultOutputDto, JQQBRule, JQQBRuleSet, JQQB_COND_AND, JQQB_OP_EQUAL, VsFilterManager, VsFilterTypeEnum, VsReadOnlyBaseService } from '@viasoft/common';
import { ensureTrailingSlash } from '@viasoft/http';
import { EstoqueLocalOutput } from '@viasoft/rnc/api-clients/Estoque-Locais/model/estoque-local-output';
import { Observable } from 'rxjs';

@Injectable()
export class EstoqueLocalService extends VsReadOnlyBaseService<EstoqueLocalOutput> {
  public idProduto: string;
  public numeroLote: string;
  private readonly endpoint: string;

  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    super();
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/gateway/estoque-locais`;
  }

  public getAll(input: IBaseGetInput): Observable<IPagedResultOutputDto<EstoqueLocalOutput>> {
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

    return this.httpClient.get(this.endpoint, { params: httpParams });
  }
  public getById(id: string): Observable<EstoqueLocalOutput> {
    return this.httpClient.get<EstoqueLocalOutput>(`${this.endpoint}/${id}`);
  }
  private applyDefaultFilters(advancedFilter: string) {
    const defaultFilter = {
      condition: JQQB_COND_AND.condition,
      rules: [
        {
          field: 'IsLocalBloquearMovimentacao',
          operator: JQQB_OP_EQUAL.operator,
          type: 'boolean',
          value: 'true'
        } as JQQBRule
      ]
    } as JQQBRuleSet;

    if (this.idProduto) {
      defaultFilter.rules.push({
        field: 'IdProduto',
        operator: JQQB_OP_EQUAL.operator,
        type: 'string',
        value: this.idProduto
      } as JQQBRule);
    }

    if (this.numeroLote) {
      defaultFilter.rules.push({
        field: 'Lote',
        operator: JQQB_OP_EQUAL.operator,
        type: 'string',
        value: this.numeroLote
      } as JQQBRule);
    }

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
