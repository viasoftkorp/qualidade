import {
  Component, Inject, OnDestroy, OnInit
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { VsAutocompleteOption, VsMaskBase } from '@viasoft/components';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { getMascaraCampoNumerico, setMaskDecimalPlace } from '@viasoft/rnc/app/tokens/consts/mask-decimal-place';
import { UUID } from 'angular2-uuid';
import { VsSubscriptionManager } from '@viasoft/common';
import { MaquinasFormControl } from './maquinas-form-control';
import { tempoTotalValidator } from 'libs/rnc-lib/src/lib/validators/servicos/tempo-total.validator';

@Component({
  selector: 'rnc-maquinas-editor-modal',
  templateUrl: './maquinas-editor-modal.component.html',
  styleUrls: ['./maquinas-editor-modal.component.scss']
})
export class MaquinasEditorModalComponent implements OnInit, OnDestroy {
  public form: FormGroup = new FormGroup({});
  public formControls = MaquinasFormControl;
  public processando = false;
  public mascaraCampoNumerico = (numeroCasasDecimais: number) => getMascaraCampoNumerico(numeroCasasDecimais)
  private editorAction: EditorAction;
  private subscriptionManager = new VsSubscriptionManager();

  constructor(
    private formBuilder: FormBuilder,
    private dialogRef: MatDialogRef<MaquinasEditorModalComponent>,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<FormGroup>
  ) {
    this.editorAction = data.action;
  }

  public ngOnInit(): void {
    if (this.editorAction === EditorAction.Create) {
      this.createForm();
      this.form.get(MaquinasFormControl.id).setValue(UUID.UUID());
    } else {
      this.form = this.data.data
    }
  }

  public ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public recursoModificadoHandle(recurso:VsAutocompleteOption<string, string> | null) {
    const descricaoRecursoFormControl = this.form.get(this.formControls.descricao);

    if(recurso) {
      descricaoRecursoFormControl.setValue(recurso.name)
    } else {
      descricaoRecursoFormControl.setValue(null)
    }
  }

  public closeModal() {
    this.dialogRef.close(null);
  }

  public canSave():boolean {
    return !this.processando && this.form.valid && this.form.dirty;
  }

  public save():void {
    if (!this.canSave()) {
      return;
    }

    const horasFormControl = this.form.get(this.formControls.horas);

    if (!horasFormControl.value) {
      horasFormControl.setValue(0);
    }

    const minutosFormControl = this.form.get(this.formControls.minutos);

    if (!minutosFormControl.value) {
      minutosFormControl.setValue(0);
    }

    this.dialogRef.close(this.form);
  }

  private createForm() {
    this.form = this.formBuilder.group({});
    this.form.addValidators(tempoTotalValidator(MaquinasFormControl.minutos,
      MaquinasFormControl.horas));

    this.form.addControl(MaquinasFormControl.id, this.formBuilder.control(null));

    this.form.addControl(MaquinasFormControl.idRecurso,
      this.formBuilder.control(null, Validators.required));
    this.form.addControl(MaquinasFormControl.horas,
      this.formBuilder.control(0));
    this.form.addControl(MaquinasFormControl.minutos,
      this.formBuilder.control(0));
    this.form.addControl(MaquinasFormControl.detalhamento,
      this.formBuilder.control(null));
    this.form.addControl(MaquinasFormControl.descricao,
      this.formBuilder.control(null));

    this.form.addControl(MaquinasFormControl.materiais,
      this.formBuilder.array([]));
  }
}
