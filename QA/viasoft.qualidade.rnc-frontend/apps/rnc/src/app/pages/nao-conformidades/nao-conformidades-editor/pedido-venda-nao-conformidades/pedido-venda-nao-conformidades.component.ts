import { Component, OnDestroy, OnInit } from '@angular/core';
import { ControlContainer, FormGroup } from '@angular/forms';
import { VsSubscriptionManager } from '@viasoft/common';
import { NaoConformidadesFormControl } from '../nao-conformidades-form-control';

@Component({
  selector: 'rnc-pedido-venda-nao-conformidades',
  templateUrl: './pedido-venda-nao-conformidades.component.html',
  styleUrls: ['./pedido-venda-nao-conformidades.component.scss'],
})
export class PedidoVendaNaoConformidadesComponent implements OnInit, OnDestroy {
  private subscriptionManager = new VsSubscriptionManager();
  public naoConformidadeForm: FormGroup;
  public formControls = NaoConformidadesFormControl;

  constructor(
    private controlContainer: ControlContainer
  ) {}

  public ngOnInit(): void {
    this.naoConformidadeForm = this.controlContainer.control as FormGroup;
  }

  public ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }
}
