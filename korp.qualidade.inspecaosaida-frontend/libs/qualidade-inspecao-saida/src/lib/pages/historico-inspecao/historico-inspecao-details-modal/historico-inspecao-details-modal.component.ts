import { Component, Inject } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Observable, of } from 'rxjs';

import { VsGridGetResult, VsGridNumberColumn, VsGridOptions, VsGridSimpleColumn } from '@viasoft/components';

import {
  formatNumberToDecimal,
  HistoricoInspecaoSaidaTransferenciaOutput,
  TipoTransferencia,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO
} from '../../../tokens';
import { HistoricoInspecaoViewService } from '../historico-inspecao-view/historico-inspecao-view.service';
import { map } from 'rxjs/operators';
import { GetInspecaoSaidaItensDTO } from '../../../tokens/interfaces/get-inspecao-saida-itens-dto.interface';

@Component({
  selector: 'qa-historico-inspecao-details-modal',
  templateUrl: './historico-inspecao-details-modal.component.html',
  styleUrls: ['./historico-inspecao-details-modal.component.scss'],
  providers: [DecimalPipe]
})
export class HistoricoInspecaoDetailsModalComponent {

  public gridOptions: VsGridOptions;
  public gridMetricasOptions: VsGridOptions;

  constructor(private decimalPipe: DecimalPipe, private service: HistoricoInspecaoViewService,
    @Inject(MAT_DIALOG_DATA) private data: { codigoInspecao, transferencias: HistoricoInspecaoSaidaTransferenciaOutput[] }) {
    this.service.getParametroBool(UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO, UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO)
      .then((utilizarReservaPedido: boolean) => {
        this.iniciarGridTransferencias(utilizarReservaPedido);
        this.iniciarGridMetricas();
      });
  }

  private iniciarGridMetricas() {
    this.gridMetricasOptions = new VsGridOptions();
    this.gridMetricasOptions.id = 'dde705f6-5607-47d8-9040-2f64b23cbd89';
    this.gridMetricasOptions.enableFilter = false;
    this.gridMetricasOptions.enableQuickFilter = false;
    this.gridMetricasOptions.enableSorting = false;

    this.gridMetricasOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.Descricao',
        field: 'descricao',
        width: 100,
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.Resultado',
        field: 'resultado',
        width: 100,
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.MenorValor',
        field: 'menorValor',
        width: 50,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.MaiorValor',
        field: 'maiorValor',
        width: 50,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.MenorValorBase',
        field: 'menorValorBase',
        width: 70,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.MaiorValorBase',
        field: 'maiorValorBase',
        width: 70,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.Observacao',
        field: 'observacao',
        width: 100,
      }),
    ];
    this.gridMetricasOptions.get = (input) => this.getGridMetricasData(input);
  }

  private iniciarGridTransferencias(utilizarReservaPedido: boolean): void {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = '9d2f445d-df73-4d2f-9363-f3756355f494';
    this.gridOptions.enableSorting = false;
    this.gridOptions.enableQuickFilter = false;
    this.gridOptions.enableFilter = false;

    this.gridOptions.columns = [];

    if (utilizarReservaPedido) {
      this.gridOptions.columns = [
        new VsGridSimpleColumn({
          headerName: 'HistoricoInspecao.Transferencia.NumeroPedido',
          field: 'numeroPedido'
        })
      ];
    }

    this.gridOptions.columns = [
      ...this.gridOptions.columns,
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Odf',
        field: 'ordemFabricacao',
        width: 80
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Transferencia.TipoTransferencia',
        field: 'tipoTransferencia',
        format: (tipoTransferencia: TipoTransferencia) => `HistoricoInspecao.Transferencia.TipoTransferencias.${TipoTransferencia[tipoTransferencia]}`,
        translate: true,
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Transferencia.Quantidade',
        field: 'quantidade',
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Transferencia.DescricaoLocalOrigem',
        field: 'descricaoLocalOrigem'
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Transferencia.DescricaoLocalDestino',
        field: 'descricaoLocalDestino'
      })
    ];

    this.gridOptions.get = () => of(new VsGridGetResult(this.data.transferencias, this.data.transferencias.length));
  }

  private getGridMetricasData(input): Observable<VsGridGetResult> {
    return this.service.getInspecaoSaidaItens(this.data.codigoInspecao, input).pipe(
      map((result: GetInspecaoSaidaItensDTO) => {
        return new VsGridGetResult(result.items, result.totalCount);
      }));
  }
}
