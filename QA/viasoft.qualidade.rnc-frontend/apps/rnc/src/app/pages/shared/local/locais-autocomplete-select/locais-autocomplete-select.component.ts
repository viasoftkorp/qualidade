import { KeyValue } from '@angular/common';
import {
  Component,
  forwardRef,
  OnDestroy,
  Input,
  Output,
  EventEmitter,
  OnInit
} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import {
  VsSubscriptionManager,
  IPagedResultOutputDto,
  VsFilterManager,
  JQQB_OP_EQUAL,
  JQQBRule,
  JQQBRuleSet,
  VsFilterTypeEnum,
  JQQB_OP_CONTAINS,
  JQQB_COND_AND,
  JQQB_COND_OR,
} from '@viasoft/common';
import {
  VsFormManipulator,
  VsAutocompleteGetNameFn,
  VsAutocompleteGetInput,
  VsAutocompleteOutput,
  VsGridGetInput,
} from '@viasoft/components';
import { TipoLocal } from '@viasoft/rnc/api-clients/Locais/model/tipo-local';
import { LocalOutput } from '@viasoft/rnc/api-clients/Locais/model/local-output';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { LocalService } from '../local.service';
import { StringUtils } from '@viasoft/rnc/utils/stringUtils';

@Component({
  selector: 'rnc-locais-autocomplete-select',
  templateUrl: './locais-autocomplete-select.component.html',
  styleUrls: ['./locais-autocomplete-select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      // eslint-disable-next-line no-use-before-define
      useExisting: forwardRef(() => LocaisAutocompleteSelectComponent),
      multi: true,
    },
  ],
})
export class LocaisAutocompleteSelectComponent extends VsFormManipulator<string> implements OnDestroy, OnInit {
  @Input() public placeholder: string;
  @Input() public required = false;
  @Input() public disabled = false;
  @Input() public tipoLocal: TipoLocal
  @Input() public autoInputFirstOptionIfHasOnlyOne = false;
  @Output() public loaded = new EventEmitter<LocalOutput>();
  @Output() public optionSelected = new EventEmitter<string>();
  @Output() public cleared = new EventEmitter();

  private subs: VsSubscriptionManager = new VsSubscriptionManager();

  constructor(private service: LocalService) {
    super();
  }
  public ngOnInit(): void {
    if (this.autoInputFirstOptionIfHasOnlyOne) {
      this.trySetFirstLocal();
    }
  }

  public ngOnDestroy(): void {
    this.subs.clear();
  }
  public getNames: VsAutocompleteGetNameFn<string> = (value) => this.service.get(value).pipe(
    map((local: LocalOutput) => {
      this.loaded.emit(local);
      return `${local.codigo} - ${local.descricao}`;
    })
  );
  public opcaoSelecionada(option: KeyValue<string, string>): void {
    this.optionSelected.emit(option.value);
  }

  public getAutocompleteItems = (input: VsAutocompleteGetInput): Observable<VsAutocompleteOutput<string>> => this.service
    .getList({
      maxResultCount: input.maxDropSize,
      skipCount: input.skipCount,
      filter: input.valueToFilter,
      sorting: '',
      advancedFilter: this.getAdvancedFilter(input),
    } as VsGridGetInput)
    .pipe(
      map(
        (pagedResult: IPagedResultOutputDto<LocalOutput>) => ({
          totalCount: pagedResult.totalCount,
          items: pagedResult.items.map((local: LocalOutput) => ({
            name: `${local.codigo} - ${local.descricao}`,
            value: local.id,
          })),
        } as VsAutocompleteOutput<string>)
      )
    );

  private trySetFirstLocal() {
    const filter = {
      skipCount: 0,
      maxResultCount: 2,
      advancedFilter: this.getAdvancedFilter({} as VsAutocompleteGetInput)
    } as VsGridGetInput;

    this.service.getList(filter).subscribe((result: IPagedResultOutputDto<LocalOutput>) => {
      if (result.items.length === 1) {
        const local = result.items[0];
        this.setValue(local.id);
        this.value = local.id;
      }
    });
  }

  private getAdvancedFilter(input: VsAutocompleteGetInput) {
    const filter = new VsFilterManager();
    filter.currentFilter = {};

    if (this.tipoLocal === TipoLocal.Retrabalho) {
      filter.currentFilter.filters = {
        field: 'IsBloquearMovimentacao',
        value: 'true',
        operator: JQQB_OP_EQUAL.operator,
        type: 'boolean'
      } as JQQBRule;
    }

    if (!input.valueToFilter) {
      return JSON.stringify(filter.getAdvancedFilter(VsFilterTypeEnum.JQQB));
    }

    filter.currentFilter.queryFilter = {
      condition: JQQB_COND_OR.condition,
      rules: [
        {
          field: 'Descricao',
          value: input.valueToFilter,
          operator: JQQB_OP_CONTAINS.operator,
          type: 'string',
        } as JQQBRule
      ]
    } as JQQBRuleSet;

    if (StringUtils.isNumber(input.valueToFilter)) {
      const { queryFilter } = filter.currentFilter;

      queryFilter.rules.push({
        field: 'codigo',
        value: input.valueToFilter,
        operator: JQQB_OP_EQUAL.operator,
        type: 'integer',
      } as JQQBRule);
    }

    return JSON.stringify(filter.getAdvancedFilter(VsFilterTypeEnum.JQQB));
  }
}
