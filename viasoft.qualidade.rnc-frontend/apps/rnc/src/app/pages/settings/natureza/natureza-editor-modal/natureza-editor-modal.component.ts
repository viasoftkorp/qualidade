import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { UUID } from 'angular2-uuid';
import { NaturezaInput, NaturezaOutput } from '@viasoft/rnc/api-clients/Naturezas';
import { finalize } from 'rxjs/operators';
import { EditorAction } from '../../../../tokens/consts/editor-action.enum';
import { NaturezaService } from '../natureza.service';
import { NaturezaFormControls } from './natureza-form-controls';

@Component({
  selector: 'rnc-natureza-editor-modal',
  templateUrl: './natureza-editor-modal.component.html',
  styleUrls: ['./natureza-editor-modal.component.scss']
})
export class NaturezaEditorModalComponent implements OnInit {
  public form: FormGroup = new FormGroup({});
  public editorAction: EditorAction;
  public formControls = NaturezaFormControls;
  public loaded: boolean;
  public processando = false;

  constructor(
    private formBuilder: FormBuilder,
    private service: NaturezaService,
    private dialogRef: MatDialogRef<NaturezaEditorModalComponent>,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<NaturezaOutput>
  ) {
    this.editorAction = data.action;

    this.formInit();
  }
  public async ngOnInit(): Promise<void> {
    if (this.editorAction === EditorAction.Create) {
      this.populateForm({
        id: UUID.UUID(),
      } as NaturezaOutput);
      this.loaded = true;
    } else {
      const natureza = await this.getNatureza(this.data.data.id);
      this.populateForm(natureza);
      this.loaded = true;
    }
  }

  public canSave():boolean {
    return !this.processando && this.form.valid && this.form.dirty;
  }

  public save():void {
    if (!this.canSave()) {
      return;
    }
    const natureza : NaturezaInput = {
      id: this.form.get(NaturezaFormControls.id).value as string,
      descricao: this.form.get(NaturezaFormControls.descricao).value as string,
    };

    const saveAction = this.editorAction === EditorAction.Create
      ? this.service.create(natureza) : this.service.update(natureza.id, natureza);

    this.processando = true;
    saveAction
      .pipe(finalize(() => {
        this.processando = false;
      }))
      .subscribe({
        next: () => {
          this.form.markAsPristine();
          this.dialogRef.close(natureza);
        }
      });
  }

  private populateForm(natureza: NaturezaOutput) {
    this.form.get(NaturezaFormControls.id).setValue(natureza.id);
    this.form.get(NaturezaFormControls.descricao).setValue(natureza.descricao);
  }

  private formInit() {
    this.form = this.formBuilder.group({});
    this.form.addControl(NaturezaFormControls.id, this.formBuilder.control(null, [Validators.required]));
    this.form.addControl(NaturezaFormControls.descricao,
      this.formBuilder.control(null, [Validators.required, Validators.maxLength(450)]));
  }

  private getNatureza(id: string):Promise<NaturezaOutput> {
    return this.service.get(id).toPromise();
  }
}
