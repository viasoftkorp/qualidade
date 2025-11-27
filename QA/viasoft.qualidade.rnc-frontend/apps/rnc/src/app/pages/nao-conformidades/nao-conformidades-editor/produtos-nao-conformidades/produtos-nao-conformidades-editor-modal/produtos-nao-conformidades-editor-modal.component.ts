import {
  Component, Inject, OnDestroy, OnInit
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { getMascaraCampoNumerico } from '@viasoft/rnc/app/tokens/consts/mask-decimal-place';
import { UUID } from 'angular2-uuid';
// eslint-disable-next-line max-len
import { ProdutosNaoConformidadesInput, ProdutosNaoConformidadesOutput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Produtos-Nao-Conformidades/model';
import { VsSubscriptionManager } from '@viasoft/common';
import { finalize } from 'rxjs/operators';
import { ProdutosNaoConformidadesFormControl } from './produtos-nao-conformidades-form-control';
import { ProdutosNaoConformidadesService } from '../produtos-nao-conformidades.service';
import { NaoConformidadesEditorService } from '../../nao-conformidades-editor.service';

@Component({
  selector: 'rnc-produtos-nao-conformidades-editor-modal',
  templateUrl: './produtos-nao-conformidades-editor-modal.component.html',
  styleUrls: ['./produtos-nao-conformidades-editor-modal.component.scss']
})
export class ProdutosNaoConformidadesEditorModalComponent implements OnInit, OnDestroy {
  private subscriptionManager = new VsSubscriptionManager();
  public form: FormGroup = new FormGroup({});
  public editorAction: EditorAction;
  public formControls = ProdutosNaoConformidadesFormControl;
  public loaded = false;
  public canEditProdutoNaoConformidade = true;
  public processando = false;
  public mascaraCampoNumerico = (numeroCasasDecimais: number) => getMascaraCampoNumerico(numeroCasasDecimais)

  constructor(
    private formBuilder: FormBuilder,
    private service: ProdutosNaoConformidadesService,
    private dialogRef: MatDialogRef<ProdutosNaoConformidadesEditorModalComponent>,
    private naoConformidadesEditorService:NaoConformidadesEditorService,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<ProdutosNaoConformidadesOutput>
  ) {
    this.editorAction = data.action;
    this.formInit();
    this.subscribeBloquearAtualizacaoNaoConformidade();
  }
  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  ngOnInit(): void { // vazio
  }

  public canSave():boolean {
    return !this.processando && this.form.valid && this.form.dirty && this.canEditProdutoNaoConformidade;
  }
  public save():void {
    if (!this.canSave()) {
      return;
    }
    const produtoSolucao = this.form.getRawValue() as ProdutosNaoConformidadesInput;
    produtoSolucao.idNaoConformidade = this.data.data.idNaoConformidade;

    const saveAction = this.editorAction === EditorAction.Create
      ? this.service.create(produtoSolucao, produtoSolucao.idNaoConformidade)
      : this.service.update(produtoSolucao.id, produtoSolucao.idNaoConformidade, produtoSolucao);
    this.processando = true;

    saveAction
      .pipe(finalize(() => {
        this.processando = false;
      }))
      .subscribe({
        next: () => {
          this.form.markAsPristine();
          this.dialogRef.close(produtoSolucao);
        }
      });
  }

  private formInit() {
    this.form = this.formBuilder.group({});
    this.form.addControl(ProdutosNaoConformidadesFormControl
      .id, this.formBuilder.control(this.data.data?.id || UUID.UUID()));
    this.form.addControl(ProdutosNaoConformidadesFormControl
      .idProduto, this.formBuilder.control(this.data.data?.idProduto, Validators.required));
    this.form.addControl(ProdutosNaoConformidadesFormControl
      .quantidade, this.formBuilder.control(this.data.data?.quantidade, Validators.required));
    this.form.addControl(ProdutosNaoConformidadesFormControl
      .detalhamento, this.formBuilder.control(this.data.data?.detalhamento));
    this.form.addControl(ProdutosNaoConformidadesFormControl
      .operacaoEngenharia, this.formBuilder.control(this.data.data?.operacaoEngenharia, [Validators.required]));
    this.loaded = true;
  }
  private subscribeBloquearAtualizacaoNaoConformidade() {
    this.subscriptionManager.add(
      'bloquearAtualizacaoNaoConformidade',
      this.naoConformidadesEditorService.validarAtualizacaoNaoConformidade.subscribe(() => {
        if (this.naoConformidadesEditorService.naoConformidadeNaoConcluida
          && !this.naoConformidadesEditorService.naoConformidadeTemRetrabalhoGerado) {
          this.form.enable();
          this.canEditProdutoNaoConformidade = true;
        } else {
          this.form.disable();
          this.canEditProdutoNaoConformidade = false;
        }
      })
    );
  }
}
