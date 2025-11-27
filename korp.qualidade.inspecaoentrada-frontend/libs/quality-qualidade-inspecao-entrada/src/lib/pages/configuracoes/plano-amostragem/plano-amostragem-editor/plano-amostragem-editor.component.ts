import { Component, OnInit, Inject } from '@angular/core';

import { ConfiguracoesService } from '../../configuracoes.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PlanoAmostragem } from 'libs/quality-qualidade-inspecao-entrada/src/lib/tokens/interfaces/planos-amostragem-dto.interface';
import { UUID } from 'angular2-uuid';

@Component({
  selector: 'inspecao-entrada-plano-amostragem-editor',
  templateUrl: './plano-amostragem-editor.component.html',
  styleUrls: ['./plano-amostragem-editor.component.css']
})
export class PlanoAmostragemEditorComponent implements OnInit {

  public formCustomValidatorMessage = '';
  public form: FormGroup;

  quantiadesCoerentesValidator() {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      if (Number(control.get('quantidadeMinima').value) > Number(control.get('quantidadeMaxima').value)) {
        this.formCustomValidatorMessage = "Quantidade Mínima ultrapassa Quantidade Máxima";
        return { 'invalidValue': true };
      } else if (Number(control.get('quantidadeInspecionar').value) > Number(control.get('quantidadeMaxima').value)) {
        this.formCustomValidatorMessage = "Quantidade Inspecionar ultrapassa Quantidade Máxima";
        return { 'invalidValue': true };
      } /* else if(Number(control.get('quantidadeInspecionar').value) < Number(control.get('quantidadeMinima').value)) {
        this.formCustomValidatorMessage = "Quantidade Inspecionar menor que Quantidade Mínima";
        return { 'invalidValue': true };
      } */
      this.formCustomValidatorMessage = "";
      return null;
    };
  }

  constructor(private configuracoesService: ConfiguracoesService, private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) private data: PlanoAmostragem, private dialogRef: MatDialogRef<PlanoAmostragemEditorComponent>) {
  }

  ngOnInit(): void {
    this.iniciarForm();
  }

  private iniciarForm(): void {
    this.form = this.formBuilder.group({
      id: [this.data?.id ?? UUID.UUID()],
      quantidadeMinima: [this.data?.quantidadeMinima, [Validators.min(0.00001), Validators.required]],
      quantidadeMaxima: [this.data?.quantidadeMaxima, [Validators.min(0.00001), Validators.required]],
      quantidadeInspecionar: [this.data?.quantidadeInspecionar, [Validators.min(0.00001), Validators.required]],
    }, { validators: [this.quantiadesCoerentesValidator()] });
  }

  public async salvar() {
    if (this.data) {
      await this.configuracoesService.atualizarPlanoAmostragem(this.data.id, {
        id: this.form.get('id').value,
        quantidadeMinima: Number(this.form.get('quantidadeMinima').value),
        quantidadeMaxima: Number(this.form.get('quantidadeMaxima').value),
        quantidadeInspecionar: Number(this.form.get('quantidadeInspecionar').value)
      });
    } else {
      await this.configuracoesService.criarPlanoAmostragem({
        id: this.form.get('id').value,
        quantidadeMinima: Number(this.form.get('quantidadeMinima').value),
        quantidadeMaxima: Number(this.form.get('quantidadeMaxima').value),
        quantidadeInspecionar: Number(this.form.get('quantidadeInspecionar').value)
      });
    }
    this.dialogRef.close();
  }

}
