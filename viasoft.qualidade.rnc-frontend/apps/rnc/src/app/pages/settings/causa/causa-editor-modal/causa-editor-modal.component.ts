import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { UUID } from 'angular2-uuid';
import { finalize } from 'rxjs/operators';
import { CausaInput } from '@viasoft/rnc/api-clients/Causas/model/causa-input';
import { CausaOutput } from '../../../../../api-clients/Causas/model/causa-output';
import { EditorAction } from '../../../../tokens/consts/editor-action.enum';
import { CausaService } from '../causa.service';
import { CausaFormControls } from './causa-form-controls';

@Component({
  selector: 'rnc-causa-editor-modal',
  templateUrl: './causa-editor-modal.component.html',
  styleUrls: ['./causa-editor-modal.component.scss']
})
export class CausaEditorModalComponent implements OnInit {
  public form: FormGroup = new FormGroup({});
  public editorAction: EditorAction;
  public formControls = CausaFormControls;
  public loaded: boolean;
  public processando = false;

  constructor(
    private formBuilder: FormBuilder,
    private service: CausaService,
    private dialogRef: MatDialogRef<CausaEditorModalComponent>,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<CausaOutput>
  ) {
    this.editorAction = data.action;

    this.createForm();
  }
  public async ngOnInit(): Promise<void> {
    if (this.editorAction === EditorAction.Create) {
      this.populateForm({
        id: UUID.UUID(),
      } as CausaOutput);
    } else {
      const causa = await this.getCausa(this.data.data.id);
      this.populateForm(causa);
    }
    this.loaded = true;
  }

  public canSave():boolean {
    return !this.processando && this.form.valid && this.form.dirty;
  }
  public save():void {
    if (!this.canSave()) {
      return;
    }
    const causa : CausaInput = {
      id: this.form.get(CausaFormControls.id).value as string,
      descricao: this.form.get(CausaFormControls.descricao).value as string,
      detalhamento: this.form.get(CausaFormControls.detalhamento).value as string
    };

    const saveAction = this.editorAction === EditorAction.Create
      ? this.service.create(causa) : this.service.update(causa.id, causa);

    this.processando = true;

    saveAction
      .pipe(finalize(() => {
        this.processando = false;
      }))
      .subscribe({
        next: () => {
          this.form.markAsPristine();
          this.dialogRef.close(causa);
        }
      });
  }

  private populateForm(causa: CausaOutput) {
    this.form.get(CausaFormControls.id).setValue(causa.id);
    this.form.get(CausaFormControls.descricao).setValue(causa.descricao);
    this.form.get(CausaFormControls.detalhamento).setValue(causa.detalhamento);
  }

  private createForm() {
    this.form = this.formBuilder.group({});
    this.form.addControl(CausaFormControls.id, this.formBuilder.control(null, [Validators.required]));
    this.form.addControl(CausaFormControls.descricao,
      this.formBuilder.control(null, [Validators.required, Validators.maxLength(450)]));
    this.form.addControl(CausaFormControls.detalhamento, this.formBuilder.control(null));
  }

  private getCausa(id: string): Promise<CausaOutput> {
    return this.service.get(id).toPromise();
  }
}
