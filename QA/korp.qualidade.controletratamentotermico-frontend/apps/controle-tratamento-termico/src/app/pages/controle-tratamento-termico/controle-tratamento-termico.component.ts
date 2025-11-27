import { ChangeDetectorRef, Component, effect, inject, OnDestroy, OnInit, QueryList, signal, ViewChildren } from '@angular/core';
import { VsGridOptions, VsGridNumberColumn, VsGridDateColumn, VsGridSimpleColumn, VsGridCheckboxColumn, VsGridDateTimeColumn, VsGridGetInput, VsGridGetResult, VsGridComponent, IVsTableEditRowResult, IVsTableEditRowOptions, IVsTableEditRowSuccessResult, IVsTableEditMultipleRowOnRowsEditInfo } from '@viasoft/components';
import { catchError, from, map, of } from 'rxjs';
import { LoteGridCellComponent } from './tokens/grid-cells/lote-grid-cell.component';
import { ControleTratamentoTermicoGridItensService } from './services/controle-tratamento-termico-grid-itens.service';
import { TipoTratamentoGridCellComponent } from './tokens/grid-cells/tipo-tratamento-grid-cell.component';
import { ParametroGridCellComponent } from './tokens/grid-cells/parametro-grid-cell.component';
import { CalcoGridCellComponent } from './tokens/grid-cells/calco-grid-cell.component';
import { HttpErrorResponse } from '@angular/common/http';
import { MessageService, VsAuthorizationService, VsSubscriptionManager } from '@viasoft/common';
import { IVsTableEditMultipleRowOptions, VsTableCancelEditTriggerEnum } from '@viasoft/components';
import { Policy } from '../../authorization/policy';
import { TratamentoTermicoApiService } from '../../services/tratamento-termico-api.service';
import { TratamentoTermicoPecaApiService } from '../../services/tratamento-termico-peca-api.service';
import { TratamentoTermicoAtualizarInput, TratamentoTermicoDtoOutput, TratamentoTermicoInserirInput, TratamentoTermicoPecaAtualizarInput, TratamentoTermicoPecaDtoOutput } from '../../tokens';
import { RightSideDatepickerGridCellComponent } from './tokens/grid-cells/right-side-datepicker-grid-cell.component';

@Component({
  selector: 'app-controle-tratamento-termico',
  templateUrl: './controle-tratamento-termico.component.html',
  styleUrls: ['./controle-tratamento-termico.component.scss'],
  providers: [ControleTratamentoTermicoGridItensService]
})
export class ControleTratamentoTermicoComponent implements OnDestroy, OnInit {
  private subs = new VsSubscriptionManager();
  // actually the data has one more prop than the interface called hTotal that is calculated from ha and hp after the data is fetched
  public tratamentoTermicoGridOptions: VsGridOptions<TratamentoTermicoDtoOutput>;
  public tratamentoTermicoPecasGridOptions: VsGridOptions<TratamentoTermicoPecaDtoOutput>;
  private gridCabecalhoEl = signal<VsGridComponent>(undefined);

  @ViewChildren(VsGridComponent) private gridsList: QueryList<VsGridComponent>;

  private selectedCabecalho = signal<TratamentoTermicoDtoOutput | null>(null);
  private isCreating = signal<boolean>(false);
  private isCreatingIndex = signal<number>(-1);
  private pecasUnsavedData = signal<TratamentoTermicoPecaDtoOutput[]>([]);

  public gridItensService = inject(ControleTratamentoTermicoGridItensService);

  private policy = Policy;
  private tratamentoTermicoRemoverTratamento: boolean;
  public tratamentoTermicoCriarEditarTratamento: boolean;

  public isLoaded = signal<boolean>(false);

  constructor(
    private tratamentoTermicoService: TratamentoTermicoApiService,
    private tratamentoTermicoPecaService: TratamentoTermicoPecaApiService,
    private messageService: MessageService,
    private authorizationService: VsAuthorizationService,
    private changeDetectorRef: ChangeDetectorRef
  ) {

    effect(() => {
      if (!this.isLoaded()) return;
      const grid = this.gridCabecalhoEl();
      const data = this.selectedCabecalho();
      if (grid) {
        grid.table.selection = data;
      }
    });

    effect(() => {
      if (!this.isLoaded()) return;
      this.gridCabecalhoEl.set(this.gridsList.find(g => g.options.id === this.tratamentoTermicoGridOptions.id));
      this.gridItensService.gridPecasEl.set(this.gridsList.find(g => g.options.id === this.tratamentoTermicoPecasGridOptions.id));
    }, { allowSignalWrites: true });
  }

