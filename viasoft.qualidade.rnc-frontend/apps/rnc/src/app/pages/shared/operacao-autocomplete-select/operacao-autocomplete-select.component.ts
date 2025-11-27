/* eslint-disable import/no-cycle */
/* eslint-disable max-classes-per-file */

import {
  Component, EventEmitter, forwardRef, Input, OnDestroy, Output
} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import {
  IPagedResultOutputDto,
  JQQB_COND_AND,
  JQQB_COND_OR,
  JQQB_OP_CONTAINS,
  JQQB_OP_EQUAL,
  JQQB_OP_NOT_BEGINS_WITH,
  JQQB_OP_NOT_EQUAL,
  JQQBRule,
  JQQBRuleSet,
  VsFilterManager,
  VsFilterTypeEnum,
  VsSubscriptionManager
} from '@viasoft/common';
import {
  VsAutocompleteGetInput, VsAutocompleteGetNameFn,
  VsAutocompleteOutput, VsFormManipulator, VsGridGetInput
} from '@viasoft/components';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { StringUtils } from '@viasoft/rnc/utils/stringUtils';
import { VsAutocompleteOption } from '@viasoft/components/autocomplete/src/shared/autocomplete-options';
import { OperacaoProxyService } from '@viasoft/rnc/api-clients/Producao/Apontamento/Operacoes/api/operacao-proxy.service';
import { ApontamentoOperacaoOutput } from '@viasoft/rnc/api-clients/Producao/Apontamento/Operacoes/models/apontamento-operacao-output';
import { OdfConsts } from '@viasoft/rnc/app/tokens/consts/odf-consts';

@Component({
  selector: 'qa-operacao-autocomplete-select',
  templateUrl: './operacao-autocomplete-select.component.html',
  styleUrls: ['./operacao-autocomplete-select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      // eslint-disable-next-line no-use-before-define
      useExisting: forwardRef(() => OperacaoAutocompleteSelectComponent),
      multi: true
    }
  ]
})
export class OperacaoAutocompleteSelectComponent extends VsFormManipulator<string> implements OnDestroy {
  @Input() public placeholder: string;
  @Input() public required = false;
  @Input() public disabled = false;
  @Input() public numeroOdf: number;
  @Input() public filtrarOdfsNaoFinal = false;
  @Input() public filtrarOdfNaoRetrabalho = false;
  @Output() public loaded = new EventEmitter<ApontamentoOperacaoOutput>();
  @Output() public selected = new EventEmitter<ApontamentoOperacaoOutput>();

  private subs: VsSubscriptionManager = new VsSubscriptionManager();

  constructor(
    private service: OperacaoProxyService,
  ) {
    super();
  }

  public ngOnDestroy(): void {
    this.subs.clear();
  }

  public getNames: VsAutocompleteGetNameFn<string> =
    (value) => this.service.get(value)
      .pipe(map((operacao: ApontamentoOperacaoOutput) => {
        this.loaded.emit(operacao);
        return `${operacao.operacao} - ${operacao.descricaoMaquina}`;
      }));

  public getAutocompleteItems =
    (input: VsAutocompleteGetInput): Observable<VsAutocompleteOutput<string>> => this.service.getList(
      {
        maxResultCount: input.maxDropSize,
        skipCount: input.skipCount,
        filter: input.valueToFilter,
        sorting: '',
        advancedFilter: this.getAdvancedFilter(input)
      } as VsGridGetInput,
      this.numeroOdf
    ).pipe(
      map((pagedResult: IPagedResultOutputDto<ApontamentoOperacaoOutput>) => ({
        totalCount: pagedResult.totalCount,
        items: pagedResult.items.map((operacao: ApontamentoOperacaoOutput) => ({
          name: `${operacao.operacao} - ${operacao.descricaoMaquina}`,
          value: operacao.operacao.toString(),
          customData: operacao
        }))
      } as VsAutocompleteOutput<string>))
    )

  public onOptionSelected(autocompleteOption: VsAutocompleteOption<string, ApontamentoOperacaoOutput>) {
    this.selected.emit(autocompleteOption.customData);
  }

  public onOptionCleared() {
    this.selected.emit(null);
  }

  private getAdvancedFilter(input: VsAutocompleteGetInput) {
    const defaultFilters = this.getDefaultFilters();

    const filter = new VsFilterManager();
    filter.currentFilter.defaultFilters = defaultFilters;

    if (input.valueToFilter) {
      filter.currentFilter.valueToFilter = {
        condition: JQQB_COND_OR.condition,
        rules: [
          {
            field: 'DESMAQ',
            value: input.valueToFilter,
            operator: JQQB_OP_CONTAINS.operator,
            type: 'string'
          } as JQQBRule
        ]
      } as JQQBRuleSet;

      if (StringUtils.isNumber(input.valueToFilter)) {
        filter.currentFilter.valueToFilter.rules.push({
          field: 'NUMOPE',
          value: input.valueToFilter,
          operator: JQQB_OP_EQUAL.operator,
          type: 'integer'
        } as JQQBRule);
      }
    }

    return JSON.stringify(filter.getAdvancedFilter(VsFilterTypeEnum.JQQB));
  }

  private getDefaultFilters(): JQQBRuleSet {
    const defaultFilters = {
      condition: JQQB_COND_AND.condition,
      rules: []
    } as JQQBRuleSet;
    if (this.filtrarOdfsNaoFinal) {
      defaultFilters.rules.push({
        operator: JQQB_OP_NOT_EQUAL.operator,
        value: OdfConsts.NumeroOperacaoFinal.toString(),
        field: 'NUMOPE',
        type: 'number'
      }as JQQBRule);
    }

    if (this.filtrarOdfNaoRetrabalho) {
      defaultFilters.rules.push({
        operator: JQQB_OP_NOT_BEGINS_WITH.operator,
        value: 'Op.Retrabalho',
        field: 'DESCRICAO_OPERACAO',
        type: 'string'
      }as JQQBRule);
    }

    return defaultFilters;
  }
}
