import {
  AfterViewInit,
  Component,
  TemplateRef,
  ViewChild
} from '@angular/core';
import { DecimalPipe } from '@angular/common';

import { VsStorageService, VsSubscriptionManager } from '@viasoft/common';
import {
  VsDialog,
  VsGridOptions,
} from '@viasoft/components';
import { MatDialogRef } from '@angular/material/dialog';
import { ProcessamentoInspecaoViewService } from './processamento-inspecao-view.service';
import {
  ProcessamentoInspecaoViewFilterComponent
} from './processamento-inspecao-view-filter/processamento-inspecao-view-filter.component';
import { ProcessamentoInspecaoService } from '../processamento.service';
import { ProcessamentoInspecaoEntradaFilters } from '../../../tokens';

@Component({
  selector: 'qa-processamento-inspecao-view',
  templateUrl: './processamento-inspecao-view.component.html',
  styleUrls: ['./processamento-inspecao-view.component.scss'],
  providers: [ProcessamentoInspecaoViewService, DecimalPipe]
})
export class ProcessamentoInspecaoViewComponent implements AfterViewInit {
  @ViewChild('actions') private actionsTemplate: TemplateRef<any>;

  private readonly PROCESSAMENTO_FILTRO_KEY = 'ProcessamentoInspecaoEntradaFiltros';
  private subs: VsSubscriptionManager = new VsSubscriptionManager();
  private filterDialogOpened: MatDialogRef<any, any>;

  public filtros: ProcessamentoInspecaoEntradaFilters = {};
  public gridOptions: VsGridOptions = new VsGridOptions();

  public get possuiFiltros(): boolean {
    return Boolean(this.filtros) && (
      Boolean(this.filtros.status)
      || Boolean(this.filtros.numeroExecucoes)
      || Boolean(this.filtros.erro)
      || Boolean(this.filtros.resultado)
      || Boolean(this.filtros.quantidadeTotal)
      || Boolean(this.filtros.codigoProduto)
      || Boolean(this.filtros.notaFiscal)
      || Boolean(this.filtros.idUsuarioExecucao)
      || Boolean(this.filtros.dataExecucao));
  }

  constructor(private processamentoInspecaoService: ProcessamentoInspecaoService,
              private service: ProcessamentoInspecaoViewService,
              private vsDialog: VsDialog,
              private storageService: VsStorageService) {
    const filtrosStr = this.storageService.get(this.PROCESSAMENTO_FILTRO_KEY);
    this.filtros = filtrosStr ? JSON.parse(filtrosStr) : {};
  }

  ngAfterViewInit(): void {
    this.processamentoInspecaoService.actionsTemplate.next(this.actionsTemplate);
  }

  public limparFiltros(): void {
    this.filtros = {};
    this.storageService.set(this.PROCESSAMENTO_FILTRO_KEY, JSON.stringify(this.filtros));

    if (this.filterDialogOpened) {
      this.filterDialogOpened.close();
    }
  }

  public abrirFiltros(): void {
    this.filterDialogOpened = this.vsDialog.open(
      ProcessamentoInspecaoViewFilterComponent,
      this.filtros,
      { maxWidth: '30vw' }
    );

    this.subs.add('open-filter-modal', this.filterDialogOpened.afterClosed()
      .subscribe((filtros: ProcessamentoInspecaoEntradaFilters) => {
        if (!filtros) {
          return;
        }

        this.filtros = filtros;
        this.storageService.set(this.PROCESSAMENTO_FILTRO_KEY, JSON.stringify(filtros));
      }));
  }
}
