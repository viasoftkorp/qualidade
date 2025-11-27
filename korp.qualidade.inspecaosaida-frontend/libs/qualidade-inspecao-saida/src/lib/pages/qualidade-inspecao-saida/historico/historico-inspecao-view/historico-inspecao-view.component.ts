import { Component, OnDestroy } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { Observable, of, Subject } from 'rxjs';
import { finalize, map } from 'rxjs/operators';
import { JQQB_COND_OR, JQQB_OP_EQUAL, VsSubscriptionManager } from '@viasoft/common';
import {
  VsDialog,
  VsFilterOptions,
  VsGridDateColumn,
  VsGridGetInput,
  VsGridGetResult,
  VsGridNumberColumn,
  VsGridOptions,
  VsGridSimpleColumn
} from '@viasoft/components';
import {
  formatNumberToDecimal,
  GetAllHistoricoInspecaoSaidaItensOutput,
  GetAllHistoricoInspecaoSaidaOutput,
  HistoricoInspecaoSaidaItensOutput,
  HistoricoInspecaoSaidaOutput,
  ResultadosInspecao,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO
} from '../../../../tokens';
import { HistoricoInspecaoViewService } from './historico-inspecao-view.service';
import { HistoricoInspecaoDetailsModalComponent } from '../historico-inspecao-details-modal/historico-inspecao-details-modal.component';
import { Router } from '@angular/router';
import { QualidadeInspecaoSaidaService } from "../../../../services/qualidade-inspecao-saida.service";
import { RncEditorModalService } from '@viasoft/rnc-lib';
import {MatDialog} from "@angular/material/dialog";
import {
  NotaFiscalDto,
  NotasSelectModalComponent
} from "../../../../components/notas-select-modal/notas-select-modal.component";
import {ConfiguracoesService} from "../../../configuracoes/configuracoes.service";
import {ConfiguracoesDto} from "../../../configuracoes/configuracoes.component";

@Component({
  selector: 'qa-historico-inspecao-view',
  templateUrl: './historico-inspecao-view.component.html',
  styleUrls: ['./historico-inspecao-view.component.scss'],
  providers: [DecimalPipe]
})
export class HistoricoInspecaoViewComponent implements OnDestroy {
  public gridOptionsHeader: VsGridOptions;
  public gridOptionsItens: VsGridOptions;
  public utilizarReservaPedido = false;
  public ordemSelecionada: HistoricoInspecaoSaidaOutput;
  private subs: VsSubscriptionManager = new VsSubscriptionManager();
  private processandoImprimir = false;
  private atualizarProcessandoImprimir = new Subject<void>();

