import { Component } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { MatDialogRef } from '@angular/material/dialog';

import { VsStorageService, VsSubscriptionManager } from '@viasoft/common';
import { VsDialog } from '@viasoft/components';

import { ProcessamentoInspecaoSaidaFilters } from '../../../tokens';
import { ProcessamentoInspecaoService } from '../processamento-inspecao.service';
import { ProcessamentoInspecaoViewService } from './processamento-inspecao-view.service';
import { ProcessamentoInspecaoViewFilterComponent } from './processamento-inspecao-view-filter/processamento-inspecao-view-filter.component';

@Component({
  selector: 'qa-processamento-inspecao-view',
  templateUrl: './processamento-inspecao-view.component.html',
  styleUrls: ['./processamento-inspecao-view.component.scss'],
  providers: [DecimalPipe]
})
export class ProcessamentoInspecaoViewComponent {
  public filtros: ProcessamentoInspecaoSaidaFilters = {};

  private readonly PROCESSAMENTO_FILTRO_KEY = 'ProcessamentoInspecaoSaidaFiltros';
  private subs: VsSubscriptionManager = new VsSubscriptionManager();
  private filterDialogOpened: MatDialogRef<any, any>;

  public get possuiFiltros(): boolean {
    return Boolean(this.filtros) && (
      Boolean(this.filtros.status)
      || Boolean(this.filtros.numeroExecucoes)
      || Boolean(this.filtros.erro)
      || Boolean(this.filtros.resultado)
      || Boolean(this.filtros.quantidadeTotal)
      || Boolean(this.filtros.codigoProduto)
      || Boolean(this.filtros.odf)
      || Boolean(this.filtros.idUsuarioExecucao)
      || Boolean(this.filtros.dataExecucao));
  }

  constructor(private processamentoInspecaoService: ProcessamentoInspecaoService,
              private service: ProcessamentoInspecaoViewService, private decimalPipe: DecimalPipe,
              private vsDialog: VsDialog, private storageService: VsStorageService) {
    const filtrosStr = this.storageService.get(this.PROCESSAMENTO_FILTRO_KEY);
    this.filtros = filtrosStr ? JSON.parse(filtrosStr) : {};
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
      .subscribe((filtros: ProcessamentoInspecaoSaidaFilters) => {
        if (!filtros) {
          return;
        }

        this.filtros = filtros;
        this.storageService.set(this.PROCESSAMENTO_FILTRO_KEY, JSON.stringify(filtros));
      }));
  }
}
