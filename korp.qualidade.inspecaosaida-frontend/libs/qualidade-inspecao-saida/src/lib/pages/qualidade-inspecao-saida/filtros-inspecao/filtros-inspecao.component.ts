import { Component, OnDestroy } from '@angular/core';
import { VsDialog } from '@viasoft/components';
import { VsSubscriptionManager } from '@viasoft/common';
import { FiltrosInspecaoModalComponent } from './filtros-inspecao-modal/filtros-inspecao-modal.component';
import { FiltrosInspecaoDto } from '../../../tokens';
import { QualidadeInspecaoSaidaService } from "../../../services/qualidade-inspecao-saida.service";

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
    private readonly QualidadeInspecaoSaidaService: QualidadeInspecaoSaidaService
  ) {
    this.init();
  }

  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public abrirModalFiltros(): void {
    this.subscriptionManager.add('filtros-inspecao-modal', this.vsDialog
      .open(FiltrosInspecaoModalComponent, this.QualidadeInspecaoSaidaService.getFiltros())
      .afterClosed()
      .subscribe((filtros: FiltrosInspecaoDto) => {
        if (!filtros) {
          return;
        }

        this.aplicarFiltros(filtros);
        this.QualidadeInspecaoSaidaService.ordemSelecionada.next(null);
        this.QualidadeInspecaoSaidaService.ordemHistoricoSelecionada.next(null);
        this.QualidadeInspecaoSaidaService.refreshOrdemGrid.next();
        this.QualidadeInspecaoSaidaService.refreshHistoricoGrid.next();
        this.QualidadeInspecaoSaidaService.refreshInspecoesOrdemGrid.next();
        this.QualidadeInspecaoSaidaService.refreshInspecoesHistoricoGrid.next();
      }));
  }

  private init(): void {
    const filtros = this.QualidadeInspecaoSaidaService.getFiltros();
    this.filtrando = this.validarFiltrando(filtros);
  }

  private aplicarFiltros(filtros: FiltrosInspecaoDto): void {
    this.QualidadeInspecaoSaidaService.setFiltros(filtros);
    this.filtrando = this.validarFiltrando(filtros);
  }

  private validarFiltrando(filtros: FiltrosInspecaoDto): boolean {
    return filtros.observacoesMetricas.length > 0;
  }
}
