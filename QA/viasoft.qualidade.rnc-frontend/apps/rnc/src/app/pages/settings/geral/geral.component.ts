import { FormGroup } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { GeralService } from './geral.service';
import { GeralFormControls } from './geral-form-controls';

@Component({
  selector: 'rnc-geral',
  templateUrl: './geral.component.html',
  styleUrls: ['./geral.component.scss'],
  providers: [GeralService]
})
export class GeralComponent implements OnInit {
  public formControls = GeralFormControls;
  constructor(private geralService: GeralService) { }

  public get form():FormGroup {
    return this.geralService.form;
  }
  public get canSave():boolean {
    return this.geralService.canSave;
  }
  public get isLoading():boolean {
    return this.geralService.isLoading;
  }

  public ngOnInit(): void {
    this.geralService.onInit();
  }

  public abirAplicativoRelatorios(): void {
    const { frontendUrl } = this.geralService.configuracaoGeral;
    window.open(`${frontendUrl}/relatorios/reports`, '_blank');
  }

  public save():void {
    this.geralService.save();
  }
}
