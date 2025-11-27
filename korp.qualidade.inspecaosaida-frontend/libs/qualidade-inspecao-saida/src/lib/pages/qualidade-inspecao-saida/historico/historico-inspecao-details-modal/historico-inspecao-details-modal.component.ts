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
  HistoricoInspecaoSaidaTransferenciaOutput,
  ResultadosInspecao,
  TipoTransferencia,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO
} from '../../../../tokens';
import { HistoricoInspecaoViewService } from '../historico-inspecao-view/historico-inspecao-view.service';
import { map } from 'rxjs/operators';
import { GetInspecaoSaidaItensDTO } from '../../../../tokens/interfaces/get-inspecao-saida-itens-dto.interface';
import { JQQB_COND_OR, JQQB_OP_EQUAL } from '@viasoft/common';

@Component({
  selector: 'qa-historico-inspecao-details-modal',
  templateUrl: './historico-inspecao-details-modal.component.html',
  styleUrls: ['./historico-inspecao-details-modal.component.scss'],
  providers: [DecimalPipe]
})
export class HistoricoInspecaoDetailsModalComponent {
  public gridOptions: VsGridOptions;
  public gridMetricasOptions: VsGridOptions;
  public idInspecao: string;
  public selectedTab: number;

  constructor(private decimalPipe: DecimalPipe, private service: HistoricoInspecaoViewService,
    @Inject(MAT_DIALOG_DATA) private data: { idInspecao, codigoInspecao, transferencias: HistoricoInspecaoSaidaTransferenciaOutput[] }) {
    this.idInspecao = data.idInspecao;
    this.service.getParametroBool(UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO, UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO)
      .then((utilizarReservaPedido: boolean) => {
        this.iniciarGridTransferencias(utilizarReservaPedido);
        this.iniciarGridMetricas();
      });
  }

  public onSelectedTabIndexChanged(index: number): void {
    this.selectedTab = index;
  }

  private iniciarGridMetricas() {
    this.gridMetricasOptions = new VsGridOptions();
    this.gridMetricasOptions.id = '95F19068-FEA8-4ADF-AF50-63B680B5D104';

    this.gridMetricasOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.Descricao',
        field: 'descricao',
        width: 100,
        filterOptions: {
          useField: 'QA_ITEM_INSPECAO_SAIDA.DESCRICAO'
        },
        sorting: {
          useField: 'QA_ITEM_INSPECAO_SAIDA.DESCRICAO'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.Resultado',
        field: 'resultado',
        width: 100,
        filterOptions: this.resultadoFilterOptions,
        sorting: {
          useField: 'QA_ITEM_INSPECAO_SAIDA.RESULTADO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.MenorValor',
        field: 'menorValor',
        width: 50,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          useField: 'QA_ITEM_INSPECAO_SAIDA.MENORVALOR'
        },
        sorting: {
          useField: 'QA_ITEM_INSPECAO_SAIDA.MENORVALOR'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.MaiorValor',
        field: 'maiorValor',
        width: 50,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          useField: 'QA_ITEM_INSPECAO_SAIDA.MAIORVALOR'
        },
        sorting: {
          useField: 'QA_ITEM_INSPECAO_SAIDA.MAIORVALOR'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.MenorValorBase',
        field: 'menorValorBase',
        width: 70,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.MaiorValorBase',
        field: 'maiorValorBase',
        width: 70,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.Observacao',
        field: 'observacao',
        width: 100,
        filterOptions: {
          useField: 'QA_ITEM_INSPECAO_SAIDA.OBSERVACAO'
        },
        sorting: {
          useField: 'QA_ITEM_INSPECAO_SAIDA.OBSERVACAO'
        }
      }),
    ];

    this.gridMetricasOptions.get = (input) => this.getGridMetricasData(input);
  }

  private iniciarGridTransferencias(utilizarReservaPedido: boolean): void {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = '01FFEF50-7955-4C01-9442-B987F0910649';
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
        field: 'odfApontada',
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

  private get resultadoFilterOptions(): VsFilterOptions {
    return {
      operators: [JQQB_OP_EQUAL],
      conditions: [JQQB_COND_OR],
      blockInput: true,
      useField: 'QA_ITEM_INSPECAO_SAIDA.RESULTADO',
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
        totalCount: 4
      })
    };
  }
}
