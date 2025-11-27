import {
  Component, Inject, OnDestroy, OnInit
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { VsSubscriptionManager } from '@viasoft/common';
import { ConclusoesNaoConformidadesInput }
  from '@viasoft/rnc/api-clients/Nao-Conformidades/Conclusoes-Nao-Conformidades/model/conclusoes-nao-conformidades-input';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { UUID } from 'angular2-uuid';
import { finalize } from 'rxjs/operators';
import { ConclusoesNaoConformidadesFormControl } from './conclusoes-nao-conformidades-form-control';
import { ConclusaoNaoConformidadesService } from '../conclusao-nao-conformidades.service';

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
    dateReuniao: false
  }
  public processando = false;

  constructor(
    private formBuilder: FormBuilder,
    private service: ConclusaoNaoConformidadesService,
    private dialogRef: MatDialogRef<ConclusaoNaoConformidadesEditorModalComponent>,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<ConclusoesNaoConformidadesInput>
  ) {
  }

  public ngOnInit(): void {
    this.idNaoConformidade = this.data.data.idNaoConformidade;
    this.formInit();
    this.novaReuniaoValueChanges();
    this.service.calcularCicloTempo().subscribe((cicloTempo:number) => {
      this.form.get(this.formControls.cicloDeTempo).setValue(cicloTempo);
    });
  }

  public ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public canSave():boolean {
    return !this.processando && this.form.valid && this.form.dirty;
  }

  public save():void {
    if (!this.canSave()) {
      return;
    }
    const conclusao = this.form.getRawValue() as ConclusoesNaoConformidadesInput;
    conclusao.cicloDeTempo = this.form.get(this.formControls.cicloDeTempo).value as number;
    conclusao.idNaoConformidade = this.idNaoConformidade;

    this.processando = true;
    this.service.concluir(conclusao)
      .pipe(finalize(() => {
        this.processando = false;
      }))
      .subscribe(() => {
        this.form.markAsPristine();
        this.dialogRef.close(conclusao);
      });
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
      .dataReuniao, this.formBuilder.control({ value: null, disabled: true }));

    this.loaded = true;
  }
  private novaReuniaoValueChanges() {
    this.subscriptionManager.add('novaReuniaoValueChanges', this.form.get(this.formControls.novaReuniao).valueChanges
      .subscribe((value:boolean) => {
        if (value) {
          const dataEntregaFormControl = this.form.get(this.formControls.dataReuniao);
          dataEntregaFormControl.enable();
          dataEntregaFormControl.addValidators(Validators.required);
          this.camposObrigatorios.dateReuniao = true;
        } else {
          const dataEntregaFormControl = this.form.get(this.formControls.dataReuniao);
          dataEntregaFormControl.disable();
          dataEntregaFormControl.removeValidators(Validators.required);
          this.camposObrigatorios.dateReuniao = false;
        }
      }));
  }
}
