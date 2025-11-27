import {
  Component,
  OnDestroy,
  OnInit
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {
  JQQB_OP_EQUAL,
  VsSubscriptionManager
} from '@viasoft/common';
import {
  VsDialog,
  VsGridDateColumn,
  VsGridGetInput,
  VsGridGetResult,
  VsGridNumberColumn,
  VsGridOptions,
  VsGridSimpleColumn
} from '@viasoft/components';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { QualidadeInspecaoEntradaService } from '../../../services/qualidade-inspecao-entrada.service';
import {
  GetNotasFiscaisDTO,
  InspecaoDetailsDTO,
  NotaFiscalDadosAdicionaisDTO,
  NotaFiscalDTO
} from '../../../tokens';
import { InspecaoDetailsComponent } from '../inspecao-details/inspecao-details.component';

@Component({
  selector: 'qa-nota-fiscal-view',
  templateUrl: './nota-fiscal-view.component.html',
  styleUrls: ['./nota-fiscal-view.component.scss'],
})
export class NotaFiscalViewComponent implements OnInit, OnDestroy {
  public gridOptions = new VsGridOptions();
  public pesquisa: string;
  private colunasEditaveis = ['observacao'];
  private subs = new VsSubscriptionManager();

  constructor(
    private vsDialog: VsDialog,
    private matDialog: MatDialog,
    private inspecaoEntradaService: QualidadeInspecaoEntradaService
  ) {
  }

  ngOnInit(): void {
    this.initGrid();
    this.subs.add('refresh-nota-fiscal-grid', this.inspecaoEntradaService
      .refreshNotaFiscalGrid
      .subscribe(() => {
        this.gridOptions.refresh();
      }));
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  private initGrid(): void {
    this.gridOptions.id = 'FA1B85C0-493B-4F5A-A498-C4AADCC853C9';
    this.gridOptions.sizeColumnsToFit = false;

    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.Plano',
        field: 'plano',
        width: 110,
        filterOptions: {
          useField: 'ESTOQUE.PLAINS'
        },
        sorting: {
          useField:  'ESTOQUE.PLAINS'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.DescricaoPlano',
        field: 'descricaoPlano',
        width: 180,
        filterOptions: {
          useField: 'PLANOINS_CABECALHO.DESCRICAO'
        },
        sorting: {
          useField:  'PLANOINS_CABECALHO.DESCRICAO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscal',
        field: 'notaFiscal',
        width: 80,
        filterOptions: {
          useField: 'HISTLISE.NFISCAL',
          defaultOperator: JQQB_OP_EQUAL
        },
        sorting: {
          useField:  'HISTLISE.NFISCAL'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.Lote',
        field: 'lote',
        width: 100,
        filterOptions: {
          operators: [JQQB_OP_EQUAL]
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.Observacao',
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
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.CodigoProduto',
        field: 'codigoProduto',
        width: 100,
        filterOptions: {
          useField: 'HISTLISE.ITEM'
        },
        sorting: {
          useField:  'HISTLISE.ITEM'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.DescricaoProduto',
        field: 'descricaoProduto',
        width: 300,
        filterOptions: {
          useField: 'HISTLISE.DESCRI'
        },
        sorting: {
          useField:  'HISTLISE.DESCRI'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.DescricaoForneced',
        field: 'descricaoForneced',
        width: 250,
        filterOptions: {
          useField: 'FORNECED.RASSOC'
        },
        sorting: {
          useField:  'DescricaoForneced'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.Quantidade',
        field: 'quantidade',
        width: 110,
        filterOptions: {
          operators: [JQQB_OP_EQUAL]
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.QuantidadeInspecionada',
        field: 'quantidadeInspecionada',
        width: 120,
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.QuantidadeInspecionar',
        field: 'quantidadeInspecionar',
        width: 110,
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridDateColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.DataEntrada',
        field: 'dataEntrada',
        width: 110,
        filterOptions: {
          useField: 'HISTLISE.DTENT'
        },
        sorting: {
          useField:  'HISTLISE.DTENT'
        }
      })
    ];

    this.gridOptions.get = (i: VsGridGetInput) => this.getGridData(i);
    this.gridOptions.select = (rowIndex: number, data: NotaFiscalDTO) => this.notaSelecionada(data);
    this.gridOptions.actions = [{
      icon: 'plus',
      tooltip: 'QualidadeInspecaoEntrada.InspecaoDetails.NovaInspecao',
      callback: (rowIndex: number, data: NotaFiscalDTO) => {
        this.notaSelecionada(data);
        const dialogOptions = this.vsDialog.generateDialogConfig(
          {
            notaFiscal: data,
            novaInspecao: true,
            codigoProduto: data.codigoProduto,
            codigoFornecedor: data.codigoForneced,
          } as InspecaoDetailsDTO,
          {
            hasBackdrop: true
          }
        );
        const openedDialog = this.matDialog.open(InspecaoDetailsComponent, dialogOptions);
        return openedDialog.afterClosed().subscribe(() => {
          this.gridOptions.refresh();
        });
      }
    }];

    this.gridOptions.editRowOptions = {
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

        return this.inspecaoEntradaService.updateNotaFiscalDadosAdicionais(input.idNotaFiscal, input)
          .pipe(map(() => {
            return { success: true }
          }))
      }
    }
  }

  private getGridData(input: VsGridGetInput): Observable<VsGridGetResult> {
    input.filter = this.pesquisa;
    const filtros = this.inspecaoEntradaService.getFiltros();

    return this.inspecaoEntradaService
      .getNotasFiscais(input, filtros)
      .pipe(
        map((r: GetNotasFiscaisDTO) => new VsGridGetResult(r.items, r.totalCount)),
      );
  }

  private notaSelecionada(nota: NotaFiscalDTO): void {
    this.inspecaoEntradaService.notaFiscalSelecionada.next(nota);
    this.inspecaoEntradaService.refreshInspecoesNotaFiscalGrid.next();
  }
}
