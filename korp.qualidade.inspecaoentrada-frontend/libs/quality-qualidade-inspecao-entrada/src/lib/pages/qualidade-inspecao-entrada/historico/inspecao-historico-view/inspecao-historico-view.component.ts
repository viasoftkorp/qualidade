import {
  Component, OnDestroy
} from '@angular/core';
import { Router } from '@angular/router';
import {
  JQQB_COND_OR,
  JQQB_OP_EQUAL,
  VsSubscriptionManager
} from '@viasoft/common';
import {
  VsDialog,
  VsFilterOptions,
  VsGridDateColumn,
  VsGridGetInput,
  VsGridGetResult,
  VsGridNumberColumn,
  VsGridOptions,
  VsGridSimpleColumn,
} from '@viasoft/components';
import { Observable, of, Subject } from 'rxjs';
import { finalize, map } from 'rxjs/operators';
import {
  GetAllHistoricoInspecaoEntradaItensOutput,
  GetAllHistoricoInspecaoEntradaOutput,
  HistoricoInspecaoEntradaItensOutput,
  NotaFiscalDadosAdicionaisDTO,
  NotaFiscalDTO,
  ResultadosInspecao
} from '../../../../tokens';
import { HistoricoInspecaoDetailsModalComponent } from '../historico-inspecao-details-modal/historico-inspecao-details-modal.component';
import { HistoricoInspecaoViewService } from './historico-inspecao-view.service';
import { RncEditorModalService } from '@viasoft/rnc-lib';
import { QualidadeInspecaoEntradaService } from '../../../../services/qualidade-inspecao-entrada.service';

@Component({
  selector: 'qa-inspecao-historico-view',
  templateUrl: './inspecao-historico-view.component.html',
  styleUrls: ['./inspecao-historico-view.component.scss'],
})
export class InspecaoHistoricoViewComponent implements OnDestroy {
  public notaSelecionada: NotaFiscalDTO;
  public gridOptionsHeader: VsGridOptions = new VsGridOptions();
  public gridOptionsItens: VsGridOptions = new VsGridOptions();

  private subs: VsSubscriptionManager = new VsSubscriptionManager();
  private colunasEditaveis = ['observacao'];

  private processandoImprimir = false;
  private atualizarProcessandoImprimir = new Subject<void>();

