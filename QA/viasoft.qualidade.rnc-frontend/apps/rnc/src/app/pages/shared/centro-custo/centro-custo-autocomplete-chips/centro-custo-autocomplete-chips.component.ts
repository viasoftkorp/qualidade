import {
  Component,
  Input,
  OnDestroy,
  forwardRef
} from '@angular/core';
import {
  IPagedResultOutputDto,
  VsFilterManager,
  JQQB_COND_OR,
  JQQB_OP_EQUAL,
  JQQBRule,
  JQQBRuleSet,
  VsFilterTypeEnum,
  VsSubscriptionManager,
  JQQB_OP_CONTAINS
} from '@viasoft/common';
import {
  VsFormManipulator,
  VsAutocompleteGetInput,
  VsGridGetInput,
  VsAutocompleteOptions,
  VsLegacyAutocompleteOutput,
  VsAutocompleteValue,
  VsLegacyAutocompleteOption
} from '@viasoft/components';
import { CentroCustoOutput } from '@viasoft/rnc/api-clients/Centros-Custo/model/centro-custo-output';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  ControlContainer,
  FormBuilder,
  FormGroup,
  NG_VALUE_ACCESSOR
} from '@angular/forms';
import { CentroCustoService } from '../centro-custo.service';

@Component({
  selector: 'rnc-centro-custo-autocomplete-chips',
  templateUrl: './centro-custo-autocomplete-chips.component.html',
  styleUrls: ['./centro-custo-autocomplete-chips.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      // eslint-disable-next-line no-use-before-define
      useExisting: forwardRef(() => CentroCustoAutocompleteChipsComponent),
      multi: true,
    },
  ]
})
export class CentroCustoAutocompleteChipsComponent extends VsFormManipulator<Array<string>> implements OnDestroy {
  @Input() public placeholder = '';
  @Input() public isOnlyAnaliticos = false;
  public internalForm: FormGroup;
  public options: VsAutocompleteOptions;
  public getInput: VsAutocompleteGetInput = { maxDropSize: 5 };
  public parentForm: FormGroup;
  private subscriptionManager = new VsSubscriptionManager();

  constructor(
    private service: CentroCustoService,
    private formBuilder: FormBuilder,
    private controlContainer: ControlContainer
  ) {
    super();
    this.options = new VsAutocompleteOptions();
    this.options.get = (input: VsAutocompleteGetInput) => this.getAutocompleteItems(input);
  }
  public ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public ngOnInit():void {
    this.parentForm = this.controlContainer.control as FormGroup;
    this.setInternalForm();
    this.subscribeToExternalValueChange();
    this.subscribeInternalDescriptionControl();
    this.subscribeToParentFormStatusChange();
  }

  public ngOnChanges():void {
    this.options.disabled = this.disabled;
  }

  private getAutocompleteItems = (input: VsAutocompleteGetInput): Observable<VsLegacyAutocompleteOutput> => this.service
    .getList({
      maxResultCount: input.maxDropSize,
      skipCount: input.skipCount,
      filter: input.valueToFilter,
      sorting: '',
      advancedFilter: this.getAdvancedFilter(input),
    } as VsGridGetInput)
    .pipe(
      map(
        (pagedResult: IPagedResultOutputDto<CentroCustoOutput>) => ({
          totalCount: pagedResult.totalCount,
          items: pagedResult.items.map((centroCusto: CentroCustoOutput) => ({
            option: {
              key: `${centroCusto.codigo} - ${centroCusto.descricao}`,
              value: centroCusto.id
            }
          } as VsAutocompleteValue)),
        } as VsLegacyAutocompleteOutput)
      )
    );

  private setInternalForm(): void {
    this.internalForm = this.formBuilder.group({});
    this.internalForm.addControl('internalDescriptionControl', this.formBuilder.control([]));
  }

  private getAdvancedFilter(input: VsAutocompleteGetInput) {
    const filter = new VsFilterManager();
    filter.currentFilter = {
    };
    if (this.isOnlyAnaliticos) {
      filter.currentFilter.analiticosFilter = {
        condition: JQQB_COND_OR.condition,
        rules: [
          {
            field: 'IsSintetico',
            value: 'false',
            operator: JQQB_OP_EQUAL.operator,
            type: 'boolean'
          } as JQQBRule,
        ]
      } as JQQBRuleSet;
    }

    if (input.valueToFilter) {
      filter.currentFilter.description = {
        condition: JQQB_COND_OR.condition,
        rules: [
          {
            field: 'Codigo',
            value: input.valueToFilter,
            operator: JQQB_OP_CONTAINS.operator,
            type: 'string'
          } as JQQBRule,
          {
            field: 'Descricao',
            value: input.valueToFilter,
            operator: JQQB_OP_CONTAINS.operator,
            type: 'string'
          } as JQQBRule,
        ]
      } as JQQBRuleSet;
    }

    return JSON.stringify(filter.getAdvancedFilter(VsFilterTypeEnum.JQQB));
  }

  private subscribeToExternalValueChange() {
    this.subscriptionManager.add('on-value-change-subscription', this.onValueChange
      .subscribe((change) => {
        if (!change || change.internalChange || !change.newValue) {
          return;
        }
        const centrosCusto = change.newValue;
        if (centrosCusto.length) {
          const input = {
            maxResultCount: this.value.length,
            skipCount: 0,
            advancedFilter: this.getInitialValueAdvancedFilter()
          } as VsGridGetInput;
          this.subscriptionManager.add('get-list', this.service.getList(input)
            .pipe(map(
              (pagedResult: IPagedResultOutputDto<CentroCustoOutput>) => (
                pagedResult.items.map((centroCusto: CentroCustoOutput) => ({
                  key: `${centroCusto.codigo} - ${centroCusto.descricao}`,
                  value: centroCusto.id
                } as VsLegacyAutocompleteOption))
              )
            ))
            .subscribe((autoCompleteOptions: Array<VsLegacyAutocompleteOption>) => {
              this.internalForm.get('internalDescriptionControl').setValue(autoCompleteOptions);
            }));
        }
      }));
  }

  private getInitialValueAdvancedFilter() {
    const filter = new VsFilterManager();
    filter.currentFilter = {
      description: {
        condition: JQQB_COND_OR.condition,
        rules: [
          {
            field: 'Id',
            value: this.value.toString(),
            operator: 'In',
            type: 'string'
          } as JQQBRule,
        ]
      } as JQQBRuleSet
    };
    return JSON.stringify(filter.getAdvancedFilter(VsFilterTypeEnum.JQQB));
  }

  private subscribeInternalDescriptionControl():void {
    this.subscriptionManager.add('internalDescriptionControlValueChanges',
      this.internalForm.get('internalDescriptionControl').valueChanges.subscribe((value: Array<string>) => {
        this.value = value;
      }));
  }
  private subscribeToParentFormStatusChange() {
    this.subscriptionManager.add('parent-form-status-change', this.parentForm.statusChanges.subscribe(() => {
      this.options.disabled = this.disabled;
    }));
  }
}
