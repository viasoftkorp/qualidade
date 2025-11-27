import {
  Component, OnDestroy, OnInit
} from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { JQQB_OP_EQUAL, VsSubscriptionManager } from '@viasoft/common';
import {
  VsDialog,
  VsGridDateColumn,
  VsGridGetInput,
  VsGridGetResult,
  VsGridNumberColumn,
  VsGridOptions,
  VsGridSimpleColumn
} from '@viasoft/components';
import {
  formatNumberToDecimal,
  GetOrdensProducaoDTO,
  InspecaoDetailsDTO,
  OrdemProducaoDTO,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO
} from '../../../tokens';
import { QualidadeInspecaoSaidaService } from '../../../services/qualidade-inspecao-saida.service';
import { InspecaoDetailsComponent } from '../inspecao-details/inspecao-details.component';

@Component({
  selector: 'qa-ordem-producao-view',
  templateUrl: './ordem-producao-view.component.html',
  styleUrls: ['./ordem-producao-view.component.scss'],
  providers: [DecimalPipe]
})
export class OrdemProducaoViewComponent implements OnInit, OnDestroy {
  public gridOptions: VsGridOptions;
  public utilizarReservaPedido = false;
  private subs = new VsSubscriptionManager();

  constructor(
    private vsDialog: VsDialog,
    private matDialog: MatDialog,
    private inspecaoSaidaService: QualidadeInspecaoSaidaService,
    private decimalPipe: DecimalPipe
  ) {
  }

