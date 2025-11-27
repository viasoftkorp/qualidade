import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
// eslint-disable-next-line max-len
import { StatusProducaoRetrabalho } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/status-producao-retrabalho';
import { VsCommandRunnerService } from '@viasoft/common';
import {
  NaoConformidadesEditorService
} from '@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/nao-conformidades-editor.service';
import { OdfRetrabalhoNaoConformidadeFormControls } from './odf-retrabalho-nao-conformidades-form-controls';

@Component({
  selector: 'rnc-odf-retrabalho-nao-conformidades',
  templateUrl: './odf-retrabalho-nao-conformidades.component.html',
  styleUrls: ['./odf-retrabalho-nao-conformidades.component.scss']
})
export class OdfRetrabalhoNaoConformidadesComponent implements OnInit, OnDestroy {
  public form: FormGroup
  public formControls = OdfRetrabalhoNaoConformidadeFormControls
  public loaded = false;
  private readonly BUSCAR_STATUS_ORDEM_RETRABALHO = 'Viasoft.Qualidade.RNC.Core.BuscarStatusOrdemRetrabalho';

  constructor(private formBuilder: FormBuilder,
    private naoConformidadesEditorService: NaoConformidadesEditorService) { }

  public ngOnInit(): void {
    this.createForm();
    this.populateForm();
    this.adicionarComandos();
  }

  public ngOnDestroy(): void {
    VsCommandRunnerService.removeCommand(this.BUSCAR_STATUS_ORDEM_RETRABALHO);
  }

  public getCorStatusClass(): string {
    const inputInformativeClass = 'input-informative';
    const inputPositiveClass = 'input-positive';
    const inputNegativeClass = 'input-negative';
    const inputNoticeClass = 'input-notice';

    const currentStatus = this.form.get(this.formControls.status).value as string;

    switch (currentStatus) {
    case StatusProducaoRetrabalho[StatusProducaoRetrabalho.Aberta]:
      return inputInformativeClass;
    case StatusProducaoRetrabalho[StatusProducaoRetrabalho.Cancelada]:
      return inputNegativeClass;
    case StatusProducaoRetrabalho[StatusProducaoRetrabalho.Encerrada]:
      return inputPositiveClass;
    case StatusProducaoRetrabalho[StatusProducaoRetrabalho.Produzindo]:
      return inputNoticeClass;
    default:
      return inputInformativeClass;
    }
  }

  private createForm() {
    this.form = this.formBuilder.group({});

    this.form.addControl(this.formControls.numeroOdfRetrabalho, this.formBuilder.control(null));
    this.form.addControl(this.formControls.quantidade, this.formBuilder.control(null));
    this.form.addControl(this.formControls.status, this.formBuilder.control(null));
  }

  private populateForm() {
    const ordemRetrabalho = { ...this.naoConformidadesEditorService.ordemRetrabalho };

    this.form.get(this.formControls.numeroOdfRetrabalho)
      .setValue(ordemRetrabalho.numeroOdfRetrabalho, { emitEvent: false });

    this.form.get(this.formControls.quantidade)
      .setValue(ordemRetrabalho.quantidade, { emitEvent: false });

    this.form.get(this.formControls.status).setValue(StatusProducaoRetrabalho[ordemRetrabalho.status], { emitEvent: false });
    this.loaded = true;
  }

  private adicionarComandos(): void {
    VsCommandRunnerService.addCommand(this.BUSCAR_STATUS_ORDEM_RETRABALHO, (numeroOdf: number) => {
      const numeroOdfRetrabalho = Number(this.form.get(this.formControls.numeroOdfRetrabalho).value);

      if (numeroOdf === numeroOdfRetrabalho) {
        this.naoConformidadesEditorService.getOdfRetrabalho().subscribe(() => {
          this.populateForm();
        });
      }

      return Promise.resolve();
    });
  }
}
