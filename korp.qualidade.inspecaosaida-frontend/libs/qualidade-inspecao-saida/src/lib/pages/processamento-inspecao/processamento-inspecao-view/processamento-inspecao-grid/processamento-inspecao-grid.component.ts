import { Component, Input, OnChanges, OnDestroy, SimpleChanges } from '@angular/core';
import { DecimalPipe } from '@angular/common';

import { TranslateService } from '@ngx-translate/core';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { VsStorageService, VsSubscriptionManager } from '@viasoft/common';
import {
  VsDialog,
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
  ProcessamentoInspecaoSaidaOutput
} from '../../../../tokens';
import { ProcessamentoInspecaoService } from '../../processamento-inspecao.service';
import { ProcessamentoInspecaoViewService } from '../processamento-inspecao-view.service';
import { ProcessamentoInspecaoDetailsModalComponent } from '../../processamento-inspecao-details-modal/processamento-inspecao-details-modal.component';

@Component({
  selector: 'qa-processamento-inspecao-grid',
  templateUrl: './processamento-inspecao-grid.component.html',
  styleUrls: ['./processamento-inspecao-grid.component.scss']
})
export class ProcessamentoInspecaoGridComponent implements OnChanges, OnDestroy {
  @Input() private estorno: boolean;
  @Input() private filtros: ProcessamentoInspecaoSaidaFilters = {};

  public gridOptions: VsGridOptions = new VsGridOptions();

  private subs: VsSubscriptionManager = new VsSubscriptionManager();

  constructor(private processamentoInspecaoService: ProcessamentoInspecaoService,
    private service: ProcessamentoInspecaoViewService, private decimalPipe: DecimalPipe,
    private vsDialog: VsDialog, private storageService: VsStorageService,
    private translateService: TranslateService) {
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
    this.gridOptions.id = '1B87A3DE-66F9-4AD0-A793-FDAEF5A48D81';
    this.gridOptions.enableSorting = false;
    this.gridOptions.enableQuickFilter = false;
    this.gridOptions.enableFilter = false;
    this.gridOptions.sizeColumnsToFit = false;

    this.gridOptions.columns = [
      new VsGridTagColumn({
        headerName: 'ProcessamentoInspecao.Status',
        field: 'statusTag',
        width: 130
      }),
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.Resultado',
        field: 'resultado'
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
        width: 500
      }),
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.Lote',
        field: 'lote',
        width: 130
      }),
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.Odf',
        field: 'odf',
        width: 130
      }),
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.OdfRetrabalho',
        field: 'odfRetrabalho',
        width: 130
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
        field: 'nomeUsuarioExecucao'
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
}
