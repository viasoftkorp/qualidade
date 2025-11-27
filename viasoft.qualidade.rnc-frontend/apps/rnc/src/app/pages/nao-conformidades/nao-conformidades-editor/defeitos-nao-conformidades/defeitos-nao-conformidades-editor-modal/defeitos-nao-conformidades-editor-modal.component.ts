import {
  Component, Inject, OnInit
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DefeitosNaoConformidadesModel, DefeitosNaoConformidadesInput } from '@viasoft/rnc/api-clients/Nao-Conformidades';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { getMascaraCampoNumerico } from '@viasoft/rnc/app/tokens/consts/mask-decimal-place';
import { UUID } from 'angular2-uuid';
import { finalize } from 'rxjs/operators';
import {
  NaoConformidadesEditorService
} from '@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/nao-conformidades-editor.service';
import { DefeitoService } from '../../../../settings/defeito/defeito.service';
import { DefeitosNaoConformidadesService } from '../defeitos-nao-conformidades.service';
import { DefeitosNaoConformidadesFormControl } from './defeitos-nao-conformidades-form-control';

@Component({
  selector: 'rnc-defeitos-nao-conformidades-editor-modal',
  templateUrl: './defeitos-nao-conformidades-editor-modal.component.html',
  styleUrls: ['./defeitos-nao-conformidades-editor-modal.component.scss']
})
export class DefeitosNaoConformidadesEditorModalComponent implements OnInit {
  private idCausa:string;
  private idSolucao:string;
  public form: FormGroup = new FormGroup({});
  public editorAction: EditorAction;
  public formControls = DefeitosNaoConformidadesFormControl;
  public loaded = false;
  public defeito: DefeitosNaoConformidadesModel
  public processando = false;
  public canEdit = true;
  public mascaraCampoNumerico = (numeroCasasDecimais: number) => getMascaraCampoNumerico(numeroCasasDecimais)
  constructor(
    private formBuilder: FormBuilder,
    private service: DefeitosNaoConformidadesService,
    private defeitoService: DefeitoService,
    private naoConformidadesEditorService: NaoConformidadesEditorService,
    private dialogRef: MatDialogRef<DefeitosNaoConformidadesEditorModalComponent>,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<DefeitosNaoConformidadesModel>
  ) {
    this.editorAction = data.action;
    this.defeito = data.data;
    this.formInit();
  }

  public get isUpdate():boolean {
    return this.editorAction === EditorAction.Update;
  }

  ngOnInit(): void {
    this.subscribeBloquearAtualizacaoNaoConformidade();
  }

  public completaCadastro(idDefeito:string): void {
    if (idDefeito) {
      this.defeitoService.get(idDefeito).subscribe((data) => {
        this.form.get(DefeitosNaoConformidadesFormControl.detalhamento).setValue(data.detalhamento);
      });
    }
  }

  public canSave(): boolean {
    return !this.processando && this.form.valid && this.form.dirty && this.canEdit;
  }
  public save(): void {
    if (!this.canSave()) {
      return;
    }
    const defeito = this.form.getRawValue() as DefeitosNaoConformidadesInput;
    defeito.idNaoConformidade = this.data.data.idNaoConformidade;

    this.processando = true;

    if (this.editorAction === EditorAction.Create) {
      this.service.create(defeito, defeito.idNaoConformidade)
        .pipe(finalize(() => {
          this.processando = false;
        }))
        .subscribe({
          next: () => {
            this.service.atualizarGridDefeitos.next();
            this.dialogRef.close();
            this.form.markAsPristine();
          }
        });
    } else {
      this.service.update(defeito.id, defeito.idNaoConformidade, defeito)
        .pipe(finalize(() => {
          this.processando = false;
        }))
        .subscribe({
          next: () => {
            this.service.atualizarGridDefeitos.next();
            this.form.markAsPristine();
          }
        });
    }
  }

  private formInit() {
    this.form = this.formBuilder.group({});
    this.form.addControl(DefeitosNaoConformidadesFormControl
      .id, this.formBuilder.control(this.data.data?.id || UUID.UUID()));
    this.form.addControl(DefeitosNaoConformidadesFormControl
      .idDefeito, this.formBuilder.control(this.data.data?.idDefeito, Validators.required));
    this.form.addControl(DefeitosNaoConformidadesFormControl
      .quantidade, this.formBuilder.control(this.data.data?.quantidade, [Validators.required, Validators.min(0)]));
    this.form.addControl(DefeitosNaoConformidadesFormControl
      .detalhamento, this.formBuilder.control(this.data.data?.detalhamento));
    this.loaded = true;
  }

  private subscribeBloquearAtualizacaoNaoConformidade() {
    this.naoConformidadesEditorService.validarAtualizacaoNaoConformidade.subscribe(() => {
      if (this.naoConformidadesEditorService.naoConformidadeNaoConcluida) {
        this.form.enable();
        this.canEdit = true;
      } else {
        this.form.disable();
        this.canEdit = false;
      }
    });
  }
}
