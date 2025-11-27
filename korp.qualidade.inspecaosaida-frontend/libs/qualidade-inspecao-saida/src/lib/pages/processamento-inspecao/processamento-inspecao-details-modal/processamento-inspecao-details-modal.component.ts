import { Component, Inject } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Observable, of } from 'rxjs';

import { VsGridGetResult, VsGridOptions, VsGridSimpleColumn } from '@viasoft/components';

import {
  formatNumberToDecimal,
  ProcessamentoInspecaoSaidaTransferenciaOutput,
  TipoTransferencia,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO
} from '../../../tokens';
import { ProcessamentoInspecaoViewService } from '../processamento-inspecao-view/processamento-inspecao-view.service';

@Component({
  selector: 'qa-processamento-inspecao-details-modal',
  templateUrl: './processamento-inspecao-details-modal.component.html',
  styleUrls: ['./processamento-inspecao-details-modal.component.scss'],
  providers: [DecimalPipe]
})
export class ProcessamentoInspecaoDetailsModalComponent {
  public gridOptions: VsGridOptions;

  constructor(private decimalPipe: DecimalPipe, private service: ProcessamentoInspecaoViewService,
              @Inject(MAT_DIALOG_DATA) private transferencias: ProcessamentoInspecaoSaidaTransferenciaOutput[]) {
    this.service.getParametroBool(UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO, UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO)
      .then((utilizarReservaPedido: boolean) => {
        this.iniciarGrid(utilizarReservaPedido);
      });
  }

  private iniciarGrid(utilizarReservaPedido: boolean): void {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = 'D4F5F5FB-DE07-4255-9136-539E0F299E5E';
    this.gridOptions.enableSorting = false;
    this.gridOptions.enableQuickFilter = false;
    this.gridOptions.enableFilter = false;

    this.gridOptions.columns = [];

    if (utilizarReservaPedido) {
      this.gridOptions.columns = [
        new VsGridSimpleColumn({
          headerName: 'ProcessamentoInspecao.Transferencia.NumeroPedido',
          field: 'numeroPedido'
        })
      ];
    }

    this.gridOptions.columns = [
      ...this.gridOptions.columns,
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.Odf',
        field: 'ordemFabricacao',
        width: 80
      }),
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.Transferencia.TipoTransferencia',
        field: 'tipoTransferencia',
        format: (tipoTransferencia: TipoTransferencia) => `ProcessamentoInspecao.Transferencia.TipoTransferencias.${TipoTransferencia[tipoTransferencia]}`,
        translate: true,
      }),
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.Transferencia.Quantidade',
        field: 'quantidade',
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.Transferencia.DescricaoLocalOrigem',
        field: 'descricaoLocalOrigem'
      }),
      new VsGridSimpleColumn({
        headerName: 'ProcessamentoInspecao.Transferencia.DescricaoLocalDestino',
        field: 'descricaoLocalDestino'
      })
    ];

    this.gridOptions.get = () => this.getGridData();
  }

  private getGridData(): Observable<VsGridGetResult> {
    return of(new VsGridGetResult(this.transferencias, this.transferencias.length));
  }
}
