import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { VsAutocompleteOption, VsMaskBase } from '@viasoft/components';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { setMaskDecimalPlace } from '@viasoft/rnc/app/tokens/consts/mask-decimal-place';
import { UUID } from 'angular2-uuid';
import { VsSubscriptionManager } from '@viasoft/common';
import { MateriaisFormControl } from './materiais-form-control';

@Component({
  selector: 'rnc-materiais-editor-modal',
  templateUrl: './materiais-editor-modal.component.html',
  styleUrls: ['./materiais-editor-modal.component.scss'],
})
export class MateriaisEditorModalComponent implements OnInit, OnDestroy {
  public form: FormGroup = new FormGroup({});
  public formControls = MateriaisFormControl;
  public processando = false;
  public idMaquina: string;
  private editorAction: EditorAction;
  private subscriptionManager = new VsSubscriptionManager();

  constructor(
    private formBuilder: FormBuilder,
    private dialogRef: MatDialogRef<MateriaisEditorModalComponent>,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<{ idMaquina: string; material: FormGroup }>
  ) {
    this.editorAction = data.action;
    this.idMaquina = data.data.idMaquina;
  }

  public ngOnInit(): void {
    if (this.editorAction === EditorAction.Create) {
      this.createForm();
      this.form.get(MateriaisFormControl.id).setValue(UUID.UUID());
    } else {
      this.form = this.data.data.material;
    }
  }

  public ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public produtoModificadoHandle(produto: VsAutocompleteOption<string, string> | null) {
    const descricaoProdutoFormControl = this.form.get(this.formControls.descricao);

    if (produto) {
      descricaoProdutoFormControl.setValue(produto.name);
    } else {
      descricaoProdutoFormControl.setValue(null);
    }
  }

  public mascaraCampoNumerico(casasDecimais: number): VsMaskBase {
    return {
      type: 'custom',
      imask: setMaskDecimalPlace(casasDecimais),
    } as VsMaskBase;
  }

  public closeModal() {
    this.dialogRef.close(null);
  }

  public canSave(): boolean {
    return !this.processando && this.form.valid && this.form.dirty;
  }

  public save(): void {
    if (!this.canSave()) {
      return;
    }

    this.dialogRef.close(this.form);
  }

  private createForm() {
    this.form = this.formBuilder.group({});

    this.form.addControl(MateriaisFormControl.id, this.formBuilder.control(null));

    this.form.addControl(MateriaisFormControl.idProduto, this.formBuilder.control(null, Validators.required));
    this.form.addControl(MateriaisFormControl.quantidade, this.formBuilder.control(0));
    this.form.addControl(MateriaisFormControl.detalhamento, this.formBuilder.control(null));
    this.form.addControl(MateriaisFormControl.descricao, this.formBuilder.control(null));
    this.form.addControl(MateriaisFormControl.idMaquina, this.formBuilder.control(this.idMaquina));
  }
}
