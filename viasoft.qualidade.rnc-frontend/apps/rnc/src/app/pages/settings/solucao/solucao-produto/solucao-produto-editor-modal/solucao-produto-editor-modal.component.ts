import { SolucaoProdutoInput } from '@viasoft/rnc/api-clients/Solucoes/model/solucao-produto-input';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { UUID } from 'angular2-uuid';
import { SolucaoProdutoModel } from '@viasoft/rnc/api-clients/Solucoes/model/solucao-produto-model';
import {
  SolucaoProdutoFormControls
} from '@viasoft/rnc/app/pages/settings/solucao/solucao-produto/solucao-produto-form-controls';
import { SolucaoService } from '@viasoft/rnc/app/pages/settings/solucao/solucao.service';
import { VsMaskBase } from '@viasoft/components';
import { getMascaraCampoNumerico, setMaskDecimalPlace } from '@viasoft/rnc/app/tokens/consts/mask-decimal-place';

@Component({
  selector: 'rnc-solucao-produto-editor-modal',
  templateUrl: './solucao-produto-editor-modal.component.html',
  styleUrls: ['./solucao-produto-editor-modal.component.scss']
})
export class SolucaoProdutoEditorModalComponent implements OnInit {
  public form: FormGroup = new FormGroup({});
  public editorAction: EditorAction;
  public formControls = SolucaoProdutoFormControls;
  public loaded: boolean;
  public id: string;
  public mascaraCampoNumerico = (numeroCasasDecimais: number) => getMascaraCampoNumerico(numeroCasasDecimais)

  constructor(
    private formBuilder: FormBuilder,
    private service: SolucaoService,
    private dialogRef: MatDialogRef<SolucaoProdutoEditorModalComponent>,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<SolucaoProdutoModel>
  ) {
    this.editorAction = data.action;
    this.formInit();
  }

  ngOnInit(): void { // vazio
  }

  public canSave() {
    return this.form.valid && this.form.dirty;
  }
  public save() {
    if (this.canSave) {
      if (!this.canSave()) {
        return;
      }
      const solucao = this.form.getRawValue() as SolucaoProdutoInput;
      solucao.idSolucao = this.data.data.idSolucao;
      if (this.editorAction === EditorAction.Create) {
        solucao.id = UUID.UUID();
      } else {
        solucao.id = this.data.data.id;
      }
      const saveAction = this.editorAction === EditorAction.Create
        ? this.service.addProduto(solucao) : this.service.updateProduto(solucao, solucao.id);
      saveAction.subscribe(() => {
        this.dialogRef.close(solucao);
      });
    }
  }

  private formInit() {
    this.form = this.formBuilder.group({});
    this.form.addControl(SolucaoProdutoFormControls.id,
      this.formBuilder.control(this.data.data?.id || UUID.UUID(), Validators.required));
    this.form.addControl(SolucaoProdutoFormControls.idProduto,
      this.formBuilder.control(this.data.data?.idProduto, Validators.required));
    this.form.addControl(SolucaoProdutoFormControls.quantidade,
      this.formBuilder.control(this.data.data?.quantidade, [Validators.required, Validators.min(0)]));
    this.form.addControl(SolucaoProdutoFormControls.operacaoEngenharia,
      this.formBuilder.control(this.data.data?.operacaoEngenharia, [Validators.required]));
  }
}
