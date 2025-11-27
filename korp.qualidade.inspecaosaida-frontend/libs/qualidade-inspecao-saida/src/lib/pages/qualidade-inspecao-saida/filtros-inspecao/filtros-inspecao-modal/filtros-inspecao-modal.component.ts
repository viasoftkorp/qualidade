import {
  AfterViewInit, Component, Inject, OnDestroy, ViewChild
} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormControl, FormGroup } from '@angular/forms';
import { VsSubscriptionManager } from '@viasoft/common';
import { VsChipListComponent } from '@viasoft/components';
import { ENTER } from '@angular/cdk/keycodes';
import { FiltrosInspecaoDto } from '../../../../tokens';

@Component({
  selector: 'qa-filtros-inspecao-modal',
  templateUrl: './filtros-inspecao-modal.component.html',
  styleUrls: ['./filtros-inspecao-modal.component.scss']
})
export class FiltrosInspecaoModalComponent implements AfterViewInit, OnDestroy {
  @ViewChild(VsChipListComponent) private readonly chipListComponent: VsChipListComponent;

  public formulario: FormGroup;
  public observacoesMetricas = new Array<string>();
  private readonly subscriptionManager = new VsSubscriptionManager();

  constructor(
    private readonly dialogRef: MatDialogRef<FiltrosInspecaoModalComponent>,
    @Inject(MAT_DIALOG_DATA) private readonly data: FiltrosInspecaoDto
  ) {
    this.criarFormulario();
  }

  ngAfterViewInit(): void {
    this.chipListComponent.separatorKeysCodes = [ENTER];
  }

  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public aplicar(): void {
    let filtrosParaAplicar = this.formulario.getRawValue() as FiltrosInspecaoDto;
    this.dialogRef.close(filtrosParaAplicar);
  }

  public limpar(): void {
    this.observacoesMetricas = new Array<string>();
    this.formulario.get('observacoesMetricas').setValue(new Array<string>());
    this.formulario.markAsDirty();
  }

  public get podeAplicar(): boolean {
    return this.formulario.dirty;
  }

  public get podeLimpar(): boolean {
    const observacoesMetricas = this.formulario.get('observacoesMetricas').value as Array<string>;
    return observacoesMetricas.length > 0;
  }

  public observacoesMetricasAlteradas(): void {
    this.formulario.get('observacoesMetricas').setValue(this.observacoesMetricas);
    this.formulario.get('observacoesMetricas').markAsDirty();
  }

  private criarFormulario(): void {
    this.formulario = new FormGroup({
      observacoesMetricas: new FormControl([...this.data.observacoesMetricas])
    });

    this.observacoesMetricas = [...this.data.observacoesMetricas];
  }
}