  async ngOnInit(): Promise<void> {
    this.subs.add('refresh-ordem-grid', this.inspecaoSaidaService
      .refreshOrdemGrid
      .subscribe(() => {
        this.gridOptions.refresh();
      }));

    this.utilizarReservaPedido = await this.inspecaoSaidaService.getParametroBool(
      UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO, UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO
    );

    this.initGrid();
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  private initGrid(): void {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = '9E03CB3F-3925-4FE1-B343-273C05CE2D8C';

    this.gridOptions.columns = [];

    if (this.utilizarReservaPedido) {
      this.gridOptions.columns = [
        new VsGridSimpleColumn({
          headerName: 'QualidadeInspecaoSaida.Cliente',
          field: 'cliente',
          width: 90,
          filterOptions: {
            operators: [JQQB_OP_EQUAL]
          },
          sorting: {
            disable: true
          }
        }),
        new VsGridSimpleColumn({
          headerName: 'QualidadeInspecaoSaida.NumeroPedido',
          field: 'numeroPedido',
          width: 110,
          filterOptions: {
            operators: [JQQB_OP_EQUAL]
          },
          sorting: {
            disable: true
          }
        }),
      ];
    }

    this.gridOptions.columns = [
      ...this.gridOptions.columns,
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.Plano',
        field: 'plano',
        width: 110,
        filterOptions: {
          useField: 'PROCESSO.PLANO_INSP'
        },
        sorting: {
          useField: 'PROCESSO.PLANO_INSP'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.DescricaoPlano',
        field: 'descricaoPlano',
        width: 180,
        filterOptions: {
          useField: 'QA_PLANO_INS_SAIDA_CABECALHO.DESCRICAO'
        },
        sorting: {
          useField: 'QA_PLANO_INS_SAIDA_CABECALHO.DESCRICAO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.Odf',
        field: 'odfApontada',
        width: 80,
        filterOptions: {
          operators: [JQQB_OP_EQUAL]
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.CodigoProduto',
        field: 'codigoProduto',
        width: 100,
        filterOptions: {
          useField: 'CTE_SALDO_CQ.CODIGO'
        },
        sorting: {
          useField: 'CTE_SALDO_CQ.CODIGO'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.DescricaoProduto',
        field: 'descricaoProduto',
        width: 200,
        filterOptions: {
          useField: 'ESTOQUE.DESCRI'
        },
        sorting: {
          useField: 'ESTOQUE.DESCRI'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.Lote',
        field: 'lote',
        width: 50,
        filterOptions: {
          useField: 'CTE_SALDO_CQ.LOTE'
        },
        sorting: {
          useField: 'CTE_SALDO_CQ.LOTE'
        }
      }),
      /* new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.Situacao',
        field: 'situacao',
        width: 100,
      }), */
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.Revisao',
        field: 'revisao',
        width: 100,
        filterOptions: {
          operators: [JQQB_OP_EQUAL]
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.QuantidadeOrdem',
        field: 'quantidadeOrdem',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          operators: [JQQB_OP_EQUAL]
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.QuantidadeProduzida',
        field: 'quantidadeProduzida',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          useField: 'CTE_SALDO_CQ.QTRECEB_ENTRADA'
        },
        sorting: {
          useField: 'CTE_SALDO_CQ.QTRECEB_ENTRADA'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.QuantidadeInspecionada',
        field: 'quantidadeInspecionada',
        width: 120,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.QuantidadeInspecionar',
        field: 'quantidadeInspecionar',
        width: 110,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridDateColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.DataInicio',
        field: 'dataInicio',
        width: 100,
        filterOptions: {
          operators: [JQQB_OP_EQUAL]
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridDateColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.DataEntrega',
        field: 'dataEntrega',
        width: 100,
        filterOptions: {
          operators: [JQQB_OP_EQUAL]
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridDateColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.DataEmissao',
        field: 'dataEmissao',
        width: 100,
        filterOptions: {
          operators: [JQQB_OP_EQUAL]
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridDateColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.DataNegociada',
        field: 'dataNegociada',
        width: 110,
        filterOptions: {
          useField: 'PPEDLISE.DTNEGO'
        },
        sorting: {
          useField: 'PPEDLISE.DTNEGO'
        }
      })
    ];

    this.gridOptions.get = (i: VsGridGetInput) => this.getGridData(i);
    this.gridOptions.select = (rowIndex: number, data: OrdemProducaoDTO) => this.ordemSelecionada(data);
    this.gridOptions.actions = [{
      icon: 'plus',
      tooltip: 'QualidadeInspecaoSaida.OrdemProducaoGrid.NovaInspecao',
      callback: (rowIndex: number, data: OrdemProducaoDTO) => {
        // TODO workaround
        const positionTop = this.vsDialog['dialogTopPosition'] ?? 40;
        const openedDialog = this.matDialog.open(InspecaoDetailsComponent, {
          data: {
            odf: data.odf,
            odfApontada: data.odfApontada,
            codProduto: data.codigoProduto,
            plano: data.plano,
            novaInspecao: true,
            quantidadeInspecionada: data.quantidadeInspecionada,
            quantidadeInspecionar: data.quantidadeInspecionar,
            lote: data.lote,
            recnoProcesso: data.recnoProcesso
          } as InspecaoDetailsDTO,
          hasBackdrop: true,
          closeOnNavigation: true,
          height: `calc(100% - ${positionTop}px)`,
          maxHeight: `calc(100% - ${positionTop}px)`,
          maxWidth: '60vw',
          position: { top: `${positionTop}px`, right: '0px', bottom: '0px' },
          panelClass: 'vs-dialog-panel'
        });
        return openedDialog.afterClosed().toPromise().then(() => {
          this.inspecaoSaidaService.ordemSelecionada.next(data);
          this.inspecaoSaidaService.refreshInspecoesOrdemGrid.next();
          this.gridOptions.refresh();
        });
      }
    }];
  }

  private getGridData(input: VsGridGetInput): Observable<VsGridGetResult> {
    const filtros = this.inspecaoSaidaService.getFiltros();

    return this.inspecaoSaidaService
      .getOrdensInspecao(input, filtros)
      .pipe(
        map((r: GetOrdensProducaoDTO) => new VsGridGetResult(r.items, r.totalCount))
      );
  }

  private ordemSelecionada(odf: OrdemProducaoDTO): void {
    this.inspecaoSaidaService.ordemSelecionada.next(odf);
    this.inspecaoSaidaService.refreshInspecoesOrdemGrid.next();
  }
}
