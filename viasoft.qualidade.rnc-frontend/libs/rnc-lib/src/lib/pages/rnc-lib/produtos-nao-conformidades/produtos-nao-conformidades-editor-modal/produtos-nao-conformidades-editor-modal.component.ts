import { AfterViewInit, Component, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { VsDialogComponent, VsMaskBase } from '@viasoft/components';
import { EditorAction } from '../../../../tokens/classes/editor-action.enum';
import { EditorModalData } from '../../../../tokens/classes/editor-modal-data';
import { getMascaraCampoNumerico, setMaskDecimalPlace } from '../../../../tokens/classes/mask-decimal-place';
import { UUID } from 'angular2-uuid';
import { ProdutosNaoConformidadesFormControl } from './produtos-nao-conformidades-form-control';
import { ProdutosNaoConformidadesService } from '../produtos-nao-conformidades.service';
import { ProdutosNaoConformidadesInput, ProdutosNaoConformidadesOutput } from '../../../../api-clients/Nao-Conformidades/Produtos-Nao-Conformidades/model';
import { VsSubscriptionManager } from '@viasoft/common';
import { NaoConformidadesEditorService } from '../../rnc-editor-modal/nao-conformidades-editor.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'rnc-produtos-nao-conformidades-editor-modal',
  templateUrl: './produtos-nao-conformidades-editor-modal.component.html',
  styleUrls: ['./produtos-nao-conformidades-editor-modal.component.scss']
})
export class ProdutosNaoConformidadesEditorModalComponent implements AfterViewInit, OnDestroy {
  @ViewChild(VsDialogComponent) private vsDialogComponent: VsDialogComponent;

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

  ngAfterViewInit(): void {
    this.vsDialogComponent.close = () => this.dialogRef.close();
  }

  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public canSave() {
    return !this.processando && this.form.valid && this.form.dirty && this.canEditProdutoNaoConformidade;
  }
  public save() {
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
        if (this.naoConformidadesEditorService.somenteLeitura) {
          this.form.disable();
          this.canEditProdutoNaoConformidade = false;
        } else {
          if (this.naoConformidadesEditorService.naoConformidadeNaoConcluida
            && !this.naoConformidadesEditorService.naoConformidadeTemRetrabalhoGerado) {
            this.form.enable();
            this.canEditProdutoNaoConformidade = true;
          } else {
            this.form.disable();
            this.canEditProdutoNaoConformidade = false;
          }
        }
      })
    );
  }
}
