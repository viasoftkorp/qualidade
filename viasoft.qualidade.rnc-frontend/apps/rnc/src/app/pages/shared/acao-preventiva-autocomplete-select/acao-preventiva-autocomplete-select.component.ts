/* eslint-disable no-use-before-define */
/* eslint-disable import/no-cycle */
/* eslint-disable max-classes-per-file */

import {
  Component, EventEmitter, forwardRef, Input, OnDestroy, Output
} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

import { map } from 'rxjs/operators';

import {
  IPagedResultOutputDto,
  JQQB_COND_OR,
  JQQB_OP_CONTAINS,
  JQQB_OP_EQUAL,
  JQQBRule,
  JQQBRuleSet,
  VsFilterManager,
  VsFilterTypeEnum,
  VsSubscriptionManager
} from '@viasoft/common';
import {
  VsAutocompleteGetInput,
  VsAutocompleteGetNameFn,
  VsAutocompleteOutput,
  VsFormManipulator,
  VsGridGetInput
} from '@viasoft/components';

import { Observable } from 'rxjs';
// eslint-disable-next-line max-len
import { AcaoPreventivaOutput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Acoes-Preventivas-Nao-Conformidades/model/acao-preventiva-output';
import { StringUtils } from '@viasoft/rnc/utils/stringUtils';
// eslint-disable-next-line max-len
import { AcaoPreventivaAutocompleteSelectService } from './acao-preventiva-provider/acao-preventiva-autocomplete-select.service';

@Component({
  selector: 'qa-acao-preventiva-autocomplete-select',
  templateUrl: './acao-preventiva-autocomplete-select.component.html',
  styleUrls: ['./acao-preventiva-autocomplete-select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AcaoPreventivaAutocompleteSelectComponent),
      multi: true
    }
  ]
})
export class AcaoPreventivaAutocompleteSelectComponent extends VsFormManipulator<string> implements OnDestroy {
  @Input() public placeholder: string;
  @Input() public required = false;
  @Input() public disabled = false;
  @Output() public loaded = new EventEmitter<AcaoPreventivaOutput>();

  private subs: VsSubscriptionManager = new VsSubscriptionManager();

  constructor(
    private service: AcaoPreventivaAutocompleteSelectService,
  ) {
    super();
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }
  public getNames: VsAutocompleteGetNameFn<string> =
  (value) => this.service.get(value)
    .pipe(map((acaoPreventiva:AcaoPreventivaOutput) => {
      this.loaded.emit(acaoPreventiva);
      return `${acaoPreventiva.codigo} - ${acaoPreventiva.descricao}`;
    }));

  public getAutocompleteItems =
  (input: VsAutocompleteGetInput): Observable<VsAutocompleteOutput<string>> => this.service.getList(
    {
      maxResultCount: input.maxDropSize,
      skipCount: input.skipCount,
      filter: input.valueToFilter,
      sorting: '',
      advancedFilter: this.getAdvancedFilter(input)
    } as VsGridGetInput
  ).pipe(
    map((pagedResult: IPagedResultOutputDto<AcaoPreventivaOutput>) => ({
      totalCount: pagedResult.totalCount,
      items: pagedResult.items.map((acaoPreventiva: AcaoPreventivaOutput) => ({
        name: `${acaoPreventiva.codigo} - ${acaoPreventiva.descricao}`,
        value: acaoPreventiva.id
      }))
    } as VsAutocompleteOutput<string>))
  )
  private getAdvancedFilter(input: VsAutocompleteGetInput) {
    const defaultFilters = this.getDefaultFilters();

    const filter = new VsFilterManager();
    filter.currentFilter.defaultFilters = defaultFilters;

    if (input.valueToFilter) {
      filter.currentFilter.valueToFilter = {
        condition: JQQB_COND_OR.condition,
        rules: [
          {
            field: 'Descricao',
            value: input.valueToFilter,
            operator: JQQB_OP_CONTAINS.operator,
            type: 'string'
          } as JQQBRule
        ]
      } as JQQBRuleSet;

      if (StringUtils.isNumber(input.valueToFilter)) {
        filter.currentFilter.valueToFilter.rules.push({
          field: 'Codigo',
          value: input.valueToFilter,
          operator: JQQB_OP_EQUAL.operator,
          type: 'integer'
        } as JQQBRule);
      }
    }

    return JSON.stringify(filter.getAdvancedFilter(VsFilterTypeEnum.JQQB));
  }
  private getDefaultFilters():JQQBRuleSet {
    const defaultFilters = {
      condition: JQQB_COND_OR.condition,
      rules: [
        {
          field: 'isAtivo',
          value: 'true',
          operator: JQQB_OP_EQUAL.operator,
          type: 'boolean'
        } as JQQBRule,
      ]
    } as JQQBRuleSet;
    return defaultFilters;
  }
}
