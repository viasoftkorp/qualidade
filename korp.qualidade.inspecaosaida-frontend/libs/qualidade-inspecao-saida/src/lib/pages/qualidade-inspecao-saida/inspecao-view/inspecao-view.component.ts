import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  OnDestroy,
  OnInit
} from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import {
  JQQB_COND_OR,
  JQQB_OP_EQUAL,
  MessageService,
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
import { QualidadeInspecaoSaidaService } from '../../../services/qualidade-inspecao-saida.service';
import {
  formatNumberToDecimal,
  getErrorMessage,
  GetInspecaoSaidaDTO,
  InspecaoDetailsDTO,
  InspecaoSaidaDTO,
  OrdemProducaoDTO,
  ResultadosInspecao,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO
} from '../../../tokens';
import { InspecaoDetailsComponent } from '../inspecao-details/inspecao-details.component';

@Component({
  selector: 'qa-inspecao-view',
  templateUrl: './inspecao-view.component.html',
  styleUrls: ['./inspecao-view.component.scss'],
  providers: [DecimalPipe]
})
export class InspecaoViewComponent implements OnInit, OnDestroy {
  private subs = new VsSubscriptionManager();
  private ordemDto: OrdemProducaoDTO;
  public gridOptions: VsGridOptions;
  public utilizarReservaPedido = false;

  private processandoImprimir = false;
  private atualizarProcessandoImprimir = new Subject<void>();

  constructor(
    private vsDialog: VsDialog,
    private matDialog: MatDialog,
    private inspecaoSaidaService: QualidadeInspecaoSaidaService,
    private router: Router,
    private messageService: MessageService,
    private decimalPipe: DecimalPipe,
  ) {
  }

  async ngOnInit(): Promise<void> {
    this.subs.add('ordem-selecionada-inspecao-view', this.inspecaoSaidaService
      .ordemSelecionada
      .subscribe((odf: OrdemProducaoDTO) => {
        this.ordemDto = odf;
      }));

    this.subs.add('refresh-inspecoes-nota-fiscal-grid', this.inspecaoSaidaService
      .refreshInspecoesOrdemGrid
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
    this.gridOptions.id = '38FBA571-6E2C-4884-A201-CEBF06DBF8C2';

    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.TipoInspecao',
        field: 'tipoInspecao',
        width: 130,
        filterOptions: {
          useField: 'TIPOINSP'
        },
        sorting: {
          useField: 'TIPOINSP'
        }
      }),
      /* new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.Odf',
        field: 'odf',
        width: 80,
      }), */
      new VsGridDateColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.DataInspecao',
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
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.Inspetor',
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
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.Resultado',
        field: 'resultado',
        width: 150,
        filterOptions: this.resultadoFilterOptions,
        sorting: {
          useField: 'RESULTADO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.QuantidadeInspecao',
        field: 'quantidadeInspecao',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          useField: 'QTD_INSPECAO'
        },
        sorting: {
          useField: 'QTD_INSPECAO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.QuantidadeLote',
        field: 'quantidadeLote',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          useField: 'QTD_LOTE'
        },
        sorting: {
          useField: 'QTD_LOTE'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.QuantidadeAceita',
        field: 'quantidadeAceita',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          useField: 'QTD_ACEITO'
        },
        sorting: {
          useField: 'QTD_ACEITO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.QuantidadeRetrabalhada',
        field: 'quantidadeRetrabalhada',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          useField: 'QTD_RETRABALHO'
        },
        sorting: {
          useField: 'QTD_RETRABALHO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.QuantidadeAprovada',
        field: 'quantidadeAprovada',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          useField: 'QTD_APROVADO'
        },
        sorting: {
          useField: 'QTD_APROVADO'
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.QuantidadeReprovada',
        field: 'quantidadeReprovada',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          useField: 'QTD_REJEITADO'
        },
        sorting: {
          useField: 'QTD_REJEITADO'
        }
      }),
    ];

    this.gridOptions.get = (input: VsGridGetInput) => this.getGridData(input);
    this.gridOptions.select = (rowIndex, data: InspecaoSaidaDTO) => this.editarInspecao(data);

    this.gridOptions.actions = [{
      icon: 'search',
      tooltip: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.EditarInspecao',
      callback: (rowIndex: number, data: InspecaoSaidaDTO) => this.editarInspecao(data)
    }, {
      icon: 'trash-alt',
      tooltip: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.ExcluirInspecao',
      callback: (rowIndex: number, data: InspecaoSaidaDTO) => this.excluirInspecao(data.codigoInspecao),
    }, {
      icon: 'print',
      tooltip: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.ImprimirInspecao',
      callback: (rowIndex: number, data: InspecaoSaidaDTO) => this.imprimirInspecao(data),
      disabled: () => this.processandoImprimir,
      refreshSubject: this.atualizarProcessandoImprimir
    }];
  }

  private getGridData(input: VsGridGetInput): Observable<VsGridGetResult> {
    if (!this.ordemDto) {
      return of(new VsGridGetResult(null, 0));
    }

    const filtros = this.inspecaoSaidaService.getFiltros();

    return this.inspecaoSaidaService
      .getInspecoesSaida(input, this.ordemDto.odf, filtros)
      .pipe(
        map((r: GetInspecaoSaidaDTO) => new VsGridGetResult(r.items, r.totalCount)),
      );
  }

  private editarInspecao(dto: InspecaoSaidaDTO): void {
    if (!dto.resultado) {
      // TODO workaround
      const positionTop = this.vsDialog['dialogTopPosition'] ?? 40;
      this.matDialog.open(InspecaoDetailsComponent, {
        data: {
          id: dto.id,
          odf: dto.odf,
          odfApontada: dto.odfApontada,
          codProduto: this.ordemDto.codigoProduto,
          plano: this.ordemDto.plano,
          novaInspecao: false,
          codInspecao: dto.codigoInspecao,
          quantidadeLote: dto.quantidadeLote,
          lote: dto.lote
        } as InspecaoDetailsDTO,
        hasBackdrop: true,
        closeOnNavigation: true,
        height: `calc(100% - ${positionTop}px)`,
        maxHeight: `calc(100% - ${positionTop}px)`,
        maxWidth: '60vw',
        position: { top: `${positionTop}px`, right: '0px', bottom: '0px' },
        panelClass: 'vs-dialog-panel'
      }).afterClosed().toPromise().then(() => {
        this.gridOptions.refresh();
        this.inspecaoSaidaService.refreshOrdemGrid.next();
      });
    }
  }

  private excluirInspecao(codigoInspecao: number): void {
    this.inspecaoSaidaService.excluirInspecaoSaida(codigoInspecao).subscribe(() => {
      this.gridOptions.refresh(true);
      this.inspecaoSaidaService.refreshOrdemGrid.next();
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
            value: `QualidadeInspecaoSaida.Resultados.Aprovado`,
          },
          {
            key: ResultadosInspecao.ParcialmenteAprovado.toString(),
            value: `QualidadeInspecaoSaida.Resultados.ParcialmenteAprovado`,
          },
          {
            key: ResultadosInspecao.NaoAplicavel.toString(),
            value: `QualidadeInspecaoSaida.Resultados.NaoAplicavel`,
          },
          {
            key: ResultadosInspecao.NaoConforme.toString(),
            value: `QualidadeInspecaoSaida.Resultados.NaoConforme`,
          }
        ],
        totalCount: 4
      })
    };
  }

  private imprimirInspecao(inspecaoSaida: InspecaoSaidaDTO) {
    if (this.processandoImprimir) {
      return null;
    }

    this.processandoImprimir = true;
    this.atualizarProcessandoImprimir.next();

    this.inspecaoSaidaService.imprimirInspecaoSaida(inspecaoSaida.codigoInspecao, null)
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
