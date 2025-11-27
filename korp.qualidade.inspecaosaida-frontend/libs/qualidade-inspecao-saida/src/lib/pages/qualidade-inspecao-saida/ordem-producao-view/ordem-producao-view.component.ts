import {
  AfterViewInit, Component, Input, OnDestroy, OnInit, TemplateRef, ViewChild
} from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { VsStorageService, VsSubscriptionManager } from '@viasoft/common';
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
  GetOrdensProducaoDTO, HistoricoInspecaoSaidaFilters,
  InspecaoDetailsDTO,
  OrdemProducaoDTO,
  OrdemProducaoFilters, UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO, UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO,
} from '../../../tokens';
import { QualidadeInspecaoSaidaService } from '../../../services/qualidade-inspecao-saida.service';
import { InspecaoDetailsComponent } from '../inspecao-details/inspecao-details.component';
import { OrdemProducaoViewFilterComponent } from './ordem-producao-view-filter/ordem-producao-view-filter.component';
import {
  HistoricoInspecaoViewFilterComponent
} from '../../historico-inspecao/historico-inspecao-view/historico-inspecao-view-filter/historico-inspecao-view-filter.component';
import {
  HistoricoInspecaoViewService
} from '../../historico-inspecao/historico-inspecao-view/historico-inspecao-view.service';

@Component({
  selector: 'qa-ordem-producao-view',
  templateUrl: './ordem-producao-view.component.html',
  styleUrls: ['./ordem-producao-view.component.scss'],
  providers: [DecimalPipe]
})
export class OrdemProducaoViewComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() private tab: string;
  @ViewChild('actions') private actionsTemplate: TemplateRef<any>;

  public gridOptions: VsGridOptions;
  public utilizarReservaPedido = false;

  private readonly ORDEM_PRODUCAO_FILTRO_KEY = 'OrdemProducaoInspecaoSaidaFiltros';

  private subs = new VsSubscriptionManager();
  private filtros: OrdemProducaoFilters = {};

  public get possuiFiltros(): boolean {
    return Boolean(this.filtros) && (
      Boolean(this.filtros.lote)
      || Boolean(this.filtros.odf)
      || Boolean(this.filtros.codigoProduto)
      || Boolean(this.filtros.dataInicio)
      || Boolean(this.filtros.dataEntrega)
      || Boolean(this.filtros.dataEmissao));
  }

  constructor(
    private vsDialog: VsDialog,
    private matDialog: MatDialog,
    private inspecaoSaidaService: QualidadeInspecaoSaidaService,
    private router: Router,
    private storageService: VsStorageService,
    private decimalPipe: DecimalPipe,
    private historicoInspecaoViewService: HistoricoInspecaoViewService
  ) {
  }

  async ngOnInit(): Promise<void> {

    this.subs.add('refresh-ordem-grid', this.inspecaoSaidaService
      .refreshOrdemGrid
      .subscribe(() => {
        this.gridOptions?.refresh();
      }));

    this.utilizarReservaPedido = await this.inspecaoSaidaService.getParametroBool(
      UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO, UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO
    );

    this.initGrid();
  }

  ngAfterViewInit(): void {
    this.inspecaoSaidaService.actionsTemplate.next(this.actionsTemplate);
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  public limparFiltros(): void {
    this.filtros = {};
    this.storageService.set(this.ORDEM_PRODUCAO_FILTRO_KEY, JSON.stringify(this.filtros));
    this.gridOptions.refresh();
  }

  public abrirFiltros(): void {
    if (this.tab == 'Historico') {
      this.subs.add('open-filter-modal', this.vsDialog.open(
        HistoricoInspecaoViewFilterComponent,
        this.filtros,
        { maxWidth: '30vw' }
      ).afterClosed().subscribe((filtros: HistoricoInspecaoSaidaFilters) => {
        if (!filtros) {
          return;
        }

        this.filtros = filtros;
        this.historicoInspecaoViewService.refreshGrid.next(this.filtros);
      }));
    } else {
      this.subs.add('open-filter-modal', this.vsDialog.open(
        OrdemProducaoViewFilterComponent,
        this.filtros,
        { maxWidth: '30vw' }
      ).afterClosed().subscribe((filtros: OrdemProducaoFilters) => {
        if (!filtros) {
          return;
        }

        this.filtros = filtros;
        this.storageService.set(this.ORDEM_PRODUCAO_FILTRO_KEY, JSON.stringify(filtros));
        this.gridOptions.refresh();
      }));
    }
  }

  private initGrid(): void {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = '0a39beb1-bfbf-4bda-afcd-2764a3925f37';
    this.gridOptions.enableFilter = false;
    this.gridOptions.enableQuickFilter = false;
    this.gridOptions.enableSorting = false;

    this.gridOptions.columns = [];

    if (this.utilizarReservaPedido) {
      this.gridOptions.columns = [
        new VsGridSimpleColumn({
          headerName: 'QualidadeInspecaoSaida.Cliente',
          field: 'cliente',
          width: 90,
        }),
        new VsGridSimpleColumn({
          headerName: 'QualidadeInspecaoSaida.NumeroPedido',
          field: 'numeroPedido',
          width: 110,
        }),
      ];
    }

    this.gridOptions.columns = [
      ...this.gridOptions.columns,
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.Plano',
        field: 'plano',
        width: 110
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.DescricaoPlano',
        field: 'descricaoPlano',
        width: 180
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.Odf',
        field: 'odfApontada',
        width: 80
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.CodigoProduto',
        field: 'codigoProduto',
        width: 100
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.DescricaoProduto',
        field: 'descricaoProduto',
        width: 200
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.Lote',
        field: 'lote',
        width: 50
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
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.QuantidadeOrdem',
        field: 'quantidadeOrdem',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.QuantidadeProduzida',
        field: 'quantidadeProduzida',
        width: 100,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.QuantidadeInspecionada',
        field: 'quantidadeInspecionada',
        width: 120,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.QuantidadeInspecionar',
        field: 'quantidadeInspecionar',
        width: 110,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridDateColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.DataInicio',
        field: 'dataInicio',
        width: 100
      }),
      new VsGridDateColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.DataEntrega',
        field: 'dataEntrega',
        width: 100
      }),
      new VsGridDateColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.DataEmissao',
        field: 'dataEmissao',
        width: 100
      }),
      new VsGridDateColumn({
        headerName: 'QualidadeInspecaoSaida.OrdemProducaoGrid.DataNegociada',
        field: 'dataNegociada',
        width: 110
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
        return openedDialog.afterClosed().toPromise().then(() => this.inspecaoSaidaService.ordemSelecionada.next(data));
      }
    }];
  }

  private getGridData(input: VsGridGetInput): Observable<VsGridGetResult> {
    return this.inspecaoSaidaService
      .getOrdensInspecao(input, this.filtros)
      .pipe(
        map((r: GetOrdensProducaoDTO) => new VsGridGetResult(r.items, r.totalCount))
      );
  }

  private ordemSelecionada(odf: OrdemProducaoDTO): void {
    this.inspecaoSaidaService.ordemSelecionada.next(odf);
  }
}
