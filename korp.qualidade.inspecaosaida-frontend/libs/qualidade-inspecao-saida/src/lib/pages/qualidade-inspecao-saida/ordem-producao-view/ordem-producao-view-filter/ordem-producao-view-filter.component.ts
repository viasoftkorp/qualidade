import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { OrdemProducaoFilters } from '../../../../tokens';

@Component({
  selector: 'qa-ordem-producao-view-filter',
  templateUrl: './ordem-producao-view-filter.component.html',
  styleUrls: ['./ordem-producao-view-filter.component.scss']
})
export class OrdemProducaoViewFilterComponent {
  public form: FormGroup;

  constructor(private formBuilder: FormBuilder, @Inject(MAT_DIALOG_DATA) private filtros: OrdemProducaoFilters,
              private dialogRef: MatDialogRef<OrdemProducaoViewFilterComponent>) {
    this.iniciarForm();
  }

  public filtrar(): void {
    this.filtros = this.form.getRawValue();
    this.dialogRef.close(this.filtros);
  }

  private iniciarForm(): void {
    this.form = this.formBuilder.group({
      lote: [this.filtros.lote],
      odf: [this.filtros.odf],
      codigoProduto: [this.filtros.codigoProduto ? { value: this.filtros.codigoProduto, key: '' } : null],
      dataInicio: [this.filtros.dataInicio],
      dataEntrega: [this.filtros.dataEntrega],
      dataEmissao: [this.filtros.dataEmissao],
    });
  }
}
