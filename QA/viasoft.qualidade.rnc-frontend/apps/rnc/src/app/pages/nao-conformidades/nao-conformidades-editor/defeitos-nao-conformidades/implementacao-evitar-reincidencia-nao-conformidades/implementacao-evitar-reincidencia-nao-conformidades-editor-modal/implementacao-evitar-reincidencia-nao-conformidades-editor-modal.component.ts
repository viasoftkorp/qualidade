import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { VsSubscriptionManager } from '@viasoft/common';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
// eslint-disable-next-line max-len
import { ImplementacaoEvitarReincidenciaNaoConformidadesModel } from '@viasoft/rnc/api-clients/Nao-Conformidades/Implementacao-Evitar-Reincidencia-Nao-Conformidades/model/implementacao-evitar-reincidencia-nao-conformidades-model';
// eslint-disable-next-line max-len
import { ImplementacaoEvitarReincidenciaNaoConformidadesInput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Implementacao-Evitar-Reincidencia-Nao-Conformidades/model/implementacao-evitar-reincidencia-nao-conformidades-input';
import { UUID } from 'angular2-uuid';
// eslint-disable-next-line max-len
import { ImplementacaoEvitarReincidenciaNaoConformidadesFormControl } from './implementacao-evitar-reincidencia-nao-conformidades-form-control';
// eslint-disable-next-line max-len
import { ImplementacaoEvitarReincidenciaNaoConformidadesService } from '../implementacao-evitar-reincidencia-nao-conformidades.service';
import { NaoConformidadesEditorService } from '../../../nao-conformidades-editor.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'rnc-implementacao-evitar-reincidencia-nao-conformidades-editor-modal',
  templateUrl: './implementacao-evitar-reincidencia-nao-conformidades-editor-modal.component.html',
  styleUrls: ['./implementacao-evitar-reincidencia-nao-conformidades-editor-modal.component.scss']
})
export class ImplementacaoEvitarReincidenciaNaoConformidadesEditorModalComponent implements OnInit {
  private subscriptionManager = new VsSubscriptionManager()
  public form: FormGroup;
  public editorAction: EditorAction;
  public formControls = ImplementacaoEvitarReincidenciaNaoConformidadesFormControl;
  public loaded = false;
  public canNaoConformidade = true;
  public implementacaoEvitarReincidenciaData: ImplementacaoEvitarReincidenciaNaoConformidadesModel
  public processando = false;

  constructor(
    private formBuilder: FormBuilder,
    private service: ImplementacaoEvitarReincidenciaNaoConformidadesService,
    private dialogRef: MatDialogRef<ImplementacaoEvitarReincidenciaNaoConformidadesEditorModalComponent>,
    private naoConformidadesEditorService: NaoConformidadesEditorService,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<ImplementacaoEvitarReincidenciaNaoConformidadesInput>
  ) {
    this.implementacaoEvitarReincidenciaData = data.data;
    this.editorAction = data.action;
    this.createForm();
  }
  public ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public ngOnInit(): void {
    if (this.editorAction === EditorAction.Update) {
      this.populateForm(this.implementacaoEvitarReincidenciaData);
    } else {
      this.loaded = true;
    }

    this.subscribeBloquearAtualizacaoNaoConformidade();
  }
  public fechar(): void {
    this.dialogRef.close();
  }
  public canSave():boolean {
    return !this.processando && this.form.valid && this.form.dirty && this.canNaoConformidade;
  }
  public save():void {
    if (!this.canSave()) {
      return;
    }
    const input = this.form.getRawValue() as ImplementacaoEvitarReincidenciaNaoConformidadesInput;
    input.idNaoConformidade = this.implementacaoEvitarReincidenciaData.idNaoConformidade;
    input.idDefeitoNaoConformidade = this.implementacaoEvitarReincidenciaData.idDefeitoNaoConformidade;
    if (this.editorAction === EditorAction.Create) {
      input.id = UUID.UUID();
      this.processando = true;

      this.service.create(input)
        .pipe(finalize(() => {
          this.processando = false;
        }))
        .subscribe({
          next: () => {
            this.form.markAsPristine();
            this.dialogRef.close(input);
          }
        });
    } else {
      input.id = this.implementacaoEvitarReincidenciaData.id;
      this.processando = true;

      this.service.update(input)
        .pipe(finalize(() => {
          this.processando = false;
        }))
        .subscribe({
          next: () => {
            this.form.markAsPristine();
            this.dialogRef.close(input);
          }
        });
    }
  }

  private createForm() {
    this.form = this.formBuilder.group({});
    this.form.addControl(this.formControls.descricao, this.formBuilder.control(null, [Validators.required]));
    this.form.addControl(this.formControls.idResponsavel, this.formBuilder.control(null));
    this.form.addControl(this.formControls.dataAnalise, this.formBuilder.control(null));
    this.form.addControl(this.formControls.dataPrevistaImplantacao, this.formBuilder.control(null));
    this.form.addControl(this.formControls.idAuditor, this.formBuilder.control(null));
    this.form.addControl(this.formControls.dataVerificacao, this.formBuilder.control(null));
    this.form.addControl(this.formControls.novaData, this.formBuilder.control(null));
    this.form.addControl(this.formControls.acaoImplementada, this.formBuilder.control(false));
  }

  private populateForm(implementacao:ImplementacaoEvitarReincidenciaNaoConformidadesModel) {
    this.form.get(this.formControls.descricao).setValue(implementacao.descricao);
    this.form.get(this.formControls.idResponsavel).setValue(implementacao.idResponsavel);
    this.form.get(this.formControls.dataAnalise).setValue(implementacao.dataAnalise);
    this.form.get(this.formControls.dataPrevistaImplantacao).setValue(implementacao.dataPrevistaImplantacao);
    this.form.get(this.formControls.idAuditor).setValue(implementacao.idAuditor);
    this.form.get(this.formControls.dataVerificacao).setValue(implementacao.dataVerificacao);
    this.form.get(this.formControls.novaData).setValue(implementacao.novaData);
    this.form.get(this.formControls.acaoImplementada).setValue(implementacao.acaoImplementada);
    this.loaded = true;
  }
  private subscribeBloquearAtualizacaoNaoConformidade() {
    this.subscriptionManager.add(
      'bloquearAtualizacaoNaoConformidade',
      this.naoConformidadesEditorService.validarAtualizacaoNaoConformidade.subscribe(() => {
        if (this.naoConformidadesEditorService.naoConformidadeNaoConcluida) {
          this.form.enable();
          this.canNaoConformidade = true;
        } else {
          this.form.disable();
          this.canNaoConformidade = false;
        }
      })
    );
  }
}
