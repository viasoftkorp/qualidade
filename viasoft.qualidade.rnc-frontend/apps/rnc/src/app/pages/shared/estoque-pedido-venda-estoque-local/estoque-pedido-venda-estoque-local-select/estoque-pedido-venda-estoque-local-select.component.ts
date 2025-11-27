import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  forwardRef
} from '@angular/core';
import { NG_VALUE_ACCESSOR, FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import {
  IBaseGetInput,
  IPagedResultOutputDto,
  JQQB_NUMBER_OPERATORS,
  VsSubscriptionManager
} from '@viasoft/common';
import {
  VsFormManipulator,
  VsSelectModalComponent,
  IVsSelectModalData,
  VsGridOptions,
  VsGridSimpleColumn,
  VsGridDateColumn
} from '@viasoft/components';
import { EstoquePedidoVendaEstoqueLocalViewOutput } from '@viasoft/rnc/api-clients/Estoque-Pedido-Venda-Estoque-Locais';
import { EstoquePedidoVendaEstoqueLocalService } from './estoque-pedido-venda-estoque-local.service';

@Component({
  selector: 'rnc-estoque-pedido-venda-estoque-local-select',
  templateUrl: './estoque-pedido-venda-estoque-local-select.component.html',
  styleUrls: ['./estoque-pedido-venda-estoque-local-select.component.scss'],
  providers: [
    EstoquePedidoVendaEstoqueLocalService,
    {
      provide: NG_VALUE_ACCESSOR,
      // eslint-disable-next-line no-use-before-define
      useExisting: forwardRef(() => EstoquePedidoVendaEstoqueLocalSelectComponent),
      multi: true,
    },
  ],
})
export class EstoquePedidoVendaEstoqueLocalSelectComponent extends VsFormManipulator<string> implements OnDestroy, OnInit {
  @Input() public placeholder = 'Rnc.Shared.EstoquePedidoVendaEstoqueLocal.SearchInput.Title';
  @Input() public required = false
  @Input() public readonly: boolean;
  @Input() public disabled: boolean;
  @Input() public idProduto = '';
  @Input() public numeroLote = '';
  @Input() public numeroPedido = '';
  @Input() public numeroOdf = '';
  @Input() public autoInputFirstOptionIfHasOnlyOne = false;
  @Output() public loaded = new EventEmitter<EstoquePedidoVendaEstoqueLocalViewOutput | null>();
  @Output() public selected = new EventEmitter<EstoquePedidoVendaEstoqueLocalViewOutput>();
  @Output() public selectCleaned = new EventEmitter<boolean>();
  public form: FormGroup;
  private subscriptionManager = new VsSubscriptionManager();
  private gridOptions: VsGridOptions

  constructor(
    public service: EstoquePedidoVendaEstoqueLocalService,
    protected dialog: MatDialog,
    private formBuilder: FormBuilder,
    private changeDetectorRef: ChangeDetectorRef
  ) {
    super();
    this.getGridOptions();
    this.setInternalForm();
    this.setInitialValue();
  }
  public ngOnInit(): void {
    this.service.idProduto = this.idProduto;
    this.service.numeroLote = this.numeroLote;
    this.service.numeroPedido = this.numeroPedido;
    this.service.numeroOdf = this.numeroOdf;
    if (this.autoInputFirstOptionIfHasOnlyOne) {
      this.trySetFirstEstoqueLocal();
    }
    this.changeDetectorRef.detectChanges();
  }

  public ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public searchCleaned():void {
    this.value = null;
    this.form.get('internalDescriptionControl').setValue(null);
    this.selectCleaned.emit(true);
  }

  public openSearchModal(): void {
    this.dialog.open(VsSelectModalComponent, {
      data: {
        title: 'Rnc.Shared.EstoquePedidoVendaEstoqueLocal.SearchInput.Title',
        icon: 'warehouse-alt',
        gridOptions: this.gridOptions,
        service: this.service,
        filterOptions: {
          fields: [{ field: 'numeroLote', type: 'string', condition: 'contains' }]
        }
      } as IVsSelectModalData
    })
      .afterClosed()
      .subscribe((value:EstoquePedidoVendaEstoqueLocalViewOutput) => {
        if (!value) {
          return;
        }
        this.setInternalValue(value);
        this.selected.emit(value);
      });
  }

  private trySetFirstEstoqueLocal() {
    this.service.getAll({
      skipCount: 0,
      maxResultCount: 2,
    } as IBaseGetInput).subscribe((result: IPagedResultOutputDto<EstoquePedidoVendaEstoqueLocalViewOutput>) => {
      if (result.items.length === 1) {
        const estoqueLocal = result.items[0];
        this.setInternalValue(estoqueLocal);
        this.loaded.emit(estoqueLocal);
      }
    });
  }

  private setInternalValue(value:EstoquePedidoVendaEstoqueLocalViewOutput) {
    this.value = value?.id;
    this.form.get('internalDescriptionControl').setValue(value?.numeroLote);
  }

  private setInternalForm(): void {
    this.form = this.formBuilder.group({
      internalDescriptionControl: '',
    });
  }

  private setInitialValue() {
    this.subscriptionManager.add('on-value-change-subscription', this.onValueChange
      .subscribe((change) => {
        if (!change || change.internalChange) {
          return;
        }

        const EstoquePedidoVendaestoqueLocalId = change.newValue;
        if (EstoquePedidoVendaestoqueLocalId != null) {
          this.subscriptionManager.add('get-by-id', this.service.getById(this.value)
            .subscribe((EstoquePedidoVendaestoqueLocal: EstoquePedidoVendaEstoqueLocalViewOutput) => {
              this.form.get('internalDescriptionControl').setValue(EstoquePedidoVendaestoqueLocal.codigoLocal);
              this.loaded.emit(EstoquePedidoVendaestoqueLocal);
            }));
        }
      }));
  }
  private getGridOptions():void {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'Rnc.Shared.EstoquePedidoVendaEstoqueLocal.SearchInput.Lote',
        field: 'numeroLote',
        width: 70
      }),
      new VsGridSimpleColumn({
        headerName: 'Rnc.Shared.EstoquePedidoVendaEstoqueLocal.SearchInput.Local',
        field: 'codigoLocal',
        width: 70,
        kind: 'number',
        filterOptions: {
          operators: JQQB_NUMBER_OPERATORS
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'Rnc.Shared.EstoquePedidoVendaEstoqueLocal.SearchInput.Saldo',
        field: 'quantidade',
        width: 70,
        kind: 'number',
        filterOptions: {
          operators: JQQB_NUMBER_OPERATORS
        }
      }),
      new VsGridDateColumn({
        headerName: 'Rnc.Shared.EstoquePedidoVendaEstoqueLocal.SearchInput.DataFabricacao',
        field: 'dataFabricacao',
      }),
      new VsGridDateColumn({
        headerName: 'Rnc.Shared.EstoquePedidoVendaEstoqueLocal.SearchInput.DataValidade',
        field: 'dataValidade',
      }),
    ];
  }
}
