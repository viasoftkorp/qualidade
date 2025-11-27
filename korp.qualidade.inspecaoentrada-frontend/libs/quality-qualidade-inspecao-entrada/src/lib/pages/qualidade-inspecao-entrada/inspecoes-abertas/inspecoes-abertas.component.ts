import { Component, OnDestroy, OnInit } from '@angular/core';
import { NotaFiscalDTO } from '../../../tokens';
import { VsSubscriptionManager } from '@viasoft/common';
import { QualidadeInspecaoEntradaService } from '../../../services/qualidade-inspecao-entrada.service';

@Component({
  selector: 'inspecao-entrada-inspecoes-abertas',
  templateUrl: './inspecoes-abertas.component.html',
  styleUrls: ['./inspecoes-abertas.component.css']
})
export class InspecoesAbertasComponent implements OnInit, OnDestroy {
  public idNotaFiscal: string;
  private subscriptionManager = new VsSubscriptionManager();
  constructor(private qualidadeInspecaoEntradaService: QualidadeInspecaoEntradaService) { }

  public ngOnInit(): void {
    this.subscriptionManager.add('nota-fiscal-selecionada-inspecoes-abertas', this.qualidadeInspecaoEntradaService
      .notaFiscalSelecionada
      .subscribe((notaFiscal: NotaFiscalDTO) => {
        this.idNotaFiscal = notaFiscal?.id;
      }));

  }
  public ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }
}
