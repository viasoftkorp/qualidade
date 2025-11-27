import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { VsSelectOption } from '@viasoft/components';

import { MovimentarInspecaoStatus, ProcessamentoInspecaoSaidaFilters } from '../../../../tokens';

@Component({
  selector: 'qa-processamento-inspecao-view-filter',
  templateUrl: './processamento-inspecao-view-filter.component.html',
  styleUrls: ['./processamento-inspecao-view-filter.component.scss']
})
export class ProcessamentoInspecaoViewFilterComponent {
  public form: FormGroup;
  public opcoesStatus: VsSelectOption[];

  constructor(private formBuilder: FormBuilder, @Inject(MAT_DIALOG_DATA) private filtros: ProcessamentoInspecaoSaidaFilters,
              private dialogRef: MatDialogRef<ProcessamentoInspecaoViewFilterComponent>) {
    this.setOpcoesStatus();
    this.iniciarForm();
  }

  public filtrar(): void {
    this.filtros = this.form.getRawValue();
    this.dialogRef.close(this.filtros);
  }

  private setOpcoesStatus(): void {
    this.opcoesStatus = [
      { name: 'ProcessamentoInspecao.MovimentarInspecaoStatus.Inicio', value: MovimentarInspecaoStatus.Inicio },
      { name: 'ProcessamentoInspecao.MovimentarInspecaoStatus.EmProcesso', value: MovimentarInspecaoStatus.EmProcesso },
      { name: 'ProcessamentoInspecao.MovimentarInspecaoStatus.Falha', value: MovimentarInspecaoStatus.Falha },
      { name: 'ProcessamentoInspecao.MovimentarInspecaoStatus.Sucesso', value: MovimentarInspecaoStatus.Sucesso }
    ] as VsSelectOption[];
  }

  private iniciarForm(): void {
    this.form = this.formBuilder.group({
      status: [this.filtros.status],
      resultado: [this.filtros.resultado],
      quantidadeTotal: [this.filtros.quantidadeTotal],
      codigoProduto: [this.filtros.codigoProduto],
      odf: [this.filtros.odf],
      erro: [this.filtros.erro],
      numeroExecucoes: [this.filtros.numeroExecucoes],
      idUsuarioExecucao: [this.filtros.idUsuarioExecucao],
      dataExecucao: [this.filtros.dataExecucao],
    });
  }
}
