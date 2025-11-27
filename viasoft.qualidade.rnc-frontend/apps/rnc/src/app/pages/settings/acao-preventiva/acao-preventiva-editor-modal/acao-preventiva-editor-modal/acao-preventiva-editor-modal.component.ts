import { UUID } from 'angular2-uuid';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { AcaoPreventivaInput, AcaoPreventivaOutput } from '@viasoft/rnc/api-clients/Acoes-Preventivas';
import { finalize } from 'rxjs/operators';
import { AcaoPreventivaFormControls } from './acao-preventiva-form-controls';
import { AcaoPreventivaService } from '../../services/acao-preventiva.service';

@Component({
  selector: 'rnc-acao-preventiva-editor-modal',
  templateUrl: './acao-preventiva-editor-modal.component.html',
  styleUrls: ['./acao-preventiva-editor-modal.component.scss']
})
export class AcaoPreventivaEditorModalComponent implements OnInit {
  public form: FormGroup = new FormGroup({})
  public formControls = AcaoPreventivaFormControls;
  public editorAction:EditorAction;
  public loaded: boolean;
  public acaoPreventiva: AcaoPreventivaOutput;
  public processando = false;

  constructor(
    private formBuilder:FormBuilder,
    private dialogRef:MatDialogRef<AcaoPreventivaEditorModalComponent>,
    private acaoPreventivaService:AcaoPreventivaService,

    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<AcaoPreventivaOutput>
  ) {
    this.editorAction = data.action;
    this.initForm();
  }
  public async ngOnInit(): Promise<void> {
    if (this.editorAction === EditorAction.Create) {
      this.populateForm({
        id: UUID.UUID()
      }as AcaoPreventivaOutput);
    } else {
      const acaoPreventiva = await this.getAcaoPreventiva(this.data.data.id);
      this.populateForm(acaoPreventiva);
    }
    this.loaded = true;
  }

  public canSave(): boolean {
    return !this.processando && this.form.valid && this.form.dirty;
  }

  public save():void {
    if (!this.canSave()) {
      return;
    }
    const acaoPreventiva : AcaoPreventivaInput = {
      id: this.form.get(AcaoPreventivaFormControls.id).value as string,
      descricao: this.form.get(AcaoPreventivaFormControls.descricao).value as string,
      detalhamento: this.form.get(AcaoPreventivaFormControls.detalhamento).value as string,
      idResponsavel: this.form.get(AcaoPreventivaFormControls.idResponsavel).value as string,
    };

    const saveAction = this.editorAction === EditorAction.Create
      ? this.acaoPreventivaService.create(acaoPreventiva)
      : this.acaoPreventivaService.update(acaoPreventiva.id, acaoPreventiva);

    this.processando = true;

    saveAction
      .pipe(finalize(() => {
        this.processando = false;
      }))
      .subscribe({
        next: () => {
          this.form.markAsPristine();
          this.dialogRef.close(acaoPreventiva);
        }
      });
  }

  private getAcaoPreventiva(id: string):Promise<AcaoPreventivaOutput> {
    return this.acaoPreventivaService.get(id).toPromise();
  }

  private populateForm(acaoPreventiva:AcaoPreventivaOutput) {
    this.acaoPreventiva = acaoPreventiva;

    this.form.get(AcaoPreventivaFormControls.id).setValue(acaoPreventiva.id);
    this.form.get(AcaoPreventivaFormControls.descricao).setValue(acaoPreventiva.descricao);
    this.form.get(AcaoPreventivaFormControls.detalhamento).setValue(acaoPreventiva.detalhamento);
    this.form.get(AcaoPreventivaFormControls.idResponsavel).setValue(acaoPreventiva.idResponsavel);
    this.form.get(AcaoPreventivaFormControls.nomeResponsavel).setValue(acaoPreventiva.idResponsavel);
  }

  private initForm() {
    this.form = this.formBuilder.group({});

    this.form.addControl(AcaoPreventivaFormControls.id, this.formBuilder.control(null, [Validators.required]));

    this.form.addControl(AcaoPreventivaFormControls.descricao,
      this.formBuilder.control(null, [Validators.required, Validators.maxLength(450)]));

    this.form.addControl(AcaoPreventivaFormControls.detalhamento, this.formBuilder.control(null));

    this.form.addControl(AcaoPreventivaFormControls.nomeResponsavel, this.formBuilder.control(null));

    this.form.addControl(AcaoPreventivaFormControls.idResponsavel, this.formBuilder.control(null));
  }
}
