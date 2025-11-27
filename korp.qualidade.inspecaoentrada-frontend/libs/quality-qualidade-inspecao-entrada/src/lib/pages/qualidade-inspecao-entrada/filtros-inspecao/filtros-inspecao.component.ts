import { Component, OnDestroy } from '@angular/core';
import { VsDialog } from '@viasoft/components';
import { VsSubscriptionManager } from '@viasoft/common';
import { QualidadeInspecaoEntradaService } from '../../../services/qualidade-inspecao-entrada.service';
import { FiltrosInspecaoModalComponent } from './filtros-inspecao-modal/filtros-inspecao-modal.component';
import { FiltrosInspecaoDto } from '../../../tokens';

@Component({
  selector: 'qa-filtros-inspecao',
  templateUrl: './filtros-inspecao.component.html',
  styleUrls: ['./filtros-inspecao.component.scss']
})
export class FiltrosInspecaoComponent implements OnDestroy {
  public filtrando = false;
  private readonly subscriptionManager = new VsSubscriptionManager();

  constructor(
    private readonly vsDialog: VsDialog,
    private readonly qualidadeInspecaoEntradaService: QualidadeInspecaoEntradaService
  ) {
    this.init();
  }

  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public abrirModalFiltros(): void {
    this.subscriptionManager.add('filtros-inspecao-modal', this.vsDialog
      .open(FiltrosInspecaoModalComponent, this.qualidadeInspecaoEntradaService.getFiltros())
      .afterClosed()
      .subscribe((filtros: FiltrosInspecaoDto) => {
        if (!filtros) {
          return;
        }

        this.aplicarFiltros(filtros);
        this.qualidadeInspecaoEntradaService.notaFiscalSelecionada.next(null);
        this.qualidadeInspecaoEntradaService.notaFiscalHistoricoSelecionada.next(null);
        this.qualidadeInspecaoEntradaService.refreshNotaFiscalGrid.next();
        this.qualidadeInspecaoEntradaService.refreshHistoricoGrid.next();
        this.qualidadeInspecaoEntradaService.refreshInspecoesNotaFiscalGrid.next();
        this.qualidadeInspecaoEntradaService.refreshInspecoesHistoricoGrid.next();
      }));
  }

  private init(): void {
    const filtros = this.qualidadeInspecaoEntradaService.getFiltros();
    this.filtrando = this.validarFiltrando(filtros);
  }

  private aplicarFiltros(filtros: FiltrosInspecaoDto): void {
    this.qualidadeInspecaoEntradaService.setFiltros(filtros);
    this.filtrando = this.validarFiltrando(filtros);
  }

  private validarFiltrando(filtros: FiltrosInspecaoDto): boolean {
    return filtros.observacoesMetricas.length > 0;
  }
}
