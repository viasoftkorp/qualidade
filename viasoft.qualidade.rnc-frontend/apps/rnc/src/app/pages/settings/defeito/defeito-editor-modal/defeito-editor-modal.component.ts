import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DefeitoOutput, DefeitoInput } from '@viasoft/rnc/api-clients/Defeitos';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { UUID } from 'angular2-uuid';
import { finalize } from 'rxjs/operators';
import { DefeitoService } from '../defeito.service';
import { DefeitoFormControls } from './defeito-form-controls';

@Component({
  selector: 'rnc-defeito-editor-modal',
  templateUrl: './defeito-editor-modal.component.html',
  styleUrls: ['./defeito-editor-modal.component.scss']
})
export class DefeitoEditorModalComponent implements OnInit {
  public form: FormGroup = new FormGroup({});
  public editorAction: EditorAction;
  public formControls = DefeitoFormControls;
  public loaded = false;
  public processando = false;

  constructor(
    private formBuilder: FormBuilder,
    private service: DefeitoService,
    private dialogRef: MatDialogRef<DefeitoEditorModalComponent>,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<DefeitoOutput>
  ) {
    this.editorAction = data.action;
    this.createForm();
  }

  public async ngOnInit(): Promise<void> {
    if (this.editorAction === EditorAction.Create) {
      this.populateForm({
        id: UUID.UUID(),
      } as DefeitoOutput);
      this.loaded = true;
    } else {
      const defeito = await this.getDefeito(this.data.data.id);
      this.populateForm(defeito);
    }
    this.loaded = true;
  }
  public canSave() : boolean {
    return !this.processando && this.form.valid && this.form.dirty;
  }
  public save():void {
    if (!this.canSave()) {
      return;
    }
    const defeito = this.form.getRawValue() as DefeitoInput;

    const saveAction = this.editorAction === EditorAction.Create
      ? this.service.create(defeito) : this.service.update(defeito.id, defeito);

    this.processando = true;
    saveAction
      .pipe(finalize(() => {
        this.processando = false;
      }))
      .subscribe({
        next: () => {
          this.form.markAsPristine();
          this.dialogRef.close(defeito);
        }
      });
  }

  private createForm() {
    this.form = this.formBuilder.group({});

    this.form.addControl(DefeitoFormControls.id, this.formBuilder.control(null, [Validators.required]));
    this.form.addControl(DefeitoFormControls.idCausa, this.formBuilder.control(null));
    this.form.addControl(DefeitoFormControls.idSolucao, this.formBuilder.control(null));
    this.form.addControl(DefeitoFormControls.descricao, this.formBuilder.control(null, [Validators.required]));
    this.form.addControl(DefeitoFormControls.detalhamento, this.formBuilder.control(null));
  }

  private populateForm(defeito: DefeitoOutput) {
    this.form.get(DefeitoFormControls.id).setValue(defeito.id);
    this.form.get(DefeitoFormControls.idCausa).setValue(defeito.idCausa);
    this.form.get(DefeitoFormControls.idSolucao).setValue(defeito.idSolucao);
    this.form.get(DefeitoFormControls.descricao).setValue(defeito.descricao);
    this.form.get(DefeitoFormControls.detalhamento).setValue(defeito.detalhamento);
  }

  private getDefeito(id: string):Promise<DefeitoOutput> {
    return this.service.get(id).toPromise();
  }
}
