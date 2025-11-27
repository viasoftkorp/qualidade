/* eslint-disable import/no-cycle */
/* eslint-disable max-classes-per-file */

import {
  Component, EventEmitter, forwardRef, Input, OnDestroy, Output
} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
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
  VsAutocompleteGetInput, VsAutocompleteGetNameFn,
  VsAutocompleteOutput, VsFormManipulator, VsGridGetInput
} from '@viasoft/components';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { StringUtils } from '@viasoft/rnc/utils/stringUtils';
import { NaturezaAutocompleteSelectService } from './natureza-provider/natureza-autocomplete-select.service';

export class NaturezaOutput {
  public id: string;
  public codigo: string;
  public descricao: string;
}

@Component({
  selector: 'qa-natureza-autocomplete-select',
  templateUrl: './natureza-autocomplete-select.component.html',
  styleUrls: ['./natureza-autocomplete-select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      // eslint-disable-next-line no-use-before-define
      useExisting: forwardRef(() => NaturezaAutocompleteSelectComponent),
      multi: true
    }
  ]
})
export class NaturezaAutocompleteSelectComponent extends VsFormManipulator<string> implements OnDestroy {
  @Input() public placeholder: string;
  @Input() public required = false;
  @Input() public disabled = false;
  @Output() public loaded = new EventEmitter<NaturezaOutput>();

  private subs: VsSubscriptionManager = new VsSubscriptionManager();

  constructor(
    private service: NaturezaAutocompleteSelectService,
  ) {
    super();
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }
  public getNames: VsAutocompleteGetNameFn<string> =
  (value) => this.service.get(value)
    .pipe(map((natureza:NaturezaOutput) => {
      this.loaded.emit(natureza);
      return `${natureza.codigo} - ${natureza.descricao}`;
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
    map((pagedResult: IPagedResultOutputDto<NaturezaOutput>) => ({
      totalCount: pagedResult.totalCount,
      items: pagedResult.items.map((natureza: NaturezaOutput) => ({
        name: `${natureza.codigo} - ${natureza.descricao}`,
        value: natureza.id
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
