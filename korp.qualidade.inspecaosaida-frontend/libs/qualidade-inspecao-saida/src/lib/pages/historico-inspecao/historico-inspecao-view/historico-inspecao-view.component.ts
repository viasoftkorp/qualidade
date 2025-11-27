import { AfterViewInit, Component, TemplateRef, ViewChild } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';
import { Observable, of } from 'rxjs';
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
  GetAllHistoricoInspecaoSaidaItensOutput,
  GetAllHistoricoInspecaoSaidaOutput,
  HistoricoInspecaoSaidaFilters,
  HistoricoInspecaoSaidaItensOutput,
  HistoricoInspecaoSaidaOutput,
  OrdemProducaoFilters,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO
} from '../../../tokens';
import { HistoricoInspecaoService } from '../historico-inspecao.service';
import { HistoricoInspecaoViewService } from './historico-inspecao-view.service';
import { HistoricoInspecaoViewFilterComponent } from './historico-inspecao-view-filter/historico-inspecao-view-filter.component';
import { HistoricoInspecaoDetailsModalComponent } from '../historico-inspecao-details-modal/historico-inspecao-details-modal.component';
import { Router } from '@angular/router';
import { QualidadeInspecaoSaidaService } from "../../../services/qualidade-inspecao-saida.service";
import { RncEditorModalService } from '@viasoft/rnc-lib';

@Component({
  selector: 'qa-historico-inspecao-view',
  templateUrl: './historico-inspecao-view.component.html',
  styleUrls: ['./historico-inspecao-view.component.scss'],
  providers: [DecimalPipe]
})
export class HistoricoInspecaoViewComponent implements AfterViewInit {
  @ViewChild('actions') private actionsTemplate: TemplateRef<any>;

  public gridOptionsHeader: VsGridOptions;
  public gridOptionsItens: VsGridOptions;
  public utilizarReservaPedido = false;

  private odfSelecionada = null;
  private readonly HISTORICO_FILTRO_KEY = 'HistoricoInspecaoSaidaFiltros';
  private subs: VsSubscriptionManager = new VsSubscriptionManager();
  private filtros: HistoricoInspecaoSaidaFilters = {};

  public get possuiFiltros(): boolean {
    return Boolean(this.filtros) && (
      Boolean(this.filtros.ordemFabricacao)
      || Boolean(this.filtros.codigoProduto)
      || Boolean(this.filtros.lote));
  }

  constructor(private historicoInspecaoService: HistoricoInspecaoService,
    private service: HistoricoInspecaoViewService, private decimalPipe: DecimalPipe,
    private vsDialog: VsDialog, private storageService: VsStorageService,
    private translateService: TranslateService,
    private router: Router, private rncEditorModalService: RncEditorModalService,
    private inspecaoSaidaService: QualidadeInspecaoSaidaService,) {
    const filtrosStr = this.storageService.get(this.HISTORICO_FILTRO_KEY);
    this.filtros = filtrosStr ? JSON.parse(filtrosStr) : {};

    this.inspecaoSaidaService.getParametroBool(
      UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO, UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO
    ).then((param) => {
      this.utilizarReservaPedido = param
      this.iniciarGridHeader();
      this.iniciarGridItens();
    });
  }

  ngAfterViewInit(): void {
    this.historicoInspecaoService.actionsTemplate.next(this.actionsTemplate);
  }

  public limparFiltros(): void {
    this.filtros = {};
    this.storageService.set(this.HISTORICO_FILTRO_KEY, JSON.stringify(this.filtros));
    this.gridOptionsHeader.refresh();
  }

  public abrirFiltros(): void {
    this.subs.add('open-filter-modal', this.vsDialog.open(
      HistoricoInspecaoViewFilterComponent,
      this.filtros,
      { maxWidth: '30vw' }
    ).afterClosed().subscribe((filtros: HistoricoInspecaoSaidaFilters) => {
      if (!filtros) {
        return;
      }

      this.filtros = filtros;
      this.storageService.set(this.HISTORICO_FILTRO_KEY, JSON.stringify(filtros));
      this.gridOptionsHeader.refresh();
    }));
  }

