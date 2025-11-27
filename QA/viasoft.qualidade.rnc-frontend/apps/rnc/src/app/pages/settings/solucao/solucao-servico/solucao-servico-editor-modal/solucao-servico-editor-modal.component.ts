import { SolucaoServicoInput } from '@viasoft/rnc/api-clients/Solucoes/model/solucao-servico-input';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { UUID } from 'angular2-uuid';
import { SolucaoServicoModel } from '@viasoft/rnc/api-clients/Solucoes/model/solucao-servico-model';
import {
  SolucaoServicoFormControls
} from '@viasoft/rnc/app/pages/settings/solucao/solucao-servico/solucao-servico-form-controls';
import { SolucaoService } from '@viasoft/rnc/app/pages/settings/solucao/solucao.service';
import { HttpErrorResponse } from '@angular/common/http';
import { MessageService } from '@viasoft/common';
// eslint-disable-next-line max-len
import { tempoTotalValidator } from '@viasoft/rnc/app/pages/shared/validators/servicos/tempo-total.validator';
// eslint-disable-next-line max-len
import { ServicoValidationResult } from '@viasoft/rnc/api-clients/Nao-Conformidades/Servicos-Solucoes-Nao-Conformidades/model/serviceValidationResult';

@Component({
  selector: 'rnc-solucao-servico-editor-modal',
  templateUrl: './solucao-servico-editor-modal.component.html',
  styleUrls: ['./solucao-servico-editor-modal.component.scss']
})
export class SolucaoServicoEditorModalComponent implements OnInit {
  public form: FormGroup = new FormGroup({});
  public editorAction: EditorAction;
  public formControls = SolucaoServicoFormControls;
  public loaded: boolean;
  public servico: SolucaoServicoModel | null;

  constructor(
    private formBuilder: FormBuilder,
    private service: SolucaoService,
    private dialogRef: MatDialogRef<SolucaoServicoEditorModalComponent>,
    private messageService:MessageService,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<SolucaoServicoModel>
  ) {
    this.editorAction = data.action;
    this.servico = data.data;
  }

  public ngOnInit(): void {
    this.createForm();
    if (this.editorAction === EditorAction.Create) {
      this.form.get(SolucaoServicoFormControls.idServicoSolucao).setValue(UUID.UUID());
      this.loaded = true;
    } else {
      this.populateForm(this.servico);
    }
  }

  public canSave(): boolean {
    return this.form.valid && this.form.dirty;
  }

  public save(): void {
    if (this.canSave) {
      if (!this.canSave()) {
        return;
      }
      const servico = this.form.getRawValue() as SolucaoServicoInput;

      if (!servico.horas) {
        servico.horas = 0;
      }
      if (!servico.minutos) {
        servico.minutos = 0;
      }

      servico.idSolucao = this.data.data.idSolucao;
      if (this.editorAction === EditorAction.Create) {
        servico.id = UUID.UUID();
      } else {
        servico.id = this.data.data.id;
      }
      const saveAction = this.editorAction === EditorAction.Create
        ? this.service.addServico(servico) : this.service.updateServico(servico);
      saveAction.subscribe({
        next: () => {
          this.dialogRef.close(servico);
        },
        error: (error:HttpErrorResponse) => {
          const result = error.error as ServicoValidationResult;

          this.messageService
            .warn(`Solucao.ServicoEditor.ServicoValidationResult.${ServicoValidationResult[result]}`);
        }
      });
    }
  }

  private createForm():void {
    this.form = this.formBuilder.group({});
    this.form.addValidators(tempoTotalValidator(SolucaoServicoFormControls.minutos,
      SolucaoServicoFormControls.horas));

    this.form.addControl(SolucaoServicoFormControls.idServicoSolucao,
      this.formBuilder.control(null));
    this.form.addControl(SolucaoServicoFormControls.idProduto,
      this.formBuilder.control(null));
    this.form.addControl(SolucaoServicoFormControls.quantidade,
      this.formBuilder.control(1, [Validators.required, Validators.min(0)]));
    this.form.addControl(SolucaoServicoFormControls.horas,
      this.formBuilder.control(0, [Validators.min(0)]));
    this.form.addControl(SolucaoServicoFormControls.minutos,
      this.formBuilder.control(0, [Validators.min(0)]));
    this.form.addControl(SolucaoServicoFormControls.idRecurso,
      this.formBuilder.control(null, [Validators.required]));
    this.form.addControl(SolucaoServicoFormControls.operacaoEngenharia,
      this.formBuilder.control(null, [Validators.min(0), Validators.required]));
  }

  private populateForm(servico: SolucaoServicoModel) {
    this.form.get(SolucaoServicoFormControls.idServicoSolucao).setValue(servico.id, { emitEvent: false });
    this.form.get(SolucaoServicoFormControls.idProduto).setValue(servico.idProduto, { emitEvent: false });
    this.form.get(SolucaoServicoFormControls.horas).setValue(servico.horas, { emitEvent: false });
    this.form.get(SolucaoServicoFormControls.minutos).setValue(servico.minutos, { emitEvent: false });
    this.form.get(SolucaoServicoFormControls.idRecurso).setValue(servico.idRecurso, { emitEvent: false });
    this.form.get(SolucaoServicoFormControls.operacaoEngenharia).setValue(servico.operacaoEngenharia, { emitEvent: false });
    this.loaded = true;
  }
}
