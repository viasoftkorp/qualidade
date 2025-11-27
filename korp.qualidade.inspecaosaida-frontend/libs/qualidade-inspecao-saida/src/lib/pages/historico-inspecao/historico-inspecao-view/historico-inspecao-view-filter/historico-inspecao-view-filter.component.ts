import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { VsSelectOption } from '@viasoft/components';

import { HistoricoInspecaoSaidaFilters } from '../../../../tokens';

@Component({
  selector: 'qa-historico-inspecao-view-filter',
  templateUrl: './historico-inspecao-view-filter.component.html',
  styleUrls: ['./historico-inspecao-view-filter.component.scss']
})
export class HistoricoInspecaoViewFilterComponent {
  public form: FormGroup;
  public opcoesStatus: VsSelectOption[];

  constructor(private formBuilder: FormBuilder, @Inject(MAT_DIALOG_DATA) private filtros: HistoricoInspecaoSaidaFilters,
              private dialogRef: MatDialogRef<HistoricoInspecaoViewFilterComponent>) {
    this.iniciarForm();
  }

  public filtrar(): void {
    this.filtros = this.form.getRawValue();
    this.dialogRef.close(this.filtros);
  }

  private iniciarForm(): void {
    this.form = this.formBuilder.group({
      lote: [this.filtros.lote],
      ordemFabricacao: [this.filtros.ordemFabricacao],
      codigoProduto: [this.filtros.codigoProduto ? { value: this.filtros.codigoProduto, key: '' } : null],
    });
  }
}
