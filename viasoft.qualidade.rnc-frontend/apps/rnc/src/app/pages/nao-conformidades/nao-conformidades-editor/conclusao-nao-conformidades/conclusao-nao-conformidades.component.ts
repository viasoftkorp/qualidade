import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { VsSubscriptionManager } from '@viasoft/common';
import { ConclusoesNaoConformidadesOutput } from '@viasoft/rnc/api-clients/Nao-Conformidades';
import { UUID } from 'angular2-uuid';
import { ConclusoesNaoConformidadesFormControl }
  from './conclusao-nao-conformidades-editor-modal/conclusoes-nao-conformidades-form-control';
import { NaoConformidadesEditorService } from '../nao-conformidades-editor.service';
import { ConclusaoNaoConformidadesService } from './conclusao-nao-conformidades.service';

@Component({
  selector: 'rnc-conclusao-nao-conformidades',
  templateUrl: './conclusao-nao-conformidades.component.html',
  styleUrls: ['./conclusao-nao-conformidades.component.scss']
})
export class ConclusaoNaoConformidadesComponent implements OnInit {
  @Input() public idNaoConformidade:string
  public form: FormGroup = new FormGroup({});
  public formControls = ConclusoesNaoConformidadesFormControl;
  public loaded = false;
  private subscriptionManager = new VsSubscriptionManager();

  constructor(
    private formBuilder: FormBuilder,
    private service: ConclusaoNaoConformidadesService,
    private naoConformidadesEditorService: NaoConformidadesEditorService
  ) {}

  public ngOnInit(): void {
    this.subscribeNaoConformidadeConcluida();
    this.getConclusao();
  }

  public ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  private formInit(conclusao?: ConclusoesNaoConformidadesOutput) {
    this.form = this.formBuilder.group({});
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .id, this.formBuilder.control(conclusao?.id || UUID.UUID()));
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .idAuditor, this.formBuilder.control({ value: conclusao?.idAuditor, disabled: true }));
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .dataReuniao, this.formBuilder.control({ value: conclusao?.dataReuniao, disabled: true }));
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .dataVerificacao, this.formBuilder.control({ value: conclusao?.dataVerificacao, disabled: true }));
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .eficaz, this.formBuilder.control({ value: conclusao?.eficaz, disabled: true }));
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .evidencia, this.formBuilder.control({ value: conclusao?.evidencia, disabled: true }));
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .novaReuniao, this.formBuilder.control({ value: conclusao?.novaReuniao, disabled: true }));
    this.form.addControl(ConclusoesNaoConformidadesFormControl
      .cicloDeTempo, this.formBuilder.control({ value: conclusao?.cicloDeTempo, disabled: true }));
    this.loaded = true;
  }

  private getConclusao() {
    return this.service.getConclusao().subscribe((conclusao: ConclusoesNaoConformidadesOutput) => {
      this.formInit(conclusao);
    });
  }
  private subscribeNaoConformidadeConcluida() {
    this.subscriptionManager.add('naoConformidadeConcluida',
      this.naoConformidadesEditorService.naoConformidadeConcluida.subscribe(() => {
        this.getConclusao();
      }));
  }
}
