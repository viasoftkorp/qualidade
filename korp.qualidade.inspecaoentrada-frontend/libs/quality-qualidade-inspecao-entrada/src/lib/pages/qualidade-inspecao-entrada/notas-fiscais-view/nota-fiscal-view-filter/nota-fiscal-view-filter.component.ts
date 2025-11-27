import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NotaFiscalFilters } from 'libs/quality-qualidade-inspecao-entrada/src/lib/tokens';

@Component({
  selector: 'qa-nota-fiscal-view-filter',
  templateUrl: './nota-fiscal-view-filter.component.html',
  styleUrls: ['./nota-fiscal-view-filter.component.scss']
})
export class NotaFiscalViewFilterComponent {
  public form: FormGroup;

  constructor(private formBuilder: FormBuilder, @Inject(MAT_DIALOG_DATA) private filtros: NotaFiscalFilters,
              private dialogRef: MatDialogRef<NotaFiscalViewFilterComponent>) {
    this.iniciarForm();
  }

  public filtrar(): void {
    this.filtros = this.form.getRawValue();
    this.dialogRef.close(this.filtros);
  }

  private iniciarForm(): void {
    this.form = this.formBuilder.group({
      lote: [this.filtros.lote],
      fornecedor: [this.filtros.fornecedor],
      notaFiscal: [this.filtros.notaFiscal],
      codigoProduto: [this.filtros.codigoProduto ? { value: this.filtros.codigoProduto, key: '' } : null],
      dataInicio: [this.filtros.dataEntrada],
    });
  }
}
