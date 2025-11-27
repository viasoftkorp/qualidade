import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  OnDestroy,
  OnInit
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {
  JQQB_COND_OR,
  JQQB_OP_EQUAL,
  MessageService,
  VsSubscriptionManager
} from '@viasoft/common';
import {
  VsDialog, VsFilterOptions,
  VsGridDateColumn,
  VsGridGetInput,
  VsGridGetResult,
  VsGridNumberColumn,
  VsGridOptions,
  VsGridSimpleColumn,
} from '@viasoft/components';
import { Observable, of, Subject } from 'rxjs';
import { finalize, map } from 'rxjs/operators';
import { QualidadeInspecaoEntradaService } from '../../../services/qualidade-inspecao-entrada.service';
import {
  GetInspecaoEntradaDTO, InspecaoDetailsDTO, InspecaoEntradaDTO, NotaFiscalDTO, ResultadosInspecao
} from '../../../tokens';
import { getErrorMessage } from '../../../tokens/functions';
import { InspecaoDetailsComponent } from '../inspecao-details/inspecao-details.component';

@Component({
  selector: 'qa-inspecao-view',
  templateUrl: './inspecao-view.component.html',
  styleUrls: ['./inspecao-view.component.scss'],
})
export class InspecaoViewComponent implements OnInit, OnDestroy {
  private subs = new VsSubscriptionManager();
  private notaDto: NotaFiscalDTO;
  public gridOptions = new VsGridOptions();

  private processandoImprimir = false;
  private atualizarProcessandoImprimir = new Subject<void>();

  constructor(
    private vsDialog: VsDialog,
    private matDialog: MatDialog,
    private inspecaoEntradaService: QualidadeInspecaoEntradaService,
    private messageService: MessageService
  ) {
  }

