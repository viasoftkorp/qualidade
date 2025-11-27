import {
  AfterViewInit,
  Component, Inject, OnInit, ViewChild
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { VsDialogComponent, VsMaskBase } from '@viasoft/components';
import { UUID } from 'angular2-uuid';
import { DefeitosNaoConformidadesService } from '../defeitos-nao-conformidades.service';
import { DefeitosNaoConformidadesFormControl } from './defeitos-nao-conformidades-form-control';
import { DefeitoService } from './defeito.service';
import {
  DefeitosNaoConformidadesInput,
  DefeitosNaoConformidadesModel
} from '../../../../api-clients/Nao-Conformidades';
import { EditorAction } from '../../../../tokens/classes/editor-action.enum';
import { EditorModalData } from '../../../../tokens/classes/editor-modal-data';
import { getMascaraCampoNumerico, setMaskDecimalPlace } from '../../../../tokens/classes/mask-decimal-place';
import { finalize } from 'rxjs/operators';
import { NaoConformidadesEditorService } from '../../rnc-editor-modal/nao-conformidades-editor.service';

@Component({
  selector: 'rnc-defeitos-nao-conformidades-editor-modal',
  templateUrl: './defeitos-nao-conformidades-editor-modal.component.html',
  styleUrls: ['./defeitos-nao-conformidades-editor-modal.component.scss']
})
export class DefeitosNaoConformidadesEditorModalComponent implements AfterViewInit {
  @ViewChild(VsDialogComponent) private vsDialogComponent: VsDialogComponent;

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
    this.subscribeBloquearAtualizacaoNaoConformidade();
  }

  public get isUpdate():boolean {
    return this.editorAction === EditorAction.Update;
  }

  ngAfterViewInit(): void {
    this.vsDialogComponent.close = () => this.dialogRef.close();
  }

  public completaCadastro(idDefeito:string): void {
    if (idDefeito) {
      this.defeitoService.get(idDefeito).subscribe((data) => {
        this.form.get(DefeitosNaoConformidadesFormControl.detalhamento).setValue(data.detalhamento);
      });
    }
  }

  public canSave() {
    return !this.processando && this.form.valid && this.form.dirty;
  }
  public save() {
    if (!this.canSave()) {
      return;
    }
    const defeito = this.form.getRawValue() as DefeitosNaoConformidadesInput;
    defeito.idNaoConformidade = this.data.data.idNaoConformidade;

    this.processando = true;

    if(this.editorAction === EditorAction.Create) {
      this.service.create(defeito, defeito.idNaoConformidade)
        .pipe(finalize(() => {
          this.processando = false;
        }))
        .subscribe({
          next: () => {
            this.form.markAsPristine();
            this.dialogRef.close(defeito);
          }
        })
    } else {
      this.service.update(defeito.id, defeito.idNaoConformidade, defeito)
        .pipe(finalize(() => {
          this.processando = false;
        }))
        .subscribe({
          next: () => {
            this.form.markAsPristine();
          }
        })
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
        if (this.naoConformidadesEditorService.somenteLeitura) {
          this.form.disable();
          this.canEdit = false;
        } else {
          if (this.naoConformidadesEditorService.naoConformidadeNaoConcluida) {
            this.form.enable();
            this.canEdit = true;
          } else {
            this.form.disable();
            this.canEdit = false;
          }
        }
    });
  }
}
