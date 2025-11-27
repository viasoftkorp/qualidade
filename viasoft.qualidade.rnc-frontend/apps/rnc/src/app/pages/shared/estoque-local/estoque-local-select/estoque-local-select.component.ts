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
  IBaseGetInput, IPagedResultOutputDto, JQQB_NUMBER_OPERATORS, VsSubscriptionManager
} from '@viasoft/common';
import {
  VsFormManipulator,
  VsSelectModalComponent,
  IVsSelectModalData,
  VsGridOptions,
  VsGridSimpleColumn,
  VsGridDateColumn
} from '@viasoft/components';
import { EstoqueLocalOutput } from '@viasoft/rnc/api-clients/Estoque-Locais/model/estoque-local-output';
import { EstoqueLocalService } from './estoque-local.service';

@Component({
  selector: 'rnc-estoque-local-select',
  templateUrl: './estoque-local-select.component.html',
  styleUrls: ['./estoque-local-select.component.scss'],
  providers: [
    EstoqueLocalService,
    {
      provide: NG_VALUE_ACCESSOR,
      // eslint-disable-next-line no-use-before-define
      useExisting: forwardRef(() => EstoqueLocalSelectComponent),
      multi: true,
    },
  ],
})
export class EstoqueLocalSelectComponent extends VsFormManipulator<string> implements OnDestroy, OnInit {
  @Input() public placeholder = 'Rnc.Shared.EstoqueLocal.SearchInput.Title';
  @Input() public required = false
  @Input() public readonly: boolean;
  @Input() public disabled: boolean;
  @Input() public idProduto = '';
  @Input() public numeroLote = '';
  @Input() public autoInputFirstOptionIfHasOnlyOne = false;
  @Output() public loaded = new EventEmitter<EstoqueLocalOutput | null>();
  @Output() public selected = new EventEmitter<EstoqueLocalOutput>();
  @Output() public selectCleaned = new EventEmitter<boolean>();
  public form: FormGroup;
  private subscriptionManager = new VsSubscriptionManager();
  private gridOptions: VsGridOptions

  constructor(
    public service: EstoqueLocalService,
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
        title: 'Rnc.Shared.EstoqueLocal.SearchInput.Title',
        icon: 'warehouse-alt',
        gridOptions: this.gridOptions,
        service: this.service,
        filterOptions: {
          fields: [{ field: 'lote', type: 'string', condition: 'contains' }]
        }
      } as IVsSelectModalData
    })
      .afterClosed()
      .subscribe((value:EstoqueLocalOutput) => {
        if (!value) {
          return;
        }

        this.setInternalValue(value);
        this.selected.emit(value);
      });
  }

  private setInternalValue(value: EstoqueLocalOutput) {
    this.value = value?.id;
    this.form.get('internalDescriptionControl').setValue(value?.lote);
  }

  private trySetFirstEstoqueLocal() {
    this.service.getAll({
      skipCount: 0,
      maxResultCount: 2,
    } as IBaseGetInput).subscribe((result: IPagedResultOutputDto<EstoqueLocalOutput>) => {
      if (result.items.length === 1) {
        const estoqueLocal = result.items[0];
        this.setInternalValue(estoqueLocal);
        this.loaded.emit(estoqueLocal);
      }
    });
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

        const estoqueLocalId = change.newValue;
        if (estoqueLocalId != null) {
          this.subscriptionManager.add('get-by-id', this.service.getById(this.value)
            .subscribe((estoqueLocal: EstoqueLocalOutput) => {
              this.form.get('internalDescriptionControl').setValue(estoqueLocal.codigoLocal);
              this.loaded.emit(estoqueLocal);
            }));
        }
      }));
  }
  private getGridOptions():void {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'Rnc.Shared.EstoqueLocal.SearchInput.Lote',
        field: 'lote',
        width: 70
      }),
      new VsGridSimpleColumn({
        headerName: 'Rnc.Shared.EstoqueLocal.SearchInput.Local',
        field: 'codigoLocal',
        width: 70,
        kind: 'number',
        filterOptions: {
          operators: JQQB_NUMBER_OPERATORS
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'Rnc.Shared.EstoqueLocal.SearchInput.Saldo',
        field: 'quantidade',
        width: 70,
        kind: 'number',
        filterOptions: {
          operators: JQQB_NUMBER_OPERATORS
        }
      }),
      new VsGridDateColumn({
        headerName: 'Rnc.Shared.EstoqueLocal.SearchInput.DataFabricacao',
        field: 'dataFabricacao',
      }),
      new VsGridDateColumn({
        headerName: 'Rnc.Shared.EstoqueLocal.SearchInput.DataValidade',
        field: 'dataValidade',
      }),
    ];
  }
}
