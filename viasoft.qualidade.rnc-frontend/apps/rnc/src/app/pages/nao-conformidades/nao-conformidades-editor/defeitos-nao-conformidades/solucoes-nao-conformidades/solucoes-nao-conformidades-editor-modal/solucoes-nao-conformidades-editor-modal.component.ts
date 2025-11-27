import {
  Component, Inject, OnDestroy, OnInit
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { VsMaskBase } from '@viasoft/components';
import { SolucoesNaoConformidadesModel } from '@viasoft/rnc/api-clients/Nao-Conformidades';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { UUID } from 'angular2-uuid';
import { SolucaoService } from '@viasoft/rnc/app/pages/settings/solucao/solucao.service';
import { VsSubscriptionManager } from '@viasoft/common';
import { SolucoesNaoConformidadesService } from '../solucoes-nao-conformidades.service';
import { SolucoesNaoConformidadesFormControls } from './solucoes-nao-conformidades-form-controls';
import { NaoConformidadesEditorService } from '../../../nao-conformidades-editor.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'rnc-solucoes-nao-conformidades-editor-modal',
  templateUrl: './solucoes-nao-conformidades-editor-modal.component.html',
  styleUrls: ['./solucoes-nao-conformidades-editor-modal.component.scss']
})
export class SolucoesNaoConformidadesEditorModalComponent implements OnInit, OnDestroy {
  private subscriptionManager = new VsSubscriptionManager();
  public editorAction: EditorAction;
  public form: FormGroup = new FormGroup({});
  public formControls = SolucoesNaoConformidadesFormControls;
  public loaded = false;
  public idSolucaoNaoConformidade: string;
  public canEditSolucaoNaoConformidade = true;
  public currencymask = {
    type: 'currency',
    options: {
      scale: 6
    }
  } as VsMaskBase;
  public processando = false;

  constructor(
    private formBuilder: FormBuilder,
    private service: SolucoesNaoConformidadesService,
    private solucoesService: SolucaoService,
    private dialogRef: MatDialogRef<SolucoesNaoConformidadesEditorModalComponent>,
    private naoConformidadesEditorService: NaoConformidadesEditorService,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<SolucoesNaoConformidadesModel>
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
  public fechar(): void {
    this.dialogRef.close();
  }
  public completaCadastro(idSolucao:string): void {
    if (idSolucao) {
      this.solucoesService.get(idSolucao).subscribe((data) => {
        this.form.get(SolucoesNaoConformidadesFormControls.detalhamento).setValue(data.detalhamento);
      });
    }
  }

  public canSave():boolean {
    return !this.processando && this.form.valid && this.form.dirty && this.canEditSolucaoNaoConformidade;
  }

  public save():void {
    if (!this.canSave()) {
      return;
    }
    const solucao = this.form.getRawValue() as SolucoesNaoConformidadesModel;
    if (!solucao.custoEstimado) {
      solucao.custoEstimado = 0;
    }
    solucao.idDefeitoNaoConformidade = this.data.data.idDefeitoNaoConformidade;
    solucao.idNaoConformidade = this.data.data.idNaoConformidade;

    this.processando = true;

    if (this.editorAction === EditorAction.Create) {
      this.service.create(solucao, solucao.idNaoConformidade)
        .pipe(finalize(() => {
          this.processando = false;
        }))
        .subscribe({
          next: () => {
            this.form.markAsPristine();
            this.dialogRef.close(solucao);
          }
        });
    } else {
      this.service.update(solucao.id, solucao.idNaoConformidade, solucao)
        .pipe(finalize(() => {
          this.processando = false;
        }))
        .subscribe({
          next: () => {
            this.form.markAsPristine();
            this.dialogRef.close(solucao);
          }
        });
    }
  }

  private formInit() {
    this.form = this.formBuilder.group({});
    this.form.addControl(SolucoesNaoConformidadesFormControls.id,
      this.formBuilder.control(this.data.data?.id || UUID.UUID()));
    this.idSolucaoNaoConformidade = this.form.controls.id.value as string;
    this.form.addControl(SolucoesNaoConformidadesFormControls.idSolucao,
      this.formBuilder.control(this.data.data?.idSolucao, Validators.required));
    this.form.addControl(SolucoesNaoConformidadesFormControls.detalhamento,
      this.formBuilder.control(this.data.data?.detalhamento));
    this.form.addControl(SolucoesNaoConformidadesFormControls.custoEstimado,
      this.formBuilder.control(this.data.data?.custoEstimado, [Validators.min(0)]));
    this.form.addControl(SolucoesNaoConformidadesFormControls.dataAnalise,
      this.formBuilder.control(this.data.data?.dataAnalise));
    this.form.addControl(SolucoesNaoConformidadesFormControls.solucaoImediata,
      this.formBuilder.control(this.data.data?.solucaoImediata || false));
    this.form.addControl(SolucoesNaoConformidadesFormControls.dataPrevistaImplantacao,
      this.formBuilder.control(this.data.data?.dataPrevistaImplantacao));
    this.form.addControl(SolucoesNaoConformidadesFormControls.dataVerificacao,
      this.formBuilder.control(this.data.data?.dataVerificacao));
    this.form.addControl(SolucoesNaoConformidadesFormControls.idResponsavel,
      this.formBuilder.control(this.data.data?.idResponsavel));
    this.form.addControl(SolucoesNaoConformidadesFormControls.idAuditor,
      this.formBuilder.control(this.data.data?.idAuditor));
    this.form.addControl(SolucoesNaoConformidadesFormControls.novaData,
      this.formBuilder.control(this.data.data?.novaData));
    this.loaded = true;
  }
  private subscribeBloquearAtualizacaoNaoConformidade() {
    this.subscriptionManager.add(
      'bloquearAtualizacaoNaoConformidade',
      this.naoConformidadesEditorService.validarAtualizacaoNaoConformidade.subscribe(() => {
        if (this.naoConformidadesEditorService.naoConformidadeNaoConcluida) {
          this.form.enable();
          this.canEditSolucaoNaoConformidade = true;
        } else {
          this.form.disable();
          this.canEditSolucaoNaoConformidade = false;
        }
      })
    );
  }
}
