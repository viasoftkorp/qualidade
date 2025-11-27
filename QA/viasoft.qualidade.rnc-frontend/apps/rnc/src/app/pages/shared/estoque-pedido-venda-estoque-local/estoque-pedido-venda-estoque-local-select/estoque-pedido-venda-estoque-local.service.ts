import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IBaseGetInput, IPagedResultOutputDto, JQQBRule, JQQBRuleSet, JQQB_COND_AND, JQQB_OP_EQUAL, VsFilterManager, VsFilterTypeEnum, VsReadOnlyBaseService } from '@viasoft/common';
import { ensureTrailingSlash } from '@viasoft/http';
import { Observable } from 'rxjs';
// eslint-disable-next-line max-len
import { EstoquePedidoVendaEstoqueLocalViewOutput } from '../../../../../api-clients/Estoque-Pedido-Venda-Estoque-Locais/model/estoque-pedido-venda-estoque-local-view-output'

@Injectable()
export class EstoquePedidoVendaEstoqueLocalService extends VsReadOnlyBaseService<EstoquePedidoVendaEstoqueLocalViewOutput> {
  private readonly endpoint: string;
  public idProduto:string;
  public numeroLote:string;
  public numeroPedido:string;
  public numeroOdf:string;

  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    super();
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/gateway/estoque-pedido-venda-estoque-local-views`;
  }

  public getAll(input: IBaseGetInput): Observable<IPagedResultOutputDto<EstoquePedidoVendaEstoqueLocalViewOutput>> {
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
  public getById(id: string): Observable<EstoquePedidoVendaEstoqueLocalViewOutput> {
    return this.httpClient.get<EstoquePedidoVendaEstoqueLocalViewOutput>(`${this.endpoint}/${id}`);
  }

  private applyDefaultFilters(advancedFilter: string) {
    const defaultFilter = {
      condition: JQQB_COND_AND.condition,
      rules: [
        {
          field: 'IsLocalBloquearMovimentacao',
          operator: JQQB_OP_EQUAL.operator,
          type: 'boolean',
          value: 'false'
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

    if (this.numeroPedido) {
      defaultFilter.rules.push({
        field: 'NumeroPedido',
        operator: JQQB_OP_EQUAL.operator,
        type: 'string',
        value: this.numeroPedido
      } as JQQBRule);
    }

    if (this.numeroLote) {
      defaultFilter.rules.push({
        field: 'NumeroLote',
        operator: JQQB_OP_EQUAL.operator,
        type: 'string',
        value: this.numeroLote
      } as JQQBRule);
    }

    if (this.numeroOdf) {
      defaultFilter.rules.push({
        field: 'NumeroOdf',
        operator: JQQB_OP_EQUAL.operator,
        type: 'integer',
        value: this.numeroOdf
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
