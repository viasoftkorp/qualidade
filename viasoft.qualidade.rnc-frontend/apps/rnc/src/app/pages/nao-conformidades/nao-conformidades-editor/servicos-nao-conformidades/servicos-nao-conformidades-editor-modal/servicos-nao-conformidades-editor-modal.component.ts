import {
  Component, Inject, OnDestroy, OnInit
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { UUID } from 'angular2-uuid';
import { ServicosNaoConformidadesInput, ServicosNaoConformidadesModel } from '@viasoft/rnc/api-clients/Nao-Conformidades';
import { MessageService, VsSubscriptionManager } from '@viasoft/common';
import { HttpErrorResponse } from '@angular/common/http';
import { finalize } from 'rxjs/operators';
import { tempoTotalValidator } from '@viasoft/rnc/app/pages/shared/validators/servicos/tempo-total.validator';
// eslint-disable-next-line max-len
import { ServicoValidationResult } from '@viasoft/rnc/api-clients/Nao-Conformidades/Servicos-Solucoes-Nao-Conformidades/model/serviceValidationResult';
import { ServicosNaoConformidadesFormControl } from './servicos-nao-conformidades-form-control';
import { ServicosNaoConformidadesService } from '../servicos-nao-conformidades.service';
import { NaoConformidadesEditorService } from '../../nao-conformidades-editor.service';
import { getMascaraCampoNumerico } from '@viasoft/rnc/app/tokens/consts/mask-decimal-place';

@Component({
  selector: 'rnc-servicos-nao-conformidades-editor-modal',
  templateUrl: './servicos-nao-conformidades-editor-modal.component.html',
  styleUrls: ['./servicos-nao-conformidades-editor-modal.component.scss']
})
export class ServicosNaoConformidadesEditorModalComponent implements OnInit, OnDestroy {
 public form: FormGroup = new FormGroup({});
  public editorAction: EditorAction;
  public formControls = ServicosNaoConformidadesFormControl;
  public loaded = false;
  public subscriptionManager = new VsSubscriptionManager();
  public canEditServicoNaoConformidade = true;
  public processando = false;
  public servicoNaoConformidade: ServicosNaoConformidadesModel | null;
  public mascaraCampoNumerico = (numeroCasasDecimais: number) => getMascaraCampoNumerico(numeroCasasDecimais)

  constructor(
    private formBuilder: FormBuilder,
    private service: ServicosNaoConformidadesService,
    private dialogRef: MatDialogRef<ServicosNaoConformidadesEditorModalComponent>,
    private naoConformidadesEditorService:NaoConformidadesEditorService,
    private messageService: MessageService,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<ServicosNaoConformidadesModel>
  ) {
    this.editorAction = data.action;
    this.servicoNaoConformidade = data.data;
  }
  public ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public ngOnInit(): void {
    this.createForm();
    if (this.editorAction === EditorAction.Create) {
      this.form.get(ServicosNaoConformidadesFormControl.id).setValue(UUID.UUID());
      this.loaded = true;
    } else {
      this.populateForm(this.servicoNaoConformidade);
    }

    this.subscribeBloquearAtualizacaoNaoConformidade();
  }

  public canSave():boolean {
    return !this.processando && this.form.valid && this.form.dirty && this.canEditServicoNaoConformidade;
  }

  public save():void {
    if (!this.canSave()) {
      return;
    }
    const servicoSolucao = this.form.getRawValue() as ServicosNaoConformidadesInput;

    if (!servicoSolucao.horas) {
      servicoSolucao.horas = 0;
    }
    if (!servicoSolucao.minutos) {
      servicoSolucao.minutos = 0;
    }

    servicoSolucao.idNaoConformidade = this.data.data.idNaoConformidade;
    if (!servicoSolucao.horas) {
      servicoSolucao.horas = 0;
    }
    if (!servicoSolucao.minutos) {
      servicoSolucao.minutos = 0;
    }
    const saveAction = this.editorAction === EditorAction.Create
      ? this.service.create(servicoSolucao, servicoSolucao.idNaoConformidade)
      : this.service.update(servicoSolucao.id, servicoSolucao.idNaoConformidade, servicoSolucao);

    this.processando = true;

    saveAction
      .pipe(finalize(() => {
        this.processando = false;
      }))
      .subscribe({
        next: (result: ServicoValidationResult) => {
          if (result === ServicoValidationResult.Ok) {
            this.form.markAsPristine();
            this.dialogRef.close(servicoSolucao);
          }
        },
        error: (error:HttpErrorResponse) => {
          const result = error.error as ServicoValidationResult;

          this.messageService
            .warn(`NaoConformidade.ServicosSolucoesModal.ServicoValidationResult.${ServicoValidationResult[result]}`);
        }
      });
  }

  private createForm() {
    this.form = this.formBuilder.group({});
    this.form.addValidators(tempoTotalValidator(ServicosNaoConformidadesFormControl.minutos,
      ServicosNaoConformidadesFormControl.horas));

    this.form.addControl(ServicosNaoConformidadesFormControl.id, this.formBuilder.control(null));
    this.form.addControl(ServicosNaoConformidadesFormControl.idProduto, this.formBuilder.control(null));
    this.form.addControl(ServicosNaoConformidadesFormControl
      .quantidade, this.formBuilder.control(1, [Validators.min(0), Validators.required]));
    this.form.addControl(ServicosNaoConformidadesFormControl.idRecurso,
      this.formBuilder.control(null, Validators.required));
    this.form.addControl(ServicosNaoConformidadesFormControl.horas,
      this.formBuilder.control(0));
    this.form.addControl(ServicosNaoConformidadesFormControl.minutos,
      this.formBuilder.control(0));
    this.form.addControl(ServicosNaoConformidadesFormControl.operacaoEngenharia,
      this.formBuilder.control(null, [Validators.required]));
    this.form.addControl(ServicosNaoConformidadesFormControl.detalhamento,
      this.formBuilder.control(null));
    this.form.addControl(ServicosNaoConformidadesFormControl.controlarApontamento,
      this.formBuilder.control(false));
  }

  private populateForm(servicoNaoConformidade: ServicosNaoConformidadesModel) {
    this.form.get(ServicosNaoConformidadesFormControl.id)
      .setValue(servicoNaoConformidade.id, { emitEvent: false });
    this.form.get(ServicosNaoConformidadesFormControl.idProduto)
      .setValue(servicoNaoConformidade.idProduto, { emitEvent: false });
    this.form.get(ServicosNaoConformidadesFormControl.idRecurso)
      .setValue(servicoNaoConformidade.idRecurso, { emitEvent: false });
    this.form.get(ServicosNaoConformidadesFormControl.horas)
      .setValue(servicoNaoConformidade.horas, { emitEvent: false });
    this.form.get(ServicosNaoConformidadesFormControl.minutos)
      .setValue(servicoNaoConformidade.minutos, { emitEvent: false });
    this.form.get(ServicosNaoConformidadesFormControl.operacaoEngenharia)
      .setValue(servicoNaoConformidade.operacaoEngenharia, { emitEvent: false });
    this.form.get(ServicosNaoConformidadesFormControl.detalhamento)
      .setValue(servicoNaoConformidade.detalhamento, { emitEvent: false });
    this.form.get(ServicosNaoConformidadesFormControl.controlarApontamento)
      .setValue(servicoNaoConformidade.controlarApontamento, { emitEvent: false });
    this.loaded = true;
  }

  private subscribeBloquearAtualizacaoNaoConformidade() {
    this.subscriptionManager.add(
      'bloquearAtualizacaoNaoConformidade',
      this.naoConformidadesEditorService.validarAtualizacaoNaoConformidade.subscribe(() => {
        if (this.naoConformidadesEditorService.naoConformidadeNaoConcluida &&
          !this.naoConformidadesEditorService.naoConformidadeTemRetrabalhoGerado) {
          this.form.enable();
          this.canEditServicoNaoConformidade = true;
        } else {
          this.form.disable();
          this.canEditServicoNaoConformidade = false;
        }
      })
    );
  }
}
