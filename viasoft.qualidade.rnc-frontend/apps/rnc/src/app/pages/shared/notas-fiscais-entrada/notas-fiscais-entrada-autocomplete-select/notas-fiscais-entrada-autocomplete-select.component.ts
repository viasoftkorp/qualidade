import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild,
  forwardRef
} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { KeyValue } from '@angular/common';
import {
  VsAutocompleteGetInput,
  VsAutocompleteGetNameFn,
  VsAutocompleteOutput,
  VsAutocompleteSelectComponent,
  VsFormManipulator,
  VsGridGetInput
} from '@viasoft/components';
import { NotaFiscalEntradaOutput } from '@viasoft/rnc/api-clients/Nota-Fiscal-Entrada/model';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import {
  IPagedResultOutputDto,
  JQQBRule,
  JQQBRuleSet,
  JQQB_COND_OR,
  JQQB_OP_EQUAL,
  VsFilterManager,
  VsFilterTypeEnum
} from '@viasoft/common';
import { NotasFiscaisEntradaService } from '../notas-fiscais-entrada.service';

@Component({
  selector: 'rnc-notas-fiscais-entrada-autocomplete-select',
  templateUrl: './notas-fiscais-entrada-autocomplete-select.component.html',
  styleUrls: ['./notas-fiscais-entrada-autocomplete-select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      // eslint-disable-next-line no-use-before-define
      useExisting: forwardRef(() => NotasFiscaisEntradaAutocompleteSelectComponent),
      multi: true,
    },
  ],
})
export class NotasFiscaisEntradaAutocompleteSelectComponent extends VsFormManipulator<string> implements AfterViewInit {
  @Input() public placeholder: string;
  @Input() public required = false;
  @Input() public disabled = false;
  @Input() public idFornecedor:string;
  @Output() public loaded = new EventEmitter<NotaFiscalEntradaOutput>();
  @Output() public optionSelected = new EventEmitter<string>();
  @Output() public cleared = new EventEmitter();
  @ViewChild('autoComplete') public autoComplete: VsAutocompleteSelectComponent;

  constructor(private service: NotasFiscaisEntradaService) {
    super();
  }

  public ngAfterViewInit(): void {
    (this.autoComplete?.inputRef.nativeElement).type = 'number';
  }

  public getNames: VsAutocompleteGetNameFn<string> = (value) => this.service.get(value).pipe(
    map((NotaFiscalEntrada: NotaFiscalEntradaOutput) => {
      this.loaded.emit(NotaFiscalEntrada);
      return `${NotaFiscalEntrada.numeroNotaFiscal}`;
    })
  );
  public opcaoSelecionada(option:KeyValue<string, string>):void {
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
    this.idFornecedor)
    .pipe(
      map(
        (pagedResult: IPagedResultOutputDto<NotaFiscalEntradaOutput>) => ({
          totalCount: pagedResult.totalCount,
          items: pagedResult.items.map((NotaFiscalEntrada: NotaFiscalEntradaOutput) => ({
            name: `${NotaFiscalEntrada.numeroNotaFiscal}`,
            value: NotaFiscalEntrada.id,
          })),
        } as VsAutocompleteOutput<string>)
      )
    );

  private getAdvancedFilter(input: VsAutocompleteGetInput) {
    if (!input.valueToFilter) {
      return '';
    }
    const filter = new VsFilterManager();
    filter.currentFilter = {
      description: {
        condition: JQQB_COND_OR.condition,
        rules: [
          {
            field: 'NumeroNotaFiscal',
            value: input.valueToFilter,
            operator: JQQB_OP_EQUAL.operator,
            type: 'double'
          } as JQQBRule
        ]
      } as JQQBRuleSet
    };
    return JSON.stringify(filter.getAdvancedFilter(VsFilterTypeEnum.JQQB));
  }
}