  ngOnInit(): void {
    this.subs.add('nota-fiscal-selecionada-inspecao-view', this.inspecaoEntradaService
      .notaFiscalSelecionada
      .subscribe((notaFiscal: NotaFiscalDTO) => {
        this.notaDto = notaFiscal;
      }));

    this.subs.add('refresh-inspecoes-nota-fiscal-grid', this.inspecaoEntradaService
      .refreshInspecoesNotaFiscalGrid
      .subscribe(() => {
        this.gridOptions.refresh();
      }));

    this.initGrid();
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  private initGrid(): void {
    this.gridOptions.id = '4FE92F28-C548-4A46-83AC-BB28C6D771AF';
    this.gridOptions.sizeColumnsToFit = false;

    this.gridOptions.columns = [
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscal',
        field: 'notaFiscal',
        width: 80,
        filterOptions: {
          useField: 'CODNOTA'
        },
        sorting: {
          useField: 'CODNOTA'
        }
      }),
      new VsGridDateColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.DataInspecao',
        field: 'dataInspecao',
        width: 100,
        filterOptions: {
          useField: 'DATAINSP'
        },
        sorting: {
          useField: 'DATAINSP'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.Inspetor',
        field: 'inspetor',
        width: 150,
        filterOptions: {
          useField: 'INSPETOR'
        },
        sorting: {
          useField: 'INSPETOR'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.Resultado',
        field: 'resultado',
        width: 150,
        filterOptions: this.resultadoFilterOptions,
        sorting: {
          useField: 'RESULTADO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.QuantidadeInspecao',
        field: 'quantidadeInspecao',
        width: 100,
        filterOptions: {
          useField: 'QTD_INSPECAO'
        },
        sorting: {
          useField: 'QTD_INSPECAO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.QuantidadeLote',
        field: 'quantidadeLote',
        width: 100,
        filterOptions: {
          useField: 'QTD_LOTE'
        },
        sorting: {
          useField: 'QTD_LOTE'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.QuantidadeAceita',
        field: 'quantidadeAceita',
        width: 100,
        filterOptions: {
          useField: 'QTD_ACEITO'
        },
        sorting: {
          useField: 'QTD_ACEITO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.QuantidadeAprovada',
        field: 'quantidadeAprovada',
        width: 100,
        filterOptions: {
          useField: 'QTD_APROVADO'
        },
        sorting: {
          useField: 'QTD_APROVADO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.QuantidadeReprovada',
        field: 'quantidadeReprovada',
        width: 100,
        filterOptions: {
          useField: 'QTD_REJEITADO'
        },
        sorting: {
          useField: 'QTD_REJEITADO'
        }
      }),
    ];

    this.gridOptions.get = (input: VsGridGetInput) => this.getGridData(input);
    this.gridOptions.select = (rowIndex, data: InspecaoEntradaDTO) => this.editarInspecao(data);

    this.gridOptions.actions = [{
      icon: 'search',
      tooltip: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.EditarInspecao',
      callback: (rowIndex: number, data: InspecaoEntradaDTO) => this.editarInspecao(data),
      condition: (rowIndex: number, data: InspecaoEntradaDTO) => !data.resultado
    }, {
      icon: 'trash-alt',
      tooltip: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.ExcluirInspecao',
      callback: (rowIndex: number, data: InspecaoEntradaDTO) => this.excluirInspecao(data.codigoInspecao),
    }, {
      icon: 'print',
      tooltip: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.ImprimirInspecao',
      callback: (rowIndex: number, data: InspecaoEntradaDTO) => this.imprimirInspecao(data),
      disabled: () => this.processandoImprimir,
      refreshSubject: this.atualizarProcessandoImprimir
    }];
  }

  private getGridData(input: VsGridGetInput): Observable<VsGridGetResult> {
    if (!this.notaDto) {
      return of(new VsGridGetResult([], 0));
    }

    const filtros = this.inspecaoEntradaService.getFiltros();

    return this.inspecaoEntradaService
      .getInspecoesEntrada(input, filtros, this.notaDto.notaFiscal, this.notaDto.lote)
      .pipe(
        map((r: GetInspecaoEntradaDTO) => new VsGridGetResult(r.items, r.totalCount)),
      );
  }

  private editarInspecao(dto: InspecaoEntradaDTO): void {
    if (!dto.resultado) {
      const data = {
        notaFiscal: this.notaDto,
        codigoProduto: this.notaDto.codigoProduto,
        codigoFornecedor: this.notaDto.codigoForneced,
        novaInspecao: false,
        codigoInspecao: dto.codigoInspecao,
      } as InspecaoDetailsDTO;
      const dialogOptions = this.vsDialog.generateDialogConfig(data, {
        hasBackdrop: true
      });
      this.matDialog.open(InspecaoDetailsComponent, dialogOptions).afterClosed().toPromise().then(() => {
        this.gridOptions.refresh();
        this.inspecaoEntradaService.refreshNotaFiscalGrid.next();
      });
    }
  }

  private excluirInspecao(codigoInspecao: number): void {
    this.inspecaoEntradaService.excluirInspecaoEntrada(codigoInspecao).subscribe(() => {
      this.gridOptions.refresh(true);
      this.inspecaoEntradaService.refreshNotaFiscalGrid.next();
    }, (err: HttpErrorResponse) => {
      this.messageService.error(getErrorMessage(err));
    });
  }

  private get resultadoFilterOptions(): VsFilterOptions {
    return {
      operators: [JQQB_OP_EQUAL],
      conditions: [JQQB_COND_OR],
      blockInput: true,
      useField: 'RESULTADO',
      mode: 'selection',
      multiple: true,
      getItems: () => of({
        items: [
          {
            key: ResultadosInspecao.Aprovado.toString(),
            value: `QualidadeInspecaoEntrada.Resultados.Aprovado`,
          },
          {
            key: ResultadosInspecao.ParcialmenteAprovado.toString(),
            value: `QualidadeInspecaoEntrada.Resultados.ParcialmenteAprovado`,
          },
          {
            key: ResultadosInspecao.NaoAplicavel.toString(),
            value: `QualidadeInspecaoEntrada.Resultados.NaoAplicavel`,
          },
          {
            key: ResultadosInspecao.NaoConforme.toString(),
            value: `QualidadeInspecaoEntrada.Resultados.NaoConforme`,
          }
        ],
        totalCount: 3
      })
    };
  }

  private imprimirInspecao(inspecaoSaida: InspecaoEntradaDTO) {
    if (this.processandoImprimir) {
      return null;
    }

    this.processandoImprimir = true;
    this.atualizarProcessandoImprimir.next();

    this.inspecaoEntradaService.imprimirInspecaoSaida(inspecaoSaida.codigoInspecao)
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