  constructor(
    private qualidadeInspecaoEntradaService: QualidadeInspecaoEntradaService,
    private service: HistoricoInspecaoViewService,
    private vsDialog: VsDialog,
    private rncEditorModalService: RncEditorModalService,
    private router: Router)
  {
    this.subscriptions();
    this.iniciarGridHeader();
    this.iniciarGridItens();
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  private subscriptions(): void {
    this.subs.add('nota-fiscal-historico-selecionada', this.qualidadeInspecaoEntradaService
      .notaFiscalHistoricoSelecionada
      .subscribe((notaFiscal: NotaFiscalDTO) => {
        this.notaSelecionada = notaFiscal;
      }));
  }

  private iniciarGridHeader(): void {
    this.gridOptionsHeader.id = '36D39AE3-B52D-43FE-83CE-B30AD5022EBD';
    this.gridOptionsHeader.sizeColumnsToFit = false;

    this.gridOptionsHeader.columns = [
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Plano',
        field: 'plano',
        width: 110,
        filterOptions: {
          useField: 'ESTOQUE.PLAINS'
        },
        sorting: {
          useField: 'ESTOQUE.PLAINS'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.DescricaoPlano',
        field: 'descricaoPlano',
        width: 180,
        filterOptions: {
          useField: 'PLANOINS_CABECALHO.DESCRICAO'
        },
        sorting: {
          useField: 'PLANOINS_CABECALHO.DESCRICAO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.NotaFiscal',
        field: 'notaFiscal',
        width: 80,
        filterOptions: {
          useField: 'HISTLISE.NFISCAL'
        },
        sorting: {
          useField: 'HISTLISE.NFISCAL'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Lote',
        field: 'lote',
        width: 80,
        filterOptions: {
          useField: 'QA_INSPECAO_ENTRADA.LOTE'
        },
        sorting: {
          useField: 'QA_INSPECAO_ENTRADA.LOTE'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Observacao',
        field: 'observacao',
        width: 250,
        filterOptions: {
          useField: 'QA_INSPECAO_ENTRADA_NOTA_FISCAL_DADOS_ADICIONAIS.OBSERVACAO'
        },
        sorting: {
          useField:  'QA_INSPECAO_ENTRADA_NOTA_FISCAL_DADOS_ADICIONAIS.OBSERVACAO'
        },
        tooltip: (data: NotaFiscalDTO) => data.observacao
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.CodigoProduto',
        field: 'codigoProduto',
        width: 110,
        filterOptions: {
          useField: 'HISTLISE.ITEM'
        },
        sorting: {
          useField: 'HISTLISE.ITEM'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.DescricaoProduto',
        field: 'descricaoProduto',
        width: 190,
        filterOptions: {
          useField: 'HISTLISE.DESCRI'
        },
        sorting: {
          useField: 'HISTLISE.DESCRI'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.DescricaoFornecedor',
        field: 'descricaoForneced',
        width: 220,
        filterOptions: {
          useField: 'FORNECED.RASSOC'
        },
        sorting: {
          useField: 'DescricaoForneced'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.QuantidadeLote',
        field: 'quantidade',
        width: 120,
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.QuantidadeInspecao',
        field: 'quantidadeInspecionada',
        width: 120,
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      })
    ];

    this.subs.add('refresh-historico-grid', this.qualidadeInspecaoEntradaService
      .refreshHistoricoGrid
      .subscribe(() => {
        this.gridOptionsHeader.refresh();
      }));

    this.subs.add('refresh-inspecoes-historico-grid', this.qualidadeInspecaoEntradaService
      .refreshInspecoesHistoricoGrid
      .subscribe(() => {
        this.gridOptionsItens.refresh();
      }));

    this.gridOptionsHeader.get = (input: VsGridGetInput) => this.getGridDataHeader(input);
    this.gridOptionsHeader.select = (rowIndex: number, data: NotaFiscalDTO) => {
      this.qualidadeInspecaoEntradaService.notaFiscalHistoricoSelecionada.next(data);
      this.gridOptionsItens.refresh();
    };

    this.gridOptionsHeader.editRowOptions = {
      editOnSingleClick: false,
      isAutoEditable: true,
      fullEditMode: true,
      shouldShowAction: () => false,
      isCellEditable: (_rowIndex, fieldName, _data: NotaFiscalDTO) =>
        this.colunasEditaveis.includes(fieldName),
      onRowEdit: (_index, _currentData: NotaFiscalDTO, newData: NotaFiscalDTO) => {
        const input = {
          idNotaFiscal: newData.id,
          observacao: newData.observacao
        } as NotaFiscalDadosAdicionaisDTO;

        return this.qualidadeInspecaoEntradaService.updateNotaFiscalDadosAdicionais(input.idNotaFiscal, input)
          .pipe(map(() => {
            return { success: true }
          }))
      }
    }
  }

  private iniciarGridItens(): void {
    this.gridOptionsItens.id = 'B2016002-CFC0-4AAE-A55A-F73FC1DE375F';
    this.gridOptionsItens.sizeColumnsToFit = false;

    this.gridOptionsItens.columns = [
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.NotaFiscal',
        field: 'notaFiscal',
        width: 80,
        filterOptions: {
          useField: 'QA_INSPECAO_ENTRADA.CODNOTA'
        },
        sorting: {
          useField: 'QA_INSPECAO_ENTRADA.CODNOTA'
        }
      }),
      new VsGridDateColumn({
        headerName: 'HistoricoInspecao.DataInspecao',
        field: 'dataInspecao',
        width: 100,
        filterOptions: {
          useField: 'QA_INSPECAO_ENTRADA.DATAINSP'
        },
        sorting: {
          useField: 'QA_INSPECAO_ENTRADA.DATAINSP'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Inspetor',
        field: 'inspetor',
        width: 150,
        filterOptions: {
          useField: 'QA_INSPECAO_ENTRADA.INSPETOR'
        },
        sorting: {
          useField: 'QA_INSPECAO_ENTRADA.INSPETOR'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Resultado',
        field: 'resultado',
        width: 150,
        filterOptions: this.resultadoFilterOptions,
        sorting: {
          useField: 'QA_INSPECAO_ENTRADA.RESULTADO'
        }
      }),
      // new VsGridSimpleColumn({
      //   headerName: 'HistoricoInspecao.CodigoProduto',
      //   field: 'codigoProduto',
      //   width: 100
      // }),
      // new VsGridSimpleColumn({
      //   headerName: 'HistoricoInspecao.DescricaoProduto',
      //   field: 'descricaoProduto',
      //   width: 140
      // }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.QuantidadeInspecao',
        field: 'quantidadeInspecao',
        width: 180,
        filterOptions: {
          useField: 'QA_INSPECAO_ENTRADA.QTD_INSPECAO'
        },
        sorting: {
          useField: 'QA_INSPECAO_ENTRADA.QTD_INSPECAO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.QuantidadeAprovada',
        field: 'quantidadeAprovada',
        width: 180,
        filterOptions: {
          useField: 'QA_INSPECAO_ENTRADA.QTD_APROVADO'
        },
        sorting: {
          useField: 'QA_INSPECAO_ENTRADA.QTD_APROVADO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.QuantidadeReprovada',
        field: 'quantidadeReprovada',
        width: 180,
        filterOptions: {
          useField: 'QA_INSPECAO_ENTRADA.QTD_REJEITADO'
        },
        sorting: {
          useField: 'QA_INSPECAO_ENTRADA.QTD_REJEITADO'
        }
      }),
    ];

    this.gridOptionsItens.get = (input: VsGridGetInput) => this.getGridDataItens(input);
    this.gridOptionsItens.edit = (index: number, itemHistorico: HistoricoInspecaoEntradaItensOutput) => {
      this.vsDialog.open(HistoricoInspecaoDetailsModalComponent, { codigoProduto: itemHistorico.codigoProduto, codigoInspecao: itemHistorico.codigoInspecao, transferencias: itemHistorico.transferencias });
    };
    this.gridOptionsItens.select = (index: number, itemHistorico: HistoricoInspecaoEntradaItensOutput) => {
      this.vsDialog.open(HistoricoInspecaoDetailsModalComponent, { codigoProduto: itemHistorico.codigoProduto, codigoInspecao: itemHistorico.codigoInspecao, transferencias: itemHistorico.transferencias });
    };
    this.gridOptionsItens.actions = [
      {
        icon: 'fragile',
        tooltip: 'HistoricoInspecao.Rnc',
        condition: (row, data) => data.idRnc != undefined,
        callback: (rowIndex: number, data: HistoricoInspecaoEntradaItensOutput) => this.openRncReadonly(data.idRnc)
      },
      {
      icon: 'redo',
      tooltip: 'HistoricoInspecao.Estornar',
      callback: (rowIndex: number, data: HistoricoInspecaoEntradaItensOutput) => this.estornar(data.recnoInspecao)
      },
      {
        icon: 'print',
        tooltip: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.ImprimirInspecao',
        callback: (rowIndex: number, data: HistoricoInspecaoEntradaItensOutput) => this.imprimirInspecao(data),
        disabled: () => this.processandoImprimir,
        refreshSubject: this.atualizarProcessandoImprimir
      }];
  }

  private getGridDataHeader(input: VsGridGetInput): Observable<VsGridGetResult> {
    const filtros = this.qualidadeInspecaoEntradaService.getFiltros();

    return this.service.get(input, filtros)
      .pipe(
        map((
          pagedHistorico: GetAllHistoricoInspecaoEntradaOutput
        ) => new VsGridGetResult(pagedHistorico.items, pagedHistorico.totalCount))
      );
  }

  private getGridDataItens(input: VsGridGetInput): Observable<VsGridGetResult> {
    if (!this.notaSelecionada) {
      return of(new VsGridGetResult([], 0));
    }

    const filtros = this.qualidadeInspecaoEntradaService.getFiltros();

    return this.service.getItens(input, this.notaSelecionada, filtros)
      .pipe(
        map((
          pagedHistorico: GetAllHistoricoInspecaoEntradaItensOutput
        ) => new VsGridGetResult(pagedHistorico.items, pagedHistorico.totalCount))
      );
  }

  private estornar(recnoInspecao: number): void {
    this.subs.add('estornar', this.service.estornar(recnoInspecao).subscribe(() => {
      this.router.navigate(['/processamento']);
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
      useField: 'QA_INSPECAO_ENTRADA.RESULTADO',
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


  private imprimirInspecao(inspecaoSaida: HistoricoInspecaoEntradaItensOutput) {
    if (this.processandoImprimir) {
      return null;
    }

    this.processandoImprimir = true;
    this.atualizarProcessandoImprimir.next();

    this.qualidadeInspecaoEntradaService.imprimirInspecaoSaida(inspecaoSaida.codigoInspecao)
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
