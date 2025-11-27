import { KeyValue } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  Output,
  forwardRef
} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import {
  VsSubscriptionManager,
  IPagedResultOutputDto,
  VsFilterManager,
  JQQB_COND_OR,
  JQQB_OP_EQUAL,
  JQQBRule,
  JQQBRuleSet,
  VsFilterTypeEnum
} from '@viasoft/common';
import {
  VsAutocompleteGetNameFn,
  VsAutocompleteGetInput,
  VsAutocompleteOutput,
  VsGridGetInput,
  VsFormManipulator
} from '@viasoft/components';
import { PedidoVendaOutput } from '@viasoft/rnc/api-clients/Pedido-Venda/model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PedidoVendaService } from '../pedido-venda.service';

@Component({
  selector: 'rnc-pedido-venda-autocomplete-select',
  templateUrl: './pedido-venda-autocomplete-select.component.html',
  styleUrls: ['./pedido-venda-autocomplete-select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      // eslint-disable-next-line no-use-before-define
      useExisting: forwardRef(() => PedidoVendaAutocompleteSelectComponent),
      multi: true,
    },
  ]
})
export class PedidoVendaAutocompleteSelectComponent extends VsFormManipulator<string> implements OnDestroy {
  @Input() public placeholder: string;
  @Input() public required = false;
  @Input() public disabled = false;
  @Input() public readonly = false;
  @Output() public loaded = new EventEmitter<PedidoVendaOutput>();
  @Output() public optionSelected = new EventEmitter<string>();
  @Output() public cleared = new EventEmitter();

  private subs: VsSubscriptionManager = new VsSubscriptionManager();

  constructor(private service: PedidoVendaService) {
    super();
  }

  public ngOnDestroy(): void {
    this.subs.clear();
  }
  public getNames: VsAutocompleteGetNameFn<string> = (value) => this.service.get(value).pipe(
    map((pedidoVenda: PedidoVendaOutput) => {
      this.loaded.emit(pedidoVenda);
      return `${pedidoVenda.numeroPedido}`;
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
    } as VsGridGetInput)
    .pipe(
      map(
        (pagedResult: IPagedResultOutputDto<PedidoVendaOutput>) => ({
          totalCount: pagedResult.totalCount,
          items: pagedResult.items.map((pedidoVenda: PedidoVendaOutput) => ({
            name: `${pedidoVenda.numeroPedido}`,
            value: pedidoVenda.id,
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
            field: 'NumeroPedido',
            value: input.valueToFilter,
            operator: JQQB_OP_EQUAL.operator,
            type: 'string'
          } as JQQBRule,
        ]
      } as JQQBRuleSet
    };
    return JSON.stringify(filter.getAdvancedFilter(VsFilterTypeEnum.JQQB));
  }
}
