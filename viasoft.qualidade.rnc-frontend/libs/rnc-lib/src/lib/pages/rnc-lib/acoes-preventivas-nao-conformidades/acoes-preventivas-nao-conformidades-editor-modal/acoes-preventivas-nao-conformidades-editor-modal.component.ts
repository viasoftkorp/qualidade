import {
  AfterViewInit,
  Component,
  Inject,
  OnDestroy,
  OnInit, ViewChild
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  AcoesPreventivasNaoConformidadesModel,
  AcoesPreventivasNaoConformidadesInput
} from '../../../../api-clients/Nao-Conformidades';
import { VsSubscriptionManager } from '@viasoft/common';
import { EditorAction } from '../../../../tokens/classes/editor-action.enum';
import { EditorModalData } from '../../../../tokens/classes/editor-modal-data';
import { UUID } from 'angular2-uuid';
import { AcoesPreventivasNaoConformidadesService } from '../acoes-preventivas-nao-conformidades.service';
import { AcoesPreventivasNaoConformidadesFormControl } from './acoes-preventivas-nao-conformidades-form-control';
import { NaoConformidadesEditorService } from '../../rnc-editor-modal/nao-conformidades-editor.service';
import { AcaoPreventivaService } from './acao-preventiva.service';
import { VsDialogComponent } from '@viasoft/components';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'rnc-acoes-preventivas-nao-conformidades-editor-modal',
  templateUrl: './acoes-preventivas-nao-conformidades-editor-modal.component.html',
  styleUrls: ['./acoes-preventivas-nao-conformidades-editor-modal.component.scss']
})
export class AcoesPreventivasNaoConformidadesEditorModalComponent implements AfterViewInit, OnDestroy {
  @ViewChild(VsDialogComponent) private vsDialogComponent: VsDialogComponent;

  private subscriptionManager = new VsSubscriptionManager()
  public form: FormGroup = new FormGroup({});
  public editorAction: EditorAction;
  public formControls = AcoesPreventivasNaoConformidadesFormControl;
  public loaded = false;
  public canEditCausaNaoConformidade = true;
  public processando = false;

  constructor(
    private formBuilder: FormBuilder,
    private service: AcoesPreventivasNaoConformidadesService,
    private acoesPreventivasService: AcaoPreventivaService,
    private dialogRef: MatDialogRef<AcoesPreventivasNaoConformidadesEditorModalComponent>,
    private naoConformidadesEditorService: NaoConformidadesEditorService,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<AcoesPreventivasNaoConformidadesModel>
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

  public completaCadastro(idAcao:string): void {
    if(idAcao) {
      this.acoesPreventivasService.get(idAcao).subscribe((data) => {
        this.form.get(AcoesPreventivasNaoConformidadesFormControl.detalhamento).setValue(data.detalhamento);
      });
    }
  }
  public canSave() {
    return !this.processando && this.form.valid && this.form.dirty && this.canEditCausaNaoConformidade;
  }
  public save() {
    if (!this.canSave()) {
      return;
    }
    const acoesPreventivas = this.form.getRawValue() as AcoesPreventivasNaoConformidadesInput;
    acoesPreventivas.idNaoConformidade = this.data.data.idNaoConformidade;
    acoesPreventivas.idDefeitoNaoConformidade = this.data.data.idDefeitoNaoConformidade;
    const saveAction = this.editorAction === EditorAction.Create
      ? this.service.create(acoesPreventivas, acoesPreventivas.idNaoConformidade)
      : this.service.update(acoesPreventivas.id, acoesPreventivas.idNaoConformidade, acoesPreventivas);

    this.processando = true;

    saveAction
      .pipe(finalize(() => {
        this.processando = false;
      }))
      .subscribe({
        next: () => {
          this.form.markAsPristine();
          this.dialogRef.close(acoesPreventivas);
        }
      });
  }

  private formInit() {
    this.form = this.formBuilder.group({});
    this.form.addControl(AcoesPreventivasNaoConformidadesFormControl
      .id, this.formBuilder.control(this.data.data?.id || UUID.UUID()));
    this.form.addControl(AcoesPreventivasNaoConformidadesFormControl
      .idAcaoPreventiva, this.formBuilder.control(this.data.data?.idAcaoPreventiva, Validators.required));
    this.form.addControl(AcoesPreventivasNaoConformidadesFormControl
      .implementada, this.formBuilder.control(this.data.data?.implementada || false));
    this.form.addControl(AcoesPreventivasNaoConformidadesFormControl
      .detalhamento, this.formBuilder.control(this.data.data?.detalhamento));
    this.form.addControl(AcoesPreventivasNaoConformidadesFormControl
      .dataAnalise, this.formBuilder.control(this.data.data?.dataAnalise));
    this.form.addControl(AcoesPreventivasNaoConformidadesFormControl
      .dataPrevistaImplantacao, this.formBuilder.control(this.data.data?.dataPrevistaImplantacao));
    this.form.addControl(AcoesPreventivasNaoConformidadesFormControl
      .dataVerificacao, this.formBuilder.control(this.data.data?.dataVerificacao));
    this.form.addControl(AcoesPreventivasNaoConformidadesFormControl
      .novaData, this.formBuilder.control(this.data.data?.novaData));
    this.form.addControl(AcoesPreventivasNaoConformidadesFormControl
      .idAuditor, this.formBuilder.control(this.data.data?.idAuditor));
    this.form.addControl(AcoesPreventivasNaoConformidadesFormControl
      .idResponsavel, this.formBuilder.control(this.data.data?.idResponsavel));
    this.loaded = true;
  }
  private subscribeBloquearAtualizacaoNaoConformidade() {
    this.subscriptionManager.add(
      'bloquearAtualizacaoNaoConformidade',
      this.naoConformidadesEditorService.validarAtualizacaoNaoConformidade.subscribe(() => {
        if (this.naoConformidadesEditorService.somenteLeitura) {
          this.form.disable();
          this.canEditCausaNaoConformidade = false;
        } else {
          if (this.naoConformidadesEditorService.naoConformidadeNaoConcluida) {
            this.form.enable();
            this.canEditCausaNaoConformidade = true;
          } else {
            this.form.disable();
            this.canEditCausaNaoConformidade = false;
          }
        }
      })
    );
  }
}