  constructor(
    private service: HistoricoInspecaoViewService,
    private decimalPipe: DecimalPipe,
    private vsDialog: VsDialog,
    private router: Router,
    private rncEditorModalService: RncEditorModalService,
    private qualidadeInspecaoSaidaService: QualidadeInspecaoSaidaService,
    private matDialog: MatDialog,
    private configuracoesService: ConfiguracoesService)
  {
    this.qualidadeInspecaoSaidaService.getParametroBool(
      UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO, UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO
    ).then((param) => {
      this.subscriptions();
      this.utilizarReservaPedido = param
      this.iniciarGridHeader();
      this.iniciarGridItens();
    });
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  private subscriptions(): void {
    this.subs.add('ordem-historico-selecionada', this.qualidadeInspecaoSaidaService
      .ordemHistoricoSelecionada
      .subscribe((ordemHistorico: HistoricoInspecaoSaidaOutput) => {
        this.ordemSelecionada = ordemHistorico;
      }));
  }

  private iniciarGridHeader(): void {
    this.gridOptionsHeader = new VsGridOptions();
    this.gridOptionsHeader.id = 'A840A551-ABAF-492B-A395-C0B19289427F';

    this.gridOptionsHeader.columns = [];

    if (this.utilizarReservaPedido) {
      this.gridOptionsHeader.columns = [
        new VsGridSimpleColumn({
          headerName: 'HistoricoInspecao.Cliente',
          field: 'cliente',
          width: 100,
          filterOptions: {
            useField: 'Cliente'
          },
          sorting: {
            useField: 'Cliente'
          }
        }),
        new VsGridSimpleColumn({
          headerName: 'HistoricoInspecao.NumeroPedido',
          field: 'numeroPedido',
          width: 110,
          filterOptions: {
            useField: 'NumeroPedido'
          },
          sorting: {
            useField: 'NumeroPedido'
          }
        }),
      ];
    }

    this.gridOptionsHeader.columns = [
      ...this.gridOptionsHeader.columns,
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Plano',
        field: 'plano',
        width: 110,
        filterOptions: {
          useField: 'Plano'
        },
        sorting: {
          useField: 'Plano'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.DescricaoPlano',
        field: 'descricaoPlano',
        width: 180,
        filterOptions: {
          useField: 'DescricaoPlano'
        },
        sorting: {
          useField: 'DescricaoPlano'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.Odf',
        field: 'odfApontada',
        width: 80,
        filterOptions: {
          operators: [JQQB_OP_EQUAL],
          useField: 'OrdemFabricacao'
        },
        sorting: {
          useField: 'OrdemFabricacao'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.CodigoProduto',
        field: 'codigoProduto',
        width: 110,
        filterOptions: {
          useField: 'CodigoProduto'
        },
        sorting: {
          useField: 'CodigoProduto'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.DescricaoProduto',
        field: 'descricaoProduto',
        width: 190,
        filterOptions: {
          useField: 'DescricaoProduto'
        },
        sorting: {
          useField: 'DescricaoProduto'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Lote',
        field: 'lote',
        width: 80,
        filterOptions: {
          useField: 'Lote'
        },
        sorting: {
          useField: 'Lote'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Revisao',
        field: 'revisao',
        width: 100,
        filterOptions: {
          operators: [JQQB_OP_EQUAL],
          useField: 'Revisao'
        },
        sorting: {
          useField: 'Revisao'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.QuantidadeLote',
        field: 'quantidadeLote',
        width: 120,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          useField: 'QuantidadeLote'
        },
        sorting: {
          useField: 'QuantidadeLote'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.QuantidadeInspecao',
        field: 'quantidadeInspecao',
        width: 120,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          useField: 'QuantidadeInspecao'
        },
        sorting: {
          useField: 'QuantidadeInspecao'
        }
      })
    ];

    this.subs.add('refresh-historico-grid', this.qualidadeInspecaoSaidaService
      .refreshHistoricoGrid
      .subscribe(() => {
        this.gridOptionsHeader.refresh();
      }));

    this.subs.add('refresh-inspecoes-historico-grid', this.qualidadeInspecaoSaidaService
      .refreshInspecoesHistoricoGrid
      .subscribe(() => {
        this.gridOptionsItens.refresh();
      }));

    this.gridOptionsHeader.get = (input: VsGridGetInput) => this.getGridDataHeader(input);
    this.gridOptionsHeader.select = (rowIndex: number, data: HistoricoInspecaoSaidaOutput) => {
      this.qualidadeInspecaoSaidaService.ordemHistoricoSelecionada.next(data);
      this.gridOptionsItens.refresh();
    };
  }

  private iniciarGridItens(): void {
    this.gridOptionsItens = new VsGridOptions();
    this.gridOptionsItens.id = 'C256ED04-B989-424B-BE9C-B29DB0F624B5';

    this.gridOptionsItens.columns = [
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.TipoInspecao',
        field: 'tipoInspecao',
        width: 130,
        filterOptions: {
          useField: 'QA_INSPECAO_SAIDA.TIPOINSP'
        },
        sorting: {
          useField: 'QA_INSPECAO_SAIDA.TIPOINSP'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Odf',
        field: 'odfApontada',
        width: 90,
        filterOptions: {
          operators: [JQQB_OP_EQUAL]
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridDateColumn({
        headerName: 'HistoricoInspecao.DataInspecao',
        field: 'dataInspecao',
        filterOptions: {
          useField: 'QA_INSPECAO_SAIDA.DATAINSP'
        },
        sorting: {
          useField: 'QA_INSPECAO_SAIDA.DATAINSP'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Inspetor',
        field: 'inspetor',
        filterOptions: {
          useField: 'QA_INSPECAO_SAIDA.INSPETOR'
        },
        sorting: {
          useField: 'QA_INSPECAO_SAIDA.INSPETOR'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Resultado',
        field: 'resultado',
        filterOptions: this.resultadoFilterOptions,
        sorting: {
          useField: 'QA_INSPECAO_SAIDA.RESULTADO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.QuantidadeTotal',
        field: 'quantidadeInspecao',
        width: 150,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          useField: 'QA_INSPECAO_SAIDA.QTD_INSPECAO'
        },
        sorting: {
          useField: 'QA_INSPECAO_SAIDA.QTD_INSPECAO'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.OdfRetrabalho',
        field: 'odfRetrabalho',
        width: 90,
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.CodigoRnc',
        field: 'codigoRnc',
        width: 90,
        filterOptions: {
          useField: 'InspecaoSaidaExecutadoWeb.CODIGO_RNC'
        },
        sorting: {
          useField: 'InspecaoSaidaExecutadoWeb.CODIGO_RNC'
        }
      }),
      // new VsGridSimpleColumn({
      //   headerName: 'HistoricoInspecao.Produto',
      //   field: 'descricaoProduto',
      //   width: 500
      // }),
    ];

    this.gridOptionsItens.get = (input: VsGridGetInput) => this.getGridDataItens(input);
    this.gridOptionsItens.edit = (index: number, itemHistorico: HistoricoInspecaoSaidaItensOutput) => {
      this.vsDialog.open(HistoricoInspecaoDetailsModalComponent, { idInspecao: itemHistorico.idInspecao, codigoInspecao: itemHistorico.codigoInspecao, transferencias: itemHistorico.transferencias });
    };
    this.gridOptionsItens.select = (index: number, itemHistorico: HistoricoInspecaoSaidaItensOutput) => {
      this.vsDialog.open(HistoricoInspecaoDetailsModalComponent, { idInspecao: itemHistorico.idInspecao, codigoInspecao: itemHistorico.codigoInspecao, transferencias: itemHistorico.transferencias });
    };
    this.gridOptionsItens.actions = [
      {
        icon: 'fragile',
        tooltip: 'HistoricoInspecao.Rnc',
        condition: (row, data) => data.idRnc != undefined,
        callback: (rowIndex: number, data: HistoricoInspecaoSaidaItensOutput) => this.openRncReadonly(data.idRnc)
      },
      {
        icon: 'redo',
        tooltip: 'HistoricoInspecao.Estornar',
        callback: (rowIndex: number, data: HistoricoInspecaoSaidaItensOutput) => this.estornar(data.recnoInspecao)
      },
      {
        icon: 'print',
        tooltip: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.ImprimirInspecao',
        callback: (rowIndex: number, data: HistoricoInspecaoSaidaItensOutput) => {
          this.configuracoesService.getConfiguracao()
            .subscribe((configuracoes: ConfiguracoesDto) => {
              if (configuracoes.usarNotaImpressaoRelatorio) {
                this.abrirSelecaoNotas(data.codigoInspecao);
              } else {
                this.imprimirInspecao(data.codigoInspecao, null);
              }
            });
        },
        disabled: () => this.processandoImprimir,
        refreshSubject: this.atualizarProcessandoImprimir
      },
    ];
  }

  private getGridDataHeader(input: VsGridGetInput): Observable<VsGridGetResult> {
    const filtros = this.qualidadeInspecaoSaidaService.getFiltros();

    return this.service.get(input, filtros)
      .pipe(
        map((pagedHistorico: GetAllHistoricoInspecaoSaidaOutput) => {
          return new VsGridGetResult(pagedHistorico.items, pagedHistorico.totalCount);
        })
      );
  }

  private getGridDataItens(input: VsGridGetInput): Observable<VsGridGetResult> {
    if (!this.ordemSelecionada) {
      return of(new VsGridGetResult([], 0));
    }

    const filtros = this.qualidadeInspecaoSaidaService.getFiltros();

    return this.service.getItens(input, this.ordemSelecionada.ordemFabricacao, this.ordemSelecionada.codigoInspecao, filtros)
      .pipe(
        map((pagedHistorico: GetAllHistoricoInspecaoSaidaItensOutput) => {
          return new VsGridGetResult(pagedHistorico.items, pagedHistorico.totalCount);
        })
      );
  }

  private estornar(recnoInspecao: number): void {
    this.subs.add('estornar', this.service.estornar(recnoInspecao)
      .subscribe(() => {
        this.router.navigate(['/processamento']);
        this.gridOptionsItens.refresh();
      }));
  }

  private openRncReadonly(idRnc) {
    this.rncEditorModalService.openReadRncModal(idRnc);
  }

  private get resultadoFilterOptions(): VsFilterOptions {
    return {
      operators: [JQQB_OP_EQUAL],
      conditions: [JQQB_COND_OR],
      blockInput: true,
      useField: 'QA_INSPECAO_SAIDA.RESULTADO',
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

  private abrirSelecaoNotas(codigoInspecao: number): void {
    this.matDialog.open(NotasSelectModalComponent, {data: codigoInspecao})
      .afterClosed()
      .subscribe((nota: NotaFiscalDto) => {
        if (!nota) {
          return
        }

        const codigoNomeCliente = !nota.clienteCodigo ? "" : `${nota.clienteCodigo} - ${nota.clienteRazaoSocial}`;

        this.imprimirInspecao(codigoInspecao, nota)
      });
  }

  private imprimirInspecao(codigoInspecao: number, nota: NotaFiscalDto): void {
    if (this.processandoImprimir) {
      return null;
    }

    this.processandoImprimir = true;
    this.atualizarProcessandoImprimir.next();

    this.qualidadeInspecaoSaidaService.imprimirInspecaoSaida(codigoInspecao, nota)
    .pipe(
      finalize(() => {
        this.processandoImprimir = false;
        this.atualizarProcessandoImprimir.next();
      })
    )
    .subscribe((fileBytes: string) => {
      const byteArray = new Uint8Array(atob(fileBytes).split('').map(char => char.charCodeAt(0)));
      const blob = new Blob([byteArray], { type: 'application/pdf' });
      const url = URL.createObjectURL(blob);
      window.open(url, '_blank', 'noopener noreferrer');
    });
  }
}
