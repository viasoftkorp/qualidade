import {
  Component,
  EventEmitter,
  Input,
  Output,
  forwardRef
} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import {
  VsAutocompleteGetInput,
  VsAutocompleteGetNameFn,
  VsAutocompleteOutput,
  VsFormManipulator,
  VsGridGetInput
} from '@viasoft/components';
import { map } from 'rxjs/operators';
import { KeyValue } from '@angular/common';
import { Observable } from 'rxjs';
import {
  IPagedResultOutputDto,
  JQQBRule,
  JQQBRuleSet,
  JQQB_OP_EQUAL,
  VsFilterManager,
  VsFilterTypeEnum,
  JQQB_COND_AND
} from '@viasoft/common';
import { NotasFiscaisSaidaService } from '../notas-fiscais-saida.service';
import { NotaFiscalSaidaOutput } from '../../../api-clients/Nota-Fiscal-Saida/model';

@Component({
  selector: 'rnc-notas-fiscais-saida-autocomplete-select',
  templateUrl: './notas-fiscais-saida-autocomplete-select.component.html',
  styleUrls: ['./notas-fiscais-saida-autocomplete-select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      // eslint-disable-next-line no-use-before-define
      useExisting: forwardRef(() => NotasFiscaisSaidaAutocompleteSelectComponent),
      multi: true,
    },
  ],
})
export class NotasFiscaisSaidaAutocompleteSelectComponent extends VsFormManipulator<string> {
  @Input() public placeholder: string;
  @Input() public required = false;
  @Input() public disabled = false;
  @Input() public idCliente: string;
  @Output() public loaded = new EventEmitter<NotaFiscalSaidaOutput>();
  @Output() public optionSelected = new EventEmitter<string>();
  @Output() public cleared = new EventEmitter();

  constructor(private service: NotasFiscaisSaidaService) {
    super();
  }

  public getNames: VsAutocompleteGetNameFn<string> = (value) => this.service.get(value).pipe(
    map((NotaFiscalSaida: NotaFiscalSaidaOutput) => {
      this.loaded.emit(NotaFiscalSaida);
      return `${NotaFiscalSaida.numeroNotaFiscal}`;
    })
  );
  public opcaoSelecionada(option: KeyValue<string, string>):void {
    this.optionSelected.emit(option.value);
  }

  public getAutocompleteItems = (input: VsAutocompleteGetInput): Observable<VsAutocompleteOutput<string>> => this.service
    .getList({
      maxResultCount: input.maxDropSize,
      skipCount: input.skipCount,
      filter: input.valueToFilter,
      sorting: '',
      advancedFilter: this.getAdvancedFilter(input),
    } as VsGridGetInput,
    this.idCliente)
    .pipe(
      map(
        (pagedResult: IPagedResultOutputDto<NotaFiscalSaidaOutput>) => ({
          totalCount: pagedResult.totalCount,
          items: pagedResult.items.map((NotaFiscalSaida: NotaFiscalSaidaOutput) => ({
            name: `${NotaFiscalSaida.numeroNotaFiscal}`,
            value: NotaFiscalSaida.id,
          })),
        } as VsAutocompleteOutput<string>)
      )
    );

  private getAdvancedFilter(input: VsAutocompleteGetInput) {
    const filter = new VsFilterManager();
    const rules = new Array<JQQBRule>();

    rules.push({
      field: 'numeroNotaFiscal',
      value: '',
      operator: 'is_not_null',
      type: 'integer'
    });

    if (input.valueToFilter) {
      rules.push({
        field: 'numeroNotaFiscal',
        value: input.valueToFilter,
        operator: JQQB_OP_EQUAL.operator,
        type: 'integer'
      });
    }

    filter.currentFilter = {
      description: {
        condition: JQQB_COND_AND.condition,
        rules: rules
      } as JQQBRuleSet
    };

    return JSON.stringify(filter.getAdvancedFilter(VsFilterTypeEnum.JQQB));
  }
}
