import {
  Component, Input,
  OnDestroy,
  OnInit,
  TemplateRef,
  ViewChild
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
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
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { QualidadeInspecaoEntradaService } from '../../../services/qualidade-inspecao-entrada.service';
import {
  GetNotasFiscaisDTO, HistoricoInspecaoEntradaFilters,
  InspecaoDetailsDTO,
  NotaFiscalDTO,
  NotaFiscalFilters
} from '../../../tokens';
import { InspecaoDetailsComponent } from '../inspecao-details/inspecao-details.component';
import { NotaFiscalViewFilterComponent } from './nota-fiscal-view-filter/nota-fiscal-view-filter.component';
import {
  HistoricoInspecaoViewFilterComponent
} from "../../historico/inspecao-historico-view/historico-inspecao-view-filter/historico-inspecao-view-filter.component";
import {HistoricoInspecaoViewService} from "../../historico/inspecao-historico-view/historico-inspecao-view.service";

@Component({
  selector: 'qa-nota-fiscal-view',
  templateUrl: './nota-fiscal-view.component.html',
  styleUrls: ['./nota-fiscal-view.component.scss'],
})
export class NotaFiscalViewComponent implements OnInit, OnDestroy {
  @Input() private tab: string;
  @ViewChild('actions') private actionsTemplate: TemplateRef<any>;
  private filtros: NotaFiscalFilters = {};
  private filtrosHistorico: HistoricoInspecaoEntradaFilters = {};
  private subs = new VsSubscriptionManager();
  public gridOptions = new VsGridOptions();
  public pesquisa: string;

  private readonly NOTA_FISCAL_FILTRO_KEY = 'NotaFiscalInspecaoEntradaFiltros';

  public get possuiFiltros(): boolean {
    return Boolean(this.filtros) && (
      Boolean(this.filtros.lote)
      || Boolean(this.filtros.notaFiscal)
      || Boolean(this.filtros.codigoProduto)
      || Boolean(this.filtros.dataEntrada));
  }

  constructor(
    private vsDialog: VsDialog,
    private matDialog: MatDialog,
    private inspecaoEntradaService: QualidadeInspecaoEntradaService,
    private storageService: VsStorageService,
    private historicoInspecaoViewService: HistoricoInspecaoViewService
  ) {
  }

  ngOnInit(): void {
    this.initGrid();

    this.subs.add('pesquisa-alterada', this.inspecaoEntradaService
      .pesquisaAlterada
      .subscribe((pesquisa: string) => {
        this.pesquisa = pesquisa;
        this.gridOptions.refresh();
      }));

    this.subs.add('refresh-nota-grid', this.inspecaoEntradaService
      .refreshNotaFiscalGrid
      .subscribe(() => {
        this.gridOptions.refresh();
      }));
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  ngAfterViewInit(): void {
    this.inspecaoEntradaService.actionsTemplate.next(this.actionsTemplate);
  }

  public limparFiltros(): void {
    this.filtros = {};
    this.storageService.set(this.NOTA_FISCAL_FILTRO_KEY, JSON.stringify(this.filtros));
    this.gridOptions.refresh();
  }

  public abrirFiltros(): void {
    if (this.tab == 'Historico') {
      this.subs.add('open-filter-modal', this.vsDialog.open(
        NotaFiscalViewFilterComponent,
        this.filtros,
        {maxWidth: '30vw'}
      ).afterClosed().subscribe((filtros: HistoricoInspecaoEntradaFilters) => {
        if (!filtros) {
          return;
        }

        this.filtrosHistorico = filtros;
        this.historicoInspecaoViewService.refreshGrid.next(this.filtrosHistorico);
      }));
    } else {
      this.subs.add('open-filter-modal', this.vsDialog.open(
        HistoricoInspecaoViewFilterComponent,
        this.filtros,
        { maxWidth: '30vw' }
      ).afterClosed().subscribe((filtros: NotaFiscalFilters) => {
        if (!filtros) {
          return;
        }

        this.filtros = filtros;
        this.storageService.set(this.NOTA_FISCAL_FILTRO_KEY, JSON.stringify(filtros));
        this.gridOptions.refresh();
      }));
    }
  }

  private initGrid(): void {
    this.gridOptions.id = '0a39beb1-bfbf-4bda-afcd-2764a3925f37';
    this.gridOptions.enableFilter = false;
    this.gridOptions.enableQuickFilter = false;
    this.gridOptions.enableSorting = false;
    this.gridOptions.sizeColumnsToFit = false;

    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.Plano',
        field: 'plano',
        width: 110
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.DescricaoPlano',
        field: 'descricaoPlano',
        width: 180
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscal',
        field: 'notaFiscal',
        width: 80,
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.Lote',
        field: 'lote',
        width: 100,
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.CodigoProduto',
        field: 'codigoProduto',
        width: 100,
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.DescricaoProduto',
        field: 'descricaoProduto',
        width: 300,
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.DescricaoForneced',
        field: 'descricaoForneced',
        width: 250,
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.Quantidade',
        field: 'quantidade',
        width: 110,
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.QuantidadeInspecionada',
        field: 'quantidadeInspecionada',
        width: 120,
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.QuantidadeInspecionar',
        field: 'quantidadeInspecionar',
        width: 110,
      }),
      new VsGridDateColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscalGrid.DataEntrada',
        field: 'dataEntrada',
        width: 110,
      }),
    ];

    this.gridOptions.get = (i: VsGridGetInput) => this.getGridData(i);
    this.gridOptions.select = (rowIndex: number, data: NotaFiscalDTO) => this.notaSelecionada(data);
    this.gridOptions.actions = [{
      icon: 'plus',
      tooltip: 'QualidadeInspecaoEntrada.InspecaoDetails.NovaInspecao',
      callback: (rowIndex: number, data: NotaFiscalDTO) => {
        const dialogOptions = this.vsDialog.generateDialogConfig(
          {
            notaFiscal: data,
            novaInspecao: true,
            codigoProduto: data.codigoProduto
          } as InspecaoDetailsDTO,
          {
            hasBackdrop: true
          }
        );
        const openedDialog = this.matDialog.open(InspecaoDetailsComponent, dialogOptions);
        return openedDialog.afterClosed();
      }
    }];
  }

  private getGridData(input: VsGridGetInput): Observable<VsGridGetResult> {
    input.filter = this.pesquisa;

    return this.inspecaoEntradaService
      .getNotasFiscais(input, this.filtros)
      .pipe(
        map((r: GetNotasFiscaisDTO) => new VsGridGetResult(r.items, r.totalCount)),
      );
  }

  private notaSelecionada(nota: NotaFiscalDTO): void {
    this.inspecaoEntradaService.notaFiscalSelecionada.next(nota);
  }
}
