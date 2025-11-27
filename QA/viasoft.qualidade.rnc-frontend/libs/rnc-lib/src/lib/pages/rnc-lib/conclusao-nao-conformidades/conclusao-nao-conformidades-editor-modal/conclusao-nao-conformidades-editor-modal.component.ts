import {
  Component, Inject, OnDestroy, OnInit
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { VsSubscriptionManager } from '@viasoft/common';
import { EditorModalData } from '../../../../tokens/classes/editor-modal-data';
import { UUID } from 'angular2-uuid';
import { ConclusoesNaoConformidadesFormControl } from './conclusoes-nao-conformidades-form-control';
import { NaoConformidadesEditorService } from '../../rnc-editor-modal/nao-conformidades-editor.service';
import { ConclusoesNaoConformidadesInput } from '../../../../api-clients/Nao-Conformidades';

@Component({
  selector: 'rnc-conclusao-nao-conformidades-editor-modal',
  templateUrl: './conclusao-nao-conformidades-editor-modal.component.html',
  styleUrls: ['./conclusao-nao-conformidades-editor-modal.component.scss']
})
export class ConclusaoNaoConformidadesEditorModalComponent implements OnInit, OnDestroy {
  public form: FormGroup = new FormGroup({});
  public idNaoConformidade:string;
  public formControls = ConclusoesNaoConformidadesFormControl;
  public loaded = false;
  public subscriptionManager = new VsSubscriptionManager()
  public camposObrigatorios = {
    idAuditor: true,
    dataVerificacao: true,
    dateReuniao: true
  }

  constructor(
    private formBuilder: FormBuilder,
    private service: NaoConformidadesEditorService,
    private dialogRef: MatDialogRef<ConclusaoNaoConformidadesEditorModalComponent>,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<ConclusoesNaoConformidadesInput>
  ) {
  }
  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  ngOnInit(): void {
    this.idNaoConformidade = this.data.data.idNaoConformidade;
    this.formInit();
    this.novaReuniaoValueChanges();
    this.service.calcularCicloTempo(this.idNaoConformidade).subscribe((cicloTempo:number) => {
      this.form.get(this.formControls.cicloDeTempo).setValue(cicloTempo);
    });
  }
  public canSave():boolean {
    return this.form.valid && this.form.dirty;
  }

  public save():void {
    if (this.canSave) {
      if (!this.canSave()) {
        return;
      }
      const conclusao = this.form.getRawValue() as ConclusoesNaoConformidadesInput;
      conclusao.cicloDeTempo = this.form.get(this.formControls.cicloDeTempo).value as number;
      conclusao.idNaoConformidade = this.idNaoConformidade;
      this.service.concluir(conclusao.idNaoConformidade, conclusao).subscribe(() => {
        this.dialogRef.close(conclusao);
      });
    }
  }
  private formInit() {
    this.form = this.formBuilder.group({});
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .id, this.formBuilder.control(UUID.UUID()));
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .idAuditor, this.formBuilder.control(null, [Validators.required]));
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .dataVerificacao, this.formBuilder.control(null, [Validators.required]));
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .eficaz, this.formBuilder.control(false));
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .evidencia, this.formBuilder.control(null));
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .cicloDeTempo, this.formBuilder.control({ value: null, disabled: true }, [Validators.required]));
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .novaReuniao, this.formBuilder.control(false));
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .dataReuniao, this.formBuilder.control({ value: null, disabled: true }, [Validators.required]));

    this.loaded = true;
  }
  private novaReuniaoValueChanges() {
    this.subscriptionManager.add('novaReuniaoValueChanges', this.form.get(this.formControls.novaReuniao).valueChanges
      .subscribe((value:boolean) => {
        if (value) {
          this.form.get(this.formControls.dataReuniao).enable();
        } else {
          this.form.get(this.formControls.dataReuniao).disable();
        }
      }));
  }
}
