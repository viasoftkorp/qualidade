import {
  Component, Inject, OnDestroy, OnInit
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CausasNaoConformidadesModel, CausasNaoConformidadesInput } from '@viasoft/rnc/api-clients/Nao-Conformidades';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { UUID } from 'angular2-uuid';
import { IPagedResultOutputDto, VsSubscriptionManager } from '@viasoft/common';
import { CausaService } from '@viasoft/rnc/app/pages/settings/causa/causa.service';
import { CausasNaoConformidadesService } from '../causas-nao-conformidades.service';
import { CausasNaoConformidadesFormControl } from './causas-nao-conformidades-form-control';
import { NaoConformidadesEditorService } from '../../../nao-conformidades-editor.service';
import { finalize } from 'rxjs/operators';
import {
  CentroCustoCausaNaoConformidadeOutput
} from '@viasoft/rnc/api-clients/Nao-Conformidades/Centros-Custo-Causas-Nao-Conformidades/model/centro-custo-causa-nao-conformidade-output';
import {
  CentrosCustoCausasNaoConformidadesProxyService
} from '@viasoft/rnc/api-clients/Nao-Conformidades/Centros-Custo-Causas-Nao-Conformidades/api/centros-custo-causas-nao-conformidades-proxy.service';

@Component({
  selector: 'rnc-causas-nao-conformidade-editor-modal',
  templateUrl: './causas-nao-conformidade-editor-modal.component.html',
  styleUrls: ['./causas-nao-conformidade-editor-modal.component.scss']
})
export class CausasNaoConformidadeEditorModalComponent implements OnInit, OnDestroy {
  private subscriptionManager = new VsSubscriptionManager()
  public form: FormGroup = new FormGroup({});
  public editorAction: EditorAction;
  public formControls = CausasNaoConformidadesFormControl;
  public loaded = false;
  public canEditCausaNaoConformidade = true
  public processando = false;

  constructor(
    private formBuilder: FormBuilder,
    private service: CausasNaoConformidadesService,
    private causaService: CausaService,
    private dialogRef: MatDialogRef<CausasNaoConformidadeEditorModalComponent>,
    private naoConformidadesEditorService:NaoConformidadesEditorService,
    private centrosCustoCausasNaoConformidadesProxyService: CentrosCustoCausasNaoConformidadesProxyService,
    @Inject(MAT_DIALOG_DATA) private data: EditorModalData<CausasNaoConformidadesModel>
  ) {
    this.editorAction = data.action;
    this.formInit();
    this.subscribeBloquearAtualizacaoNaoConformidade();
  }
  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  ngOnInit(): void { // vazio
  }
  public fechar(): void {
    this.dialogRef.close();
  }
  public completaCadastro(idCausa:string): void {
    if (idCausa) {
      this.causaService.get(idCausa).subscribe((data) => {
        this.form.get(CausasNaoConformidadesFormControl.detalhamento).setValue(data.detalhamento);
      });
    }
  }
  public canSave(): boolean {
    return !this.processando && this.form.valid && this.form.dirty && this.canEditCausaNaoConformidade;
  }
  public save(): void {
    if (!this.canSave()) {
      return;
    }
    const causa = this.form.getRawValue() as CausasNaoConformidadesInput;
    causa.idNaoConformidade = this.data.data.idNaoConformidade;
    causa.idDefeitoNaoConformidade = this.data.data.idDefeitoNaoConformidade;
    const saveAction = this.editorAction === EditorAction.Create
      ? this.service.create(causa, causa.idNaoConformidade)
      : this.service.update(causa.id, causa.idNaoConformidade, causa);

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

  private formInit() {
    this.form = this.formBuilder.group({});
    this.form.addControl(CausasNaoConformidadesFormControl
      .id, this.formBuilder.control(this.data.data?.id || UUID.UUID()));
    this.form.addControl(CausasNaoConformidadesFormControl
      .idCausa, this.formBuilder.control(this.data.data?.idCausa, Validators.required));
    this.form.addControl(CausasNaoConformidadesFormControl
      .detalhamento, this.formBuilder.control(this.data.data?.detalhamento));
    this.form.addControl(CausasNaoConformidadesFormControl
      .idsCentrosCustos, this.formBuilder.control([]));

    if (this.data.action === EditorAction.Update) {
      this.setCentrosCustos();
    }

    this.loaded = true;
  }
  private subscribeBloquearAtualizacaoNaoConformidade() {
    this.subscriptionManager.add(
      'bloquearAtualizacaoNaoConformidade',
      this.naoConformidadesEditorService.validarAtualizacaoNaoConformidade.subscribe(() => {
        if (this.naoConformidadesEditorService.naoConformidadeNaoConcluida) {
          this.form.enable();
          this.canEditCausaNaoConformidade = true;
        } else {
          this.form.disable();
          this.canEditCausaNaoConformidade = false;
        }
      })
    );
  }

  private setCentrosCustos() {
    this.centrosCustoCausasNaoConformidadesProxyService.getList(this.data.data.idNaoConformidade, this.data.data.id)
      .subscribe((centrosCustos: IPagedResultOutputDto<CentroCustoCausaNaoConformidadeOutput>) => {
        const idsCentrosCusto = centrosCustos.items.map((centroCusto: CentroCustoCausaNaoConformidadeOutput) => centroCusto.idCentroCusto);
        this.form.get(this.formControls.idsCentrosCustos).setValue(idsCentrosCusto);
      });
  }
}
