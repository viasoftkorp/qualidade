import { DecimalPipe } from '@angular/common';
import {
  Component,
  TemplateRef,
  ViewChild
} from '@angular/core';
import { Router } from '@angular/router';
import {
  VsStorageService,
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
import { HistoricoService } from '../../../services/historico.service';
import {
  GetAllHistoricoInspecaoEntradaItensOutput,
  GetAllHistoricoInspecaoEntradaOutput,
  HistoricoInspecaoEntradaFilters,
  HistoricoInspecaoEntradaItensOutput,
  InspecoesHistoricoInput,
  NotaFiscalDTO
} from '../../../tokens';
import { HistoricoInspecaoDetailsModalComponent } from '../historico-inspecao-details-modal/historico-inspecao-details-modal.component';
import {
  HistoricoInspecaoViewFilterComponent
} from './historico-inspecao-view-filter/historico-inspecao-view-filter.component';
import { HistoricoInspecaoViewService } from './historico-inspecao-view.service';
import { RncEditorModalService } from '@viasoft/rnc-lib';

@Component({
  selector: 'qa-inspecao-historico-view',
  templateUrl: './inspecao-historico-view.component.html',
  styleUrls: ['./inspecao-historico-view.component.scss'],
})
export class InspecaoHistoricoViewComponent {
  @ViewChild('actions') private actionsTemplate: TemplateRef<any>;

  private notaSelecionada: InspecoesHistoricoInput;
  private readonly HISTORICO_FILTRO_KEY = 'HistoricoInspecaoEntradaFiltros'
  private subs: VsSubscriptionManager = new VsSubscriptionManager();
  private filtros: HistoricoInspecaoEntradaFilters = {};

  public gridOptionsHeader: VsGridOptions = new VsGridOptions();
  public gridOptionsItens: VsGridOptions = new VsGridOptions();

  public get possuiFiltros(): boolean {
    return Boolean(this.filtros) && (
      Boolean(this.filtros.notaFiscal)
      || Boolean(this.filtros.codigoProduto)
      || Boolean(this.filtros.lote));
  }

  constructor(private historicoInspecaoService: HistoricoService,
    private service: HistoricoInspecaoViewService, private decimalPipe: DecimalPipe,
    private vsDialog: VsDialog, private storageService: VsStorageService, private rncEditorModalService: RncEditorModalService,
    private router: Router) {
    const filtrosStr = this.storageService.get(this.HISTORICO_FILTRO_KEY);
    this.filtros = filtrosStr ? JSON.parse(filtrosStr) : {};
    this.iniciarGridHeader();
    this.iniciarGridItens();
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
    ).afterClosed().subscribe((filtros: HistoricoInspecaoEntradaFilters) => {
      if (!filtros) {
        return;
      }

      this.filtros = filtros;
      this.storageService.set(this.HISTORICO_FILTRO_KEY, JSON.stringify(filtros));
      this.gridOptionsHeader.refresh();
    }));
  }

  private iniciarGridHeader(): void {
    this.gridOptionsHeader.id = '3f059d34-b636-45e9-a87a-b5feec39b607';
    this.gridOptionsHeader.enableSorting = false;
    this.gridOptionsHeader.enableQuickFilter = false;
    this.gridOptionsHeader.enableFilter = false;
    this.gridOptionsHeader.sizeColumnsToFit = false;

    this.gridOptionsHeader.columns = [
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
        headerName: 'HistoricoInspecao.NotaFiscal',
        field: 'notaFiscal',
        width: 80
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Lote',
        field: 'lote',
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
        headerName: 'HistoricoInspecao.DescricaoFornecedor',
        field: 'descricaoForneced',
        width: 220,
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.QuantidadeLote',
        field: 'quantidade',
        width: 120
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.QuantidadeInspecao',
        field: 'quantidadeInspecionada',
        width: 120
      })
    ];

    this.subs.add('refresh-grid', this.service.refreshGrid
      .subscribe((filtros: HistoricoInspecaoEntradaFilters) => {
        this.filtros = filtros;
        this.gridOptionsHeader.refresh();
      }));

    this.gridOptionsHeader.get = (input: VsGridGetInput) => this.getGridDataHeader(input);
    this.gridOptionsHeader.select = (rowIndex: number, data: NotaFiscalDTO) => {
      this.notaSelecionada = {
        notaFiscal: data.notaFiscal,
        lote: data.lote,
      };
      this.gridOptionsItens.refresh();
    };
  }

  private iniciarGridItens(): void {
    this.gridOptionsItens.id = '4f059d34-b636-45e9-a87a-b5feec39b604';
    this.gridOptionsItens.enableSorting = false;
    this.gridOptionsItens.enableQuickFilter = false;
    this.gridOptionsItens.enableFilter = false;
    this.gridOptionsItens.sizeColumnsToFit = false;

    this.gridOptionsItens.columns = [
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.NotaFiscal',
        field: 'notaFiscal',
        width: 80,
      }),
      new VsGridDateColumn({
        headerName: 'HistoricoInspecao.DataInspecao',
        field: 'dataInspecao',
        width: 100,
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Inspetor',
        field: 'inspetor',
        width: 150,
      }),
      new VsGridSimpleColumn({
        headerName: 'HistoricoInspecao.Resultado',
        field: 'resultado',
        width: 150,
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
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.QuantidadeAprovada',
        field: 'quantidadeAprovada',
        width: 180,
      }),
      new VsGridNumberColumn({
        headerName: 'HistoricoInspecao.QuantidadeReprovada',
        field: 'quantidadeReprovada',
        width: 180,
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
    }];
  }

  private getGridDataHeader(input: VsGridGetInput): Observable<VsGridGetResult> {
    return this.service.get(input, this.filtros)
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
    return this.service.getItens(input, this.notaSelecionada)
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
}
