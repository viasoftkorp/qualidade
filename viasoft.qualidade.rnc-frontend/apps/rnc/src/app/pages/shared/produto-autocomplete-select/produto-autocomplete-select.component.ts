/* eslint-disable no-use-before-define */
/* eslint-disable import/no-cycle */
/* eslint-disable max-classes-per-file */

import {
  Component, EventEmitter, forwardRef, Input, OnDestroy, OnInit, Output
} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

import { map } from 'rxjs/operators';

import {
  IPagedResultOutputDto,
  JQQBRule,
  JQQBRuleSet,
  JQQB_COND_OR,
  JQQB_OP_CONTAINS,
  VsFilterManager,
  VsFilterTypeEnum,
  VsSubscriptionManager
} from '@viasoft/common';
import {
  VsAutocompleteGetInput,
  VsAutocompleteGetNameFn,
  VsAutocompleteOption,
  VsAutocompleteOutput,
  VsFormManipulator,
  VsGridGetInput
} from '@viasoft/components';
import { Observable } from 'rxjs';
import { ProdutoAutocompleteSelectService } from './produto-provider/produto-autocomplete-select.service';

export class ProdutoOutput {
  public id: string;
  public codigo: string;
  public descricao: string;
  public idCategoria: string;
}

@Component({
  selector: 'qa-produto-autocomplete-select',
  templateUrl: './produto-autocomplete-select.component.html',
  styleUrls: ['./produto-autocomplete-select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ProdutoAutocompleteSelectComponent),
      multi: true
    }
  ]
})
export class ProdutoAutocompleteSelectComponent extends VsFormManipulator<string> implements OnInit, OnDestroy {
  @Input() public placeholder: string;
  @Input() public required = false;
  @Input() public disabled = false;
  @Input() public readonly = false;
  @Output() public loaded = new EventEmitter<ProdutoOutput>();
  @Output() public optionSelected = new EventEmitter<VsAutocompleteOption<string, string>>();
  @Output() public cleared = new EventEmitter();
  @Input() public isService: boolean;
  private subs: VsSubscriptionManager = new VsSubscriptionManager();
  private codigoCategoria: string;

  constructor(private service: ProdutoAutocompleteSelectService) {
    super();
  }

  public ngOnInit(): void {
    this.checkService();
  }

  public ngOnDestroy(): void {
    this.subs.clear();
  }
  public getNames: VsAutocompleteGetNameFn<string> =
  (value) => this.service.get(value)
    .pipe(map((produto:ProdutoOutput) => {
      this.loaded.emit(produto);
      return `${produto.codigo} - ${produto.descricao}`;
    }));

  public getAutocompleteItems =
  (input: VsAutocompleteGetInput): Observable<VsAutocompleteOutput<string>> => this.service.getListPageless(
    {
      maxResultCount: input.maxDropSize,
      skipCount: input.skipCount,
      filter: input.valueToFilter,
      sorting: '',
      advancedFilter: this.getAdvancedFilter(input)
    } as VsGridGetInput, this.codigoCategoria
  ).pipe(
    map((pagedResult: IPagedResultOutputDto<ProdutoOutput>) => {
      // FakeTotalCount usado para autocomplete funcionar com get produtos sem totalCount
      let fakeTotalCount = pagedResult.items.length;

      if (pagedResult.items.length >= input.maxDropSize) {
        fakeTotalCount = input.skipCount + input.maxDropSize + 1;
      }

      return {
        totalCount: fakeTotalCount,
        items: pagedResult.items.map((produto: ProdutoOutput) => ({
          name: `${produto.codigo} - ${produto.descricao}`,
          value: produto.id
        }))
      } as VsAutocompleteOutput<string>;
    })
  )

  public optionSelectedHandle(optionSelected: VsAutocompleteOption<string, string>):void {
    this.optionSelected.emit(optionSelected);
  }

  public clearedHandle():void {
    this.cleared.emit();
  }

  private checkService(): void {
    if (this.isService === true) {
      this.codigoCategoria = '30';
    }
  }
  private getAdvancedFilter(input: VsAutocompleteGetInput):string {
    const filter = new VsFilterManager();

    if (input.valueToFilter) {
      filter.currentFilter = {
        description: {
          condition: JQQB_COND_OR.condition,
          rules: [
            {
              field: 'Description',
              value: input.valueToFilter ?? '',
              operator: JQQB_OP_CONTAINS.operator,
              type: 'string'
            } as JQQBRule,
            {
              field: 'Code',
              value: input.valueToFilter ?? '',
              operator: JQQB_OP_CONTAINS.operator,
              type: 'string'
            } as JQQBRule,
          ]
        } as JQQBRuleSet
      };
    }

    if (this.codigoCategoria) {
      filter.currentFilter
        .codigoCategoriaFiltro = {
          condition: JQQB_COND_OR.condition,
          rules: [
            {
              field: 'LegacyCategoryCode',
              value: this.codigoCategoria,
              operator: JQQB_OP_CONTAINS.operator,
              type: 'string'
            } as JQQBRule,
          ]
        } as JQQBRuleSet;
    }
    return JSON.stringify(filter.getAdvancedFilter(VsFilterTypeEnum.JQQB));
  }
}