  private iniciarGridHeader(): void {
    this.gridOptionsHeader = new VsGridOptions();
    this.gridOptionsHeader.id = '3f059d34-b636-45e9-a87a-b5feec39b607';
    this.gridOptionsHeader.enableSorting = false;
    this.gridOptionsHeader.enableQuickFilter = false;
    this.gridOptionsHeader.enableFilter = false;
    this.gridOptionsHeader.sizeColumnsToFit = false;

    this.gridOptionsHeader.columns = [];

    if (this.utilizarReservaPedido) {
      this.gridOptionsHeader.columns = [
        new VsGridSimpleColumn({
          headerName: 'HistoricoInspecao.Cliente',
          field: 'cliente',
          width: 100,
        }),
        new VsGridSimpleColumn({
          headerName: 'HistoricoInspecao.NumeroPedido',
          field: 'numeroPedido',
          width: 110,
        }),
      ];
    }

    this.gridOptionsHeader.columns = [
      ...this.gridOptionsHeader.columns,
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Plano',
        field: 'plano',
        width: 110
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.DescricaoPlano',
        field: 'descricaoPlano',
        width: 180
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.Odf',
        field: 'odfApontada',
        width: 80
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.CodigoProduto',
        field: 'codigoProduto',
        width: 110
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.DescricaoProduto',
        field: 'descricaoProduto',
        width: 190
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Lote',
        field: 'lote',
        width: 80
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Revisao',
        field: 'revisao',
        width: 100,
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.QuantidadeLote',
        field: 'quantidadeLote',
        width: 120,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.QuantidadeInspecao',
        field: 'quantidadeInspecao',
        width: 120,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      })
    ];

    this.subs.add('refresh-grid', this.service.refreshGrid
      .subscribe((filtros: OrdemProducaoFilters) => {
        this.filtros = filtros;
        this.gridOptionsHeader.refresh();
      }));

    this.gridOptionsHeader.get = (input: VsGridGetInput) => this.getGridDataHeader(input);
    this.gridOptionsHeader.select = (rowIndex: number, data: HistoricoInspecaoSaidaOutput) => {
      this.odfSelecionada = data.ordemFabricacao;
      this.gridOptionsItens.refresh();
    };
  }

  private iniciarGridItens(): void {
    this.gridOptionsItens = new VsGridOptions();
    this.gridOptionsItens.id = '4f059d34-b636-45e9-a87a-b5feec39b604';
    this.gridOptionsItens.enableSorting = false;
    this.gridOptionsItens.enableQuickFilter = false;
    this.gridOptionsItens.enableFilter = false;
    this.gridOptionsItens.sizeColumnsToFit = false;

    this.gridOptionsItens.columns = [
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.TipoInspecao',
        field: 'tipoInspecao',
        width: 130
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Odf',
        field: 'odfApontada',
        width: 90,
      }),
      new VsGridDateColumn({
        headerName: 'HistoricoInspecao.DataInspecao',
        field: 'dataInspecao'
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Inspetor',
        field: 'inspetor'
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Resultado',
        field: 'resultado'
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.QuantidadeTotal',
        field: 'quantidadeInspecao',
        width: 150,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade)
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.OdfRetrabalho',
        field: 'odfRetrabalho',
        width: 90,
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.CodigoRnc',
        field: 'codigoRnc',
        width: 90,
      }),
      // new VsGridSimpleColumn({
      //   headerName: 'HistoricoInspecao.Produto',
      //   field: 'descricaoProduto',
      //   width: 500
      // }),
    ];

    this.gridOptionsItens.get = (input: VsGridGetInput) => this.getGridDataItens(input);
    this.gridOptionsItens.edit = (index: number, itemHistorico: HistoricoInspecaoSaidaItensOutput) => {
      this.vsDialog.open(HistoricoInspecaoDetailsModalComponent, { codigoInspecao: itemHistorico.codigoInspecao, transferencias: itemHistorico.transferencias });
    };
    this.gridOptionsItens.select = (index: number, itemHistorico: HistoricoInspecaoSaidaItensOutput) => {
      this.vsDialog.open(HistoricoInspecaoDetailsModalComponent, { codigoInspecao: itemHistorico.codigoInspecao, transferencias: itemHistorico.transferencias });
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
    ];
  }

  private getGridDataHeader(input: VsGridGetInput): Observable<VsGridGetResult> {
    return this.service.get(input, this.filtros)
      .pipe(
        map((pagedHistorico: GetAllHistoricoInspecaoSaidaOutput) => {
          return new VsGridGetResult(pagedHistorico.items, pagedHistorico.totalCount);
        })
      );
  }

  private getGridDataItens(input: VsGridGetInput): Observable<VsGridGetResult> {
    if (!this.odfSelecionada) {
      return of(new VsGridGetResult([], 0));
    }
    return this.service.getItens(input, this.odfSelecionada)
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
}
