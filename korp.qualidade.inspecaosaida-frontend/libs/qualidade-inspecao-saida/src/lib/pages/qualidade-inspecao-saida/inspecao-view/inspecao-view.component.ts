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
  MessageService,
  VsSubscriptionManager
} from '@viasoft/common';
import {
  VsDialog,
  VsGridDateColumn,
  VsGridGetInput,
  VsGridGetResult,
  VsGridNumberColumn,
  VsGridOptions,
  VsGridSimpleColumn,
} from '@viasoft/components';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { QualidadeInspecaoSaidaService } from '../../../services/qualidade-inspecao-saida.service';
import {
  formatNumberToDecimal,
  getErrorMessage,
  GetInspecaoSaidaDTO,
  InspecaoDetailsDTO,
  InspecaoSaidaDTO,
  OrdemProducaoDTO, UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO, UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO
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
    this.subs.add('get-odf', this.inspecaoSaidaService
      .ordemSelecionada
      .subscribe((odf: OrdemProducaoDTO) => {
        this.ordemDto = odf;
        this.gridOptions?.refresh();
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
    this.gridOptions.id = 'f4a80bcf-ae40-4d88-a393-034509444cdc';
    this.gridOptions.enableFilter = false;
    this.gridOptions.enableQuickFilter = false;
    this.gridOptions.enableSorting = false;

    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.TipoInspecao',
        field: 'tipoInspecao',
        width: 130
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
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.Inspetor',
        field: 'inspetor',
        width: 150,
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.Resultado',
        field: 'resultado',
        width: 150,
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.QuantidadeInspecao',
        field: 'quantidadeInspecao',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.QuantidadeLote',
        field: 'quantidadeLote',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.QuantidadeAceita',
        field: 'quantidadeAceita',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.QuantidadeRetrabalhada',
        field: 'quantidadeRetrabalhada',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.QuantidadeAprovada',
        field: 'quantidadeAprovada',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoSaidaGrid.QuantidadeReprovada',
        field: 'quantidadeReprovada',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
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
    }];
  }

  private getGridData(input: VsGridGetInput): Observable<VsGridGetResult> {
    if (!this.ordemDto) {
      return of(new VsGridGetResult(null, 0));
    }

    return this.inspecaoSaidaService
      .getInspecoesSaida(input, this.ordemDto.odf)
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
          odf: dto.odf,
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
        this.inspecaoSaidaService.refreshOrdemGrid.next(undefined);
      });
    }
  }

  private excluirInspecao(codigoInspecao: number): void {
    this.inspecaoSaidaService.excluirInspecaoSaida(codigoInspecao).subscribe(() => {
      this.gridOptions.refresh(true);
      this.inspecaoSaidaService.refreshOrdemGrid.next(undefined);
    }, (err: HttpErrorResponse) => {
      this.messageService.error(getErrorMessage(err));
    });
  }
}