  async ngOnInit(): Promise<void> {
    await this.getPermissions();
    this.initGrids();
    this.isLoaded.set(true);
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  private async getPermissions() {
    const policies: string[] = [
      this.policy.tratamentoTermicoCriarEditarTratamento,
      this.policy.tratamentoTermicoRemoverTratamento,
    ];
    const permissions = await this.authorizationService.isGrantedMap(policies);
    this.tratamentoTermicoRemoverTratamento = permissions.get(this.policy.tratamentoTermicoRemoverTratamento);
    this.tratamentoTermicoCriarEditarTratamento = permissions.get(this.policy.tratamentoTermicoCriarEditarTratamento);
  }

  initGrids() {
    this.configGridCabecalho();
    this.configGridItens();
  }

  private setSelectedCabecalho(data: TratamentoTermicoDtoOutput) {
    this.selectedCabecalho.set(data);
    this.tratamentoTermicoPecasGridOptions.refresh();
  }

  //#region Cabecalho
  configGridCabecalho() {
    this.tratamentoTermicoGridOptions = new VsGridOptions();
    this.tratamentoTermicoGridOptions.id = "b91b32ac-505f-4513-94e6-6689ccec6792";
    this.tratamentoTermicoGridOptions.enableVirtualScroll = true;
    this.tratamentoTermicoGridOptions.onRowClick = (_rowindex, data) => {
      !this.gridCabecalhoEl().editManagerService.isEditModeEnabled() && this.setSelectedCabecalho(data);
    };
    this.tratamentoTermicoGridOptions.columns = this.getColumnsCabecalho();
    this.tratamentoTermicoGridOptions.get = (input) => this.getAllCabecalho(input);
    this.tratamentoTermicoGridOptions.editRowOptions = this.getEditRowOptionsCabecalho();
    this.tratamentoTermicoGridOptions.actions = [
      {
        icon: 'trash-alt',
        tooltip: 'TratamentoTermico.Remove.Title',
        callback: (_rowIndex, data) => this.deletarCabecalho(data.id),
        condition: () => this.tratamentoTermicoRemoverTratamento
      },
      {
        icon: 'file-chart-line',
        tooltip: 'TratamentoTermico.ImprimirRelatorio',
        callback: (rowIndex, tratamentoTermico: TratamentoTermicoDtoOutput) => this.imprimirRelatorio(tratamentoTermico)
      }
    ];
  }

  public startCreateCabecalho() {
    this.isCreating.set(true);
    this.tratamentoTermicoGridOptions.refresh();
  }

  private getColumnsCabecalho() {
    return [
      new VsGridSimpleColumn({
        field: 'lote',
        headerName: 'TratamentoTermico.Column.Lote',
        type: LoteGridCellComponent,
      }),
      new VsGridDateColumn({
        field: 'dataEmissao',
        headerName: 'TratamentoTermico.Column.DataEmissao',
      }),
      new VsGridSimpleColumn({
        field: 'descricaoTratamentoTermicoTipo',
        headerName: 'TratamentoTermico.Column.TipoTratamento',
        type: TipoTratamentoGridCellComponent
      }),
      new VsGridNumberColumn({
        field: 'ha',
        headerName: 'TratamentoTermico.Column.HA',
      }),
      new VsGridNumberColumn({
        field: 'hp',
        headerName: 'TratamentoTermico.Column.HP',
      }),
      new VsGridNumberColumn({
        field: 'hTotal',
        headerName: 'TratamentoTermico.Column.HTotal',
        filterOptions: { disable: true },
        sorting: { disable: true }
      }),
      new VsGridNumberColumn({
        field: 'total',
        headerName: 'TratamentoTermico.Column.Total',
      }),
      new VsGridNumberColumn({
        field: 'tMin',
        headerName: 'TratamentoTermico.Column.TMin',
      }),
      new VsGridNumberColumn({
        field: 'tMax',
        headerName: 'TratamentoTermico.Column.TMax',
      }),
      new VsGridCheckboxColumn({
        field: 'grafico',
        headerName: 'TratamentoTermico.Column.Grafico',
        disabled: true
      }),
      new VsGridCheckboxColumn({
        field: 'ventilar',
        headerName: 'TratamentoTermico.Column.Ventilar',
        disabled: true
      }),
      new VsGridNumberColumn({
        field: 'taf',
        headerName: 'TratamentoTermico.Column.TAF',
      }),
      new VsGridNumberColumn({
        field: 'pesoBrutoTotal',
        headerName: 'TratamentoTermico.Column.PesoBrutoTotal',
      }),
      new VsGridNumberColumn({
        field: 'pesoLiquidoTotal',
        headerName: 'TratamentoTermico.Column.PesoLiquidoTotal',
      }),
      new VsGridSimpleColumn({
        field: 'descricaoParametro',
        headerName: 'TratamentoTermico.Column.Parametro',
        type: ParametroGridCellComponent
      }),
      new VsGridSimpleColumn({
        field: 'descricaoCalco',
        headerName: 'TratamentoTermico.Column.Calco',
        type: CalcoGridCellComponent
      }),
      new VsGridSimpleColumn({
        field: 'velocidadeAquecimento',
        headerName: 'TratamentoTermico.Column.VelocidadeAquecimento',
      }),
      new VsGridSimpleColumn({
        field: 'enchimentoTemperatura',
        headerName: 'TratamentoTermico.Column.EnchimentoTemperatura',
      }),
      new VsGridNumberColumn({
        field: 'patamar',
        headerName: 'TratamentoTermico.Column.Patamar',
      }),
      new VsGridNumberColumn({
        field: 'temperaturaPatamar',
        headerName: 'TratamentoTermico.Column.TemperaturaPatamar',
      }),
      new VsGridSimpleColumn({
        field: 'velocidadeResfriamento',
        headerName: 'TratamentoTermico.Column.VelocidadeResfriamento',
      }),
      new VsGridDateTimeColumn({
        field: 'dataInicio',
        headerName: 'TratamentoTermico.Column.DataInicio',
      }),
      new VsGridDateTimeColumn({
        field: 'dataChegada',
        headerName: 'TratamentoTermico.Column.DataChegada',
      }),
      new VsGridDateTimeColumn({
        field: 'dataDesligamento',
        headerName: 'TratamentoTermico.Column.DataDesligamento',
      }),
      new VsGridDateTimeColumn({
        field: 'dataAbertura',
        headerName: 'TratamentoTermico.Column.DataAbertura',
        type: RightSideDatepickerGridCellComponent
      }),
    ];
  }

  private getEditRowOptionsCabecalho(): IVsTableEditRowOptions<TratamentoTermicoDtoOutput, TratamentoTermicoDtoOutput> {
    return {
      isAutoEditable: this.tratamentoTermicoCriarEditarTratamento,
      shouldHideEditModeButtons: !this.tratamentoTermicoCriarEditarTratamento,
      onCancelEdit: (trigger) => {
        if (trigger === VsTableCancelEditTriggerEnum.InlineCancelButton || trigger === VsTableCancelEditTriggerEnum.EscapeKey) {
          this.isCreating.set(false);
          this.gridItensService.selectedLote.set(null);
          this.pecasUnsavedData.set([]);
          this.gridCabecalhoEl().options.refresh(true);
        }
      },
      isCellEditable: (_index, fieldName) => {
        const blockedFields = ['dataEmissao', 'hTotal', 'pesoBrutoTotal', 'pesoLiquidoTotal'];
        if (!this.isCreating()) {
          blockedFields.push('lote');
        }
        return !blockedFields.includes(fieldName);
      },
      onRowEdit: (index, originalData, newData) => {
        this.gridItensService.gridPecasEl().editManagerService.saveEdit();
        const isDataInvalidMessage = this.validateCabecalho(newData);
        if (isDataInvalidMessage) {
          return of({ success: false, errorMessage: isDataInvalidMessage } as IVsTableEditRowResult);
        }

        const isCreatingData = typeof originalData?.id === 'undefined';

        if (isCreatingData) {
          if (!this.pecasUnsavedData().length) {
            return of({ success: false, avoidThrowError: true } as IVsTableEditRowResult);
          }
          const input: TratamentoTermicoInserirInput = {
            ...newData,
            pecas: this.pecasUnsavedData()
          };
          this.pecasUnsavedData.set([]);
          return from(this.tratamentoTermicoService.criar(input)).pipe(
            map(result => {
              return { success: true, shouldAutoRefreshGrid: true, updatedRowData: result } as IVsTableEditRowResult;
            }),
            catchError((err: HttpErrorResponse) => {
              return of({ success: false, errorMessage: 'TratamentoTermico.Errors.UnknownError' } as IVsTableEditRowResult);
            })
          );
        } else {
          const input: TratamentoTermicoAtualizarInput = {
            ...newData,
            id: originalData.id,
            pecas: this.pecasUnsavedData().map(p => ({
              id: p.id,
              numeroTermopares: p.numeroTermopares,
              codigoTratamentoTermicoTipo: p.codigoTratamentoTermicoTipo
            }))
          };
          this.pecasUnsavedData.set([]);
          return from(this.tratamentoTermicoService.atualizar(input)).pipe(
            map(result => {
              return { success: true, shouldAutoRefreshGrid: true, updatedRowData: result } as IVsTableEditRowResult;
            }), 
            catchError((err: HttpErrorResponse) => {
              return of({ success: false, errorMessage: 'TratamentoTermico.Errors.UnknownError' } as IVsTableEditRowResult);
            })
          );
        }
      },
    };
  }

  private getAllCabecalho(input: VsGridGetInput) {
    return from(this.tratamentoTermicoService.buscarLista(input))
      .pipe(
        map(res => new VsGridGetResult(res.items)),
        map((res) => {
          res.data = res.data.map((item) => ({
            ...item,
            hTotal: item.ha + item.hp
          }));
          return res;
        }),
        map(result => {
          if (!this.isCreating()) {
            this.setSelectedCabecalho(result.data[0]);
            return result;
          }
          // Add new line at the end
          result.data = result.data.concat({ id: undefined } as TratamentoTermicoDtoOutput);

          // TODO: we need 500ms so virtual scroll + edit + startEdit works properly
          setTimeout(() => {
            const index = result.data.length - 1;
            this.isCreatingIndex.set(index);
            this.setSelectedCabecalho(result.data[index]);
            this.gridCabecalhoEl().editManagerService.startEdit(index, { columnName: 'lote', rowIndex: index });
          }, 500);
          return result;
        })
      );
  };

  private deletarCabecalho(id: string) {
    this.subs.add('msg-deletar', this.messageService
      .confirm('TratamentoTermico.Remove.Confirm')
      .subscribe((result: boolean) => {
        if (result) {
          return this.tratamentoTermicoService.deletar(id)
            .then(() => {
              this.tratamentoTermicoGridOptions.refresh();
            }).catch(() => {
              this.messageService.error('TratamentoTermico.Remove.UnknownError');
            });
        }
      }));

  }

  private validateCabecalho(data: TratamentoTermicoDtoOutput): string | undefined {
    if (!data.lote) {
      return 'TratamentoTermico.Errors.LoteRequired';
    }

    const numberColumns: (keyof TratamentoTermicoDtoOutput)[] = [
      'ha', 'hp', 'total', 'tMin', 'tMax', 'taf',
      'patamar', 'temperaturaPatamar'
    ];

    for (const col of numberColumns) {
      const value = data[col];

      if (value == null || value == undefined) continue;

      if (value === '' || isNaN(Number(value)) || this.isDecimal(Number(value))) {
        return `TratamentoTermico.Errors.${col}Invalid`;
      }
    }

    return undefined;
  }

  private isDecimal(num: number): boolean {
    return num % 1 !== 0;
  }
  //#endregion Cabecalho

  //#region Itens
  configGridItens() {
    this.tratamentoTermicoPecasGridOptions = new VsGridOptions();
    this.tratamentoTermicoPecasGridOptions.id = "3bcc3c27-04c1-4375-bc6f-5e1cd8f9cb42";
    this.tratamentoTermicoPecasGridOptions.columns = this.getColumnsItens();
    this.tratamentoTermicoPecasGridOptions.get = (input) => this.getAllItens(this.selectedCabecalho(), input);
    this.tratamentoTermicoPecasGridOptions.editRowOptions = this.getEditRowOptionsItens();
    this.tratamentoTermicoPecasGridOptions.enableVirtualScroll = true;
  }

  private getColumnsItens() {
    return [
      new VsGridNumberColumn({
        field: 'numeroOdf',
        headerName: 'TratamentoTermico.Itens.Column.ODF'
      }),
      new VsGridSimpleColumn({
        field: 'numeroOperacao',
        headerName: 'TratamentoTermico.Itens.Column.Operacao'
      }),
      new VsGridSimpleColumn({
        field: 'peca',
        headerName: 'TratamentoTermico.Itens.Column.Peca'
      }),
      new VsGridSimpleColumn({
        field: 'descricao',
        headerName: 'TratamentoTermico.Itens.Column.Descricao'
      }),
      new VsGridNumberColumn({
        field: 'ni',
        headerName: 'TratamentoTermico.Itens.Column.NI',
        filterOptions: { disable: true },
        sorting: { disable: true }
      }),
      new VsGridNumberColumn({
        field: 'quantidade',
        headerName: 'TratamentoTermico.Itens.Column.Qtde'
      }),
      new VsGridNumberColumn({
        field: 'pesoBruto',
        headerName: 'TratamentoTermico.Itens.Column.PesoBruto'
      }),
      new VsGridNumberColumn({
        field: 'pesoLiquido',
        headerName: 'TratamentoTermico.Itens.Column.PesoLiquido'
      }),
      new VsGridSimpleColumn({
        field: 'cliente',
        headerName: 'TratamentoTermico.Itens.Column.Cliente'
      }),
      new VsGridSimpleColumn({
        field: 'descricaoTratamentoTermicoTipo',
        headerName: 'TratamentoTermico.Itens.Column.TipoTratamento',
        type: TipoTratamentoGridCellComponent
      }),
      new VsGridSimpleColumn({
        field: 'numeroTermopares',
        headerName: 'TratamentoTermico.Itens.Column.NumeroTermopares'
      })
    ];
  }

  private getEditRowOptionsItens(): IVsTableEditMultipleRowOptions {
    return {
      isAutoEditable: this.tratamentoTermicoCriarEditarTratamento,
      shouldHideEditModeButtons: !this.tratamentoTermicoCriarEditarTratamento,
      isCellEditable(_index, fieldName) {
        const blockedFields = ['numeroOdf', 'numeroOperacao', 'peca', 'descricao', 'ni', 'quantidade', 'pesoBruto', 'pesoLiquido', 'cliente'];
        return !blockedFields.includes(fieldName);
      },
      fullEditMode: true,
      onRowsEdit: (rowEditData: IVsTableEditMultipleRowOnRowsEditInfo<any>[]) => {

        // If cabecalho is editing we just update the pecasNewDataInput signal
        if (this.gridCabecalhoEl().editManagerService.isEditModeEnabled()) {
          const newData = this.gridItensService.gridPecasEl().data;

          rowEditData.forEach(rowInfo => {
            if (rowInfo.index !== -1) {
              newData[rowInfo.index] = rowInfo.newData;
            }
          });

          this.pecasUnsavedData.set(newData);
          return of<IVsTableEditRowSuccessResult>({ success: true });

        } else {
          const input = rowEditData.map(row => ({
            ...row.newData,
            id: row.originalData.id
          } as TratamentoTermicoPecaAtualizarInput));

          return from(this.tratamentoTermicoPecaService.atualizar(input)).pipe(
            map(result => ({
              success: true,
              shouldAutoRefreshGrid: true,
              updatedRowData: result
            } as IVsTableEditRowSuccessResult)),
            catchError(() => of({ success: false } as IVsTableEditRowResult))
          );
        }
      }
    };
  }

  getAllItens(selectedCabecalho: TratamentoTermicoDtoOutput, input: VsGridGetInput) {
    // On edit mode when lote of cabeçalho changes we need to get the peças from the selected lote
    if (this.gridCabecalhoEl()?.editManagerService.isEditModeEnabled() && this.gridItensService.selectedLote() != null) {
      return from(this.tratamentoTermicoPecaService.buscarListaPorLote(this.gridItensService.selectedLote()))
        .pipe(
          map(res => new VsGridGetResult(res.items)),
          map((res) => {
            setTimeout(() => {
              this.gridItensService.gridPecasEl().editManagerService.startEdit(undefined, { rowIndex: (res.data.length - 1) });
            }, 500);
            return res;
          })
        );
    }
    if (selectedCabecalho == null || selectedCabecalho.id == undefined) {
      return of(new VsGridGetResult([]));
    }
    return from(this.tratamentoTermicoPecaService.buscarLista(selectedCabecalho.id, input))
      .pipe(
        map(res => new VsGridGetResult(res.items, undefined))
      );
  };
  //#endregion Itens

  public imprimirRelatorio(tratamentoTermico: TratamentoTermicoDtoOutput) {
    this.tratamentoTermicoService.imprimirRelatorio(tratamentoTermico.id).then((r) => {
      const byteArray = new Uint8Array(atob(r).split('').map((char) => char.charCodeAt(0)));
      const blob = new Blob([byteArray], { type: 'application/pdf' });
      const url = URL.createObjectURL(blob);
      window.open(url, '_blank', 'noopener');
    });
  }
}
