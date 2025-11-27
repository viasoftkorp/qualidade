import { Component, Inject } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Observable, of } from 'rxjs';

import {
  VsFilterOptions,
  VsGridGetResult,
  VsGridNumberColumn,
  VsGridOptions,
  VsGridSimpleColumn
} from '@viasoft/components';

import {
  formatNumberToDecimal,
  GetInspecaoEntradaItensDTO,
  HistoricoInspecaoEntradaTransferenciaOutput,
  ResultadosInspecao,
  TipoTransferencia,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO
} from '../../../../tokens';
import { HistoricoInspecaoViewService } from '../inspecao-historico-view/historico-inspecao-view.service';
import { map } from 'rxjs/operators';
import { JQQB_COND_OR, JQQB_OP_EQUAL } from "@viasoft/common";

@Component({
  selector: 'qa-historico-inspecao-details-modal',
  templateUrl: './historico-inspecao-details-modal.component.html',
  styleUrls: ['./historico-inspecao-details-modal.component.scss'],
  providers: [DecimalPipe]
})
export class HistoricoInspecaoDetailsModalComponent {

  public gridOptions: VsGridOptions;
  public gridMetricasOptions : VsGridOptions;

  constructor(private decimalPipe: DecimalPipe, private service: HistoricoInspecaoViewService,
              @Inject(MAT_DIALOG_DATA) private data: {codigoInspecao, codigoProduto, transferencias: HistoricoInspecaoEntradaTransferenciaOutput[]}) {
    this.service.getParametroBool(UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO, UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO)
      .then((utilizarReservaPedido: boolean) => {
        this.iniciarGrid(utilizarReservaPedido);
        this.iniciarGridMetricas();
      });
  }

  private iniciarGridMetricas() {
    this.gridMetricasOptions = new VsGridOptions();
    this.gridMetricasOptions.id = 'FCE3096F-8171-4DB0-A108-FDA236769AB4';

    this.gridMetricasOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.Descricao',
        field: 'descricao',
        width: 100,
        filterOptions: {
          useField: 'QA_ITEM_INSPECAO_ENTRADA.DESCRICAO_PLANO'
        },
        sorting: {
          useField: 'QA_ITEM_INSPECAO_ENTRADA.DESCRICAO_PLANO'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.Resultado',
        field: 'resultado',
        width: 100,
        filterOptions: this.resultadoFilterOptions,
        sorting: {
          useField: 'QA_ITEM_INSPECAO_ENTRADA.RESULTADO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.MenorValor',
        field: 'menorValorInspecionado',
        width: 50,
        filterOptions: {
          useField: 'QA_ITEM_INSPECAO_ENTRADA.MENORVALOR'
        },
        sorting: {
          useField: 'QA_ITEM_INSPECAO_ENTRADA.MENORVALOR'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.MaiorValor',
        field: 'maiorValorInspecionado',
        width: 50,
        filterOptions: {
          useField: 'QA_ITEM_INSPECAO_ENTRADA.MAIORVALOR'
        },
        sorting: {
          useField: 'QA_ITEM_INSPECAO_ENTRADA.MAIORVALOR'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.MenorValorBase',
        field: 'menorValorBase',
        width: 70,
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.MaiorValorBase',
        field: 'maiorValorBase',
        width: 70,
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.Observacao',
        field: 'observacao',
        width: 100,
        filterOptions: {
          useField: 'QA_ITEM_INSPECAO_ENTRADA.OBSERVACAO'
        },
        sorting: {
          useField: 'QA_ITEM_INSPECAO_ENTRADA.OBSERVACAO'
        }
      }),
    ];

    this.gridMetricasOptions.get = (input) => this.getGridMetricasData(input);
  }

  private iniciarGrid(utilizarReservaPedido: boolean): void {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = '63DF5D28-B301-482E-A4DF-32B3F5ACB3D6';
    this.gridOptions.enableFilter = false;
    this.gridOptions.enableQuickFilter = false;
    this.gridOptions.enableSorting = false;
    this.gridOptions.sizeColumnsToFit = false;

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
        headerName: 'HistoricoInspecao.Lote',
        field: 'lote',
        width: 100
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
    return this.service.getInspecaoEntradaItens(this.data.codigoInspecao, this.data.codigoProduto, input).pipe(
      map((result: GetInspecaoEntradaItensDTO) => {
        return new VsGridGetResult(result.items, result.totalCount);
      }));
  }

  private get resultadoFilterOptions(): VsFilterOptions {
    return {
      operators: [JQQB_OP_EQUAL],
      conditions: [JQQB_COND_OR],
      blockInput: true,
      useField: 'QA_ITEM_INSPECAO_ENTRADA.RESULTADO',
      mode: 'selection',
      multiple: true,
      getItems: () => of({
        items: [
          {
            key: ResultadosInspecao.Aprovado.toString(),
            value: `HistoricoInspecao.Resultados.Aprovado`,
          },
          {
            key: ResultadosInspecao.ParcialmenteAprovado.toString(),
            value: `HistoricoInspecao.Resultados.ParcialmenteAprovado`,
          },
          {
            key: ResultadosInspecao.NaoAplicavel.toString(),
            value: `HistoricoInspecao.Resultados.NaoAplicavel`,
          },
          {
            key: ResultadosInspecao.NaoConforme.toString(),
            value: `HistoricoInspecao.Resultados.NaoConforme`,
          }
        ],
        totalCount: 3
      })
    };
  }
}
