import {
  Component,
  Input,
  OnDestroy,
  OnInit
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { VsSubscriptionManager } from '@viasoft/common';
import {
  ReclamacoesNaoConformidadesOutput
} from '../../../api-clients/Nao-Conformidades';
import { UUID } from 'angular2-uuid';
import { ReclamacoesNaoConformidadesFormControl } from './reclamacoes-nao-conformidades-form-control';
import { NaoConformidadesEditorService } from '../rnc-editor-modal/nao-conformidades-editor.service';

@Component({
  selector: 'rnc-reclamacoes-nao-conformidades',
  templateUrl: './reclamacoes-nao-conformidades.component.html',
  styleUrls: ['./reclamacoes-nao-conformidades.component.scss']
})
export class ReclamacoesNaoConformidadesComponent implements OnInit, OnDestroy {
  @Input() public idNaoConformidade:string
  @Input() public naoConformidadesForm:FormGroup
  private subscriptionManager: VsSubscriptionManager = new VsSubscriptionManager();
  public form: FormGroup;
  public formControls = ReclamacoesNaoConformidadesFormControl;
  public loaded = false;

  constructor(
    private formBuilder: FormBuilder,
    private naoConformidadesEditorService: NaoConformidadesEditorService
  ) {
    this.formInit();
    this.subscribeBloquearAtualizacaoNaoConformidade();
  }
  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  ngOnInit(): void {
    this.getReclamacao(this.idNaoConformidade);
    this.subscribeReclamacaoFormValueChanges();
    this.subscribeReclamacaoAtualizada();
  }

  private formInit() {
    this.form = this.formBuilder.group({});
    this.form.addControl(ReclamacoesNaoConformidadesFormControl
      .id, this.formBuilder.control(UUID.UUID()));
    this.form.addControl(ReclamacoesNaoConformidadesFormControl
      .devolucaoFornecedor, this.formBuilder.control(false));
    this.form.addControl(ReclamacoesNaoConformidadesFormControl
      .disposicaoProdutosAprovados, this.formBuilder.control(null, Validators.min(0)));
    this.form.addControl(ReclamacoesNaoConformidadesFormControl
      .disposicaoProdutosConcessao, this.formBuilder.control(null, Validators.min(0)));
    this.form.addControl(ReclamacoesNaoConformidadesFormControl
      .improcedentes, this.formBuilder.control(null, Validators.min(0)));
    this.form.addControl(ReclamacoesNaoConformidadesFormControl
      .procedentes, this.formBuilder.control(null, Validators.min(0)));
    this.form.addControl(ReclamacoesNaoConformidadesFormControl
      .observacao, this.formBuilder.control(null));
    this.form.addControl(ReclamacoesNaoConformidadesFormControl
      .quantidadeLote, this.formBuilder.control(null, Validators.min(0)));
    this.form.addControl(ReclamacoesNaoConformidadesFormControl
      .quantidadeNaoConformidade, this.formBuilder.control(null, Validators.min(0)));
    this.form.addControl(ReclamacoesNaoConformidadesFormControl
      .recodificar, this.formBuilder.control(false));
    this.form.addControl(ReclamacoesNaoConformidadesFormControl
      .retrabalhoComOnus, this.formBuilder.control(false));
    this.form.addControl(ReclamacoesNaoConformidadesFormControl
      .retrabalhoSemOnus, this.formBuilder.control(false));
    this.form.addControl(ReclamacoesNaoConformidadesFormControl
      .sucata, this.formBuilder.control(false));
    this.form.addControl(ReclamacoesNaoConformidadesFormControl
      .rejeitado, this.formBuilder.control(null, Validators.min(0)));
    this.form.addControl(ReclamacoesNaoConformidadesFormControl
      .retrabalho, this.formBuilder.control(null, Validators.min(0)));
    this.naoConformidadesEditorService.setReclamacaoForm(this.form);
    this.loaded = true;

    this.subscriptionManager.add('statusChangeEvent', this.form.statusChanges.subscribe(() => {
      this.naoConformidadesEditorService.setReclamacaoForm(this.form);
    }));
  }

  private populateForm(reclamacao:ReclamacoesNaoConformidadesOutput):void {
    this.form.get(ReclamacoesNaoConformidadesFormControl.id).setValue(reclamacao.id);
    this.form.get(ReclamacoesNaoConformidadesFormControl.devolucaoFornecedor)
      .setValue(reclamacao.devolucaoFornecedor);
    this.form.get(ReclamacoesNaoConformidadesFormControl.disposicaoProdutosAprovados)
      .setValue(reclamacao.disposicaoProdutosAprovados);
    this.form.get(ReclamacoesNaoConformidadesFormControl.disposicaoProdutosConcessao)
      .setValue(reclamacao.disposicaoProdutosConcessao);
    this.form.get(ReclamacoesNaoConformidadesFormControl.improcedentes).setValue(reclamacao.improcedentes);
    this.form.get(ReclamacoesNaoConformidadesFormControl.procedentes).setValue(reclamacao.procedentes);
    this.form.get(ReclamacoesNaoConformidadesFormControl.observacao).setValue(reclamacao.observacao);
    this.form.get(ReclamacoesNaoConformidadesFormControl.quantidadeLote).setValue(reclamacao.quantidadeLote);
    this.form.get(ReclamacoesNaoConformidadesFormControl.quantidadeNaoConformidade)
      .setValue(reclamacao.quantidadeNaoConformidade);
    this.form.get(ReclamacoesNaoConformidadesFormControl.recodificar).setValue(reclamacao.recodificar);
    this.form.get(ReclamacoesNaoConformidadesFormControl.retrabalhoComOnus).setValue(reclamacao.retrabalhoComOnus);
    this.form.get(ReclamacoesNaoConformidadesFormControl.retrabalhoSemOnus).setValue(reclamacao.retrabalhoSemOnus);
    this.form.get(ReclamacoesNaoConformidadesFormControl.sucata).setValue(reclamacao.sucata);
    this.form.get(ReclamacoesNaoConformidadesFormControl.rejeitado).setValue(reclamacao.rejeitado);
    this.form.get(ReclamacoesNaoConformidadesFormControl.retrabalho).setValue(reclamacao.retrabalho);
  }

  private getReclamacao(idNaoConformidade:string) {
    return this.naoConformidadesEditorService.getReclamacao(idNaoConformidade).subscribe((reclamacao: ReclamacoesNaoConformidadesOutput) => {
      if (reclamacao) {
        this.populateForm(reclamacao);
      }
    });
  }
  private subscribeReclamacaoFormValueChanges() {
    this.subscriptionManager.add('reclamacaoFormValueChanges', this.subscriptionManager
      .add('valueChangeEvent', this.form.valueChanges
        .subscribe(() => {
          this.naoConformidadesEditorService.setReclamacaoForm(this.form);
          this.naoConformidadesForm.markAsDirty();
        })));
  }

  private subscribeBloquearAtualizacaoNaoConformidade() {
    this.subscriptionManager.add(
      'bloquearAtualizacaoNaoConformidade',
      this.naoConformidadesEditorService.validarAtualizacaoNaoConformidade.subscribe(() => {
        if (this.naoConformidadesEditorService.somenteLeitura) {
          this.form.disable();
        } else {
          if (this.naoConformidadesEditorService.naoConformidadeNaoConcluida) {
            this.form.enable();
          } else {
            this.form.disable();
          }
        }
      })
    );
  }

  private subscribeReclamacaoAtualizada(): void {
    this.subscriptionManager.add('reclamacaoAtualizada',
      this.naoConformidadesEditorService.reclamacaoAtualizada
        .subscribe((idNaoConformidade) => {
          this.getReclamacao(idNaoConformidade);
        })
    );
  }
}
