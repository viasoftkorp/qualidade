import { Component, Input, OnChanges, OnDestroy, SimpleChanges } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';
import { Observable, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import {
  IPagedResultOutputDto,
  JQQB_COND_OR,
  JQQB_NUMBER_OPERATORS, JQQB_OP_CONTAINS,
  JQQB_OP_EQUAL,
  JQQB_OP_GREATER_OR_EQUAL,
  VsStorageService,
  VsSubscriptionManager
} from '@viasoft/common';
import {
  VsDialog,
  VsFilterGetItemsInput,
  VsFilterGetItemsOutput,
  VsFilterItem,
  VsFilterOptions,
  VsGridDateTimeColumn,
  VsGridGetInput,
  VsGridGetResult,
  VsGridNumberColumn,
  VsGridOptions,
  VsGridSimpleColumn,
  VsGridTagColumn
} from '@viasoft/components';

import {
  formatNumberToDecimal,
  GetAllProcessamentoInspecaoSaidaOutput,
  MovimentarInspecaoStatus,
  ProcessamentoInspecaoSaidaFilters,
  ProcessamentoInspecaoSaidaOutput,
  ResultadosInspecao
} from '../../../../tokens';
import { ProcessamentoInspecaoService } from '../../processamento-inspecao.service';
import { ProcessamentoInspecaoViewService } from '../processamento-inspecao-view.service';
import { ProcessamentoInspecaoDetailsModalComponent } from '../../processamento-inspecao-details-modal/processamento-inspecao-details-modal.component';
import { UserAutocompleteService, UserOutput } from '@viasoft/administration';

@Component({
  selector: 'qa-processamento-inspecao-grid',
  templateUrl: './processamento-inspecao-grid.component.html',
  styleUrls: ['./processamento-inspecao-grid.component.scss'],
  providers: [UserAutocompleteService]
})
export class ProcessamentoInspecaoGridComponent implements OnChanges, OnDestroy {
  @Input() private estorno: boolean;
  @Input() private filtros: ProcessamentoInspecaoSaidaFilters = {};

  public gridOptions: VsGridOptions = new VsGridOptions();

  private subs: VsSubscriptionManager = new VsSubscriptionManager();

  constructor(private processamentoInspecaoService: ProcessamentoInspecaoService,
    private service: ProcessamentoInspecaoViewService, private decimalPipe: DecimalPipe,
    private vsDialog: VsDialog, private storageService: VsStorageService,
    private translateService: TranslateService, private userAutocompleteService: UserAutocompleteService) {
    this.iniciarGrid();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.filtros) {
      this.gridOptions.refresh();
    }
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  private iniciarGrid(): void {
    this.gridOptions.id = '7AC3E739-23B5-4D55-9A0B-EB0B944CEE45';
    this.gridOptions.sizeColumnsToFit = false;

    this.gridOptions.columns = [
      new VsGridTagColumn({
        headerName: 'ProcessamentoInspecao.Status',
        field: 'statusTag',
        width: 130,
        kind: 'number',
        filterOptions: this.statusFilterOptions,
        sorting: {
          disable: true
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.Resultado',
        field: 'resultado',
        filterOptions: this.resultadoFilterOptions
      }),
      new VsGridNumberColumn({
        headerName: 'ProcessamentoInspecao.QuantidadeTotal',
        field: 'quantidadeTotal',
        format: (quantidadeTotal: number) => formatNumberToDecimal(this.decimalPipe, quantidadeTotal),
        width: 150
      }),
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.Produto',
        field: 'descricaoProduto',
        width: 500,
        filterOptions: {
          useField: 'codigoProduto'
        },
        sorting: {
          useField: 'codigoProduto'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.Lote',
        field: 'lote',
        width: 130
      }),
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.Odf',
        field: 'odf',
        width: 130,
        kind: 'number',
        filterOptions: {
          useField: 'ordemFabricacao',
          operators: JQQB_NUMBER_OPERATORS
        },
        sorting: {
          useField: 'ordemFabricacao'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.OdfRetrabalho',
        field: 'odfRetrabalho',
        width: 130,
        kind: 'number',
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.Erro',
        field: 'erro'
      }),
      new VsGridNumberColumn({
        headerName: 'ProcessamentoInspecao.NumeroExecucoes',
        field: 'numeroExecucoes',
        width: 150
      }),
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.UsuarioExecucao',
        field: 'nomeUsuarioExecucao',
        filterOptions: this.nomeUsuarioExecucaoFilterOptions,
        sorting: {
          disable: true
        }
      }),
      new VsGridDateTimeColumn({
        headerName: 'ProcessamentoInspecao.DataExecucao',
        field: 'dataExecucao'
      })
    ];

    this.gridOptions.actions = [
      {
        icon: 'redo',
        tooltip: 'ProcessamentoInspecao.Reprocessar',
        condition: (index: number, inspecaoSaida: ProcessamentoInspecaoSaidaOutput) => {
          return inspecaoSaida.status === MovimentarInspecaoStatus.Falha;
        },
        callback: (index: number, inspecaoSaida: ProcessamentoInspecaoSaidaOutput) => {
          this.reprocessar(inspecaoSaida.idSaga);
        }
      },
      {
        icon: 'undo',
        tooltip: 'ProcessamentoInspecao.LiberarProcessamento',
        callback: (rowIndex: number, data: ProcessamentoInspecaoSaidaOutput) => this.removeProcess(data.idSaga),
        condition: (rowIndex: number, data: ProcessamentoInspecaoSaidaOutput) => data.status === MovimentarInspecaoStatus.Falha,
      },
    ];

    this.gridOptions.get = (input: VsGridGetInput) => this.getGridData(input);
    this.gridOptions.edit = (index: number, processamento: ProcessamentoInspecaoSaidaOutput) => {
      this.vsDialog.open(ProcessamentoInspecaoDetailsModalComponent, processamento.transferencias);
    };
    this.gridOptions.select = (index: number, processamento: ProcessamentoInspecaoSaidaOutput) => {
      this.vsDialog.open(ProcessamentoInspecaoDetailsModalComponent, processamento.transferencias);
    };
  }

  private getGridData(input: VsGridGetInput): Observable<VsGridGetResult> {
    return this.service.get(input, this.filtros, this.estorno)
      .pipe(
        tap((pagedProcessamento: GetAllProcessamentoInspecaoSaidaOutput) => {
          if (!pagedProcessamento.items) {
            pagedProcessamento.items = [];
          }

          const statusColorMapping = new Map<MovimentarInspecaoStatus, string>([
            [MovimentarInspecaoStatus.Inicio, '#095ABA'],
            [MovimentarInspecaoStatus.EmProcesso, '#095ABA'],
            [MovimentarInspecaoStatus.Falha, '#E34850'],
            [MovimentarInspecaoStatus.Sucesso, '#2D9D78']
          ]);

          pagedProcessamento.items.forEach((processamento: ProcessamentoInspecaoSaidaOutput) => {
            const descricaoStatus = `ProcessamentoInspecao.MovimentarInspecaoStatus.${MovimentarInspecaoStatus[processamento.status]}`;

            (processamento as any).statusTag = {
              color: statusColorMapping.get(processamento.status),
              text: this.translateService.instant(descricaoStatus)
            };
          });
        }),
        map((pagedProcessamento: GetAllProcessamentoInspecaoSaidaOutput) => {
          return new VsGridGetResult(pagedProcessamento.items, pagedProcessamento.totalCount);
        })
      );
  }

  private reprocessar(idSaga: string): void {
    this.subs.add('reprocessar', this.service.reprocessar(idSaga)
      .subscribe(() => {
        this.gridOptions.refresh();
      }));
  }

  private removeProcess(idSaga: string) {
    this.subs.add('removeProcess', this.service.removeProcess(idSaga)
      .subscribe(() => {
        this.gridOptions.refresh();
      }));
  }

  private get statusFilterOptions(): VsFilterOptions {
    return {
      operators: [JQQB_OP_EQUAL],
      conditions: [JQQB_COND_OR],
      blockInput: true,
      useField: 'status',
      mode: 'selection',
      multiple: true,
      getItems: () => of({
        items: [
          {
            key: MovimentarInspecaoStatus.Inicio.toString(),
            value: `ProcessamentoInspecao.MovimentarInspecaoStatus.${MovimentarInspecaoStatus[MovimentarInspecaoStatus.Inicio]}`,
          },
          {
            key: MovimentarInspecaoStatus.EmProcesso.toString(),
            value: `ProcessamentoInspecao.MovimentarInspecaoStatus.${MovimentarInspecaoStatus[MovimentarInspecaoStatus.EmProcesso]}`,
          },
          {
            key: MovimentarInspecaoStatus.Falha.toString(),
            value: `ProcessamentoInspecao.MovimentarInspecaoStatus.${MovimentarInspecaoStatus[MovimentarInspecaoStatus.Falha]}`,
          },
          {
            key: MovimentarInspecaoStatus.Sucesso.toString(),
            value: `ProcessamentoInspecao.MovimentarInspecaoStatus.${MovimentarInspecaoStatus[MovimentarInspecaoStatus.Sucesso]}`,
          }
        ],
        totalCount: 4
      })
    };
  }

  private get resultadoFilterOptions(): VsFilterOptions {
    return {
      operators: [JQQB_OP_EQUAL],
      conditions: [JQQB_COND_OR],
      blockInput: true,
      mode: 'selection',
      multiple: true,
      getItems: () => of({
        items: [
          {
            key: ResultadosInspecao.Aprovado.toString(),
            value: `ProcessamentoInspecao.Resultados.Aprovado`,
          },
          {
            key: ResultadosInspecao.ParcialmenteAprovado.toString(),
            value: `ProcessamentoInspecao.Resultados.ParcialmenteAprovado`,
          },
          {
            key: ResultadosInspecao.NaoAplicavel.toString(),
            value: `ProcessamentoInspecao.Resultados.NaoAplicavel`,
          },
          {
            key: ResultadosInspecao.NaoConforme.toString(),
            value: `ProcessamentoInspecao.Resultados.NaoConforme`,
          }
        ],
        totalCount: 4
      })
    };
  }

  private get nomeUsuarioExecucaoFilterOptions(): VsFilterOptions {
    return {
      useField: 'idUsuarioExecucao',
      mode: 'selection',
      blockInput: true,
      getItemsFilterFields: ['firstName', 'secondName'],
      getItemsFilterOperator: JQQB_OP_CONTAINS,
      operators: [JQQB_OP_EQUAL],
      conditions: [JQQB_COND_OR],
      multiple: true,
      getItems: (input: VsFilterGetItemsInput) => this.userAutocompleteService
        .getAll({
          maxResultCount: input.maxResultCount,
          skipCount: input.skipCount,
          advancedFilter: input.filter
        } as VsGridGetInput)
        .pipe(map((pagedResult: IPagedResultOutputDto<UserOutput>): VsFilterGetItemsOutput => ({
          items: pagedResult.items.map((user) => ({
            key: user.id,
            value: user.login
          } as VsFilterItem)),
          totalCount: pagedResult.totalCount
        })))
    };
  }
}
