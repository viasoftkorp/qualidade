import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
// eslint-disable-next-line max-len
import { OperacaoRetrabalhoInput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/operacao-retrabalho-input';
import { HttpErrorResponse } from '@angular/common/http';
import { MessageService, VsSubscriptionManager } from '@viasoft/common';
// eslint-disable-next-line max-len
import { OperacaoRetrabalhoOutput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/operacao-retrabalho-output';
import { finalize, first } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { OdfConsts } from '@viasoft/rnc/app/tokens/consts/odf-consts';
import { ApontamentoOperacaoOutput } from '@viasoft/rnc/api-clients/Producao/Apontamento/Operacoes/models/apontamento-operacao-output';
import { OperacaoProxyService } from '@viasoft/rnc/api-clients/Producao/Apontamento/Operacoes/api/operacao-proxy.service';
import { VsGridGetInput } from '@viasoft/components';
import { OperacaoSaldoOutput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/operacao-saldo-output';
import {
  NaoConformidadesEditorService
} from '@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/nao-conformidades-editor.service';
import { GerarOperacaoRetrabalhoEditorModalService } from './gerar-operacao-retrabalho-editor-modal.service';
import { GerarOperacaoRetrabalhoFormControls } from './gerar-operacao-retrabalho-form-controls';
import { OperacaoRetrabalhoNaoConformidadesService } from '../operacao-retrabalho-nao-conformidades.service';

@Component({
  selector: 'rnc-gerar-operacao-retrabalho-editor-modal',
  templateUrl: './gerar-operacao-retrabalho-editor-modal.component.html',
  styleUrls: ['./gerar-operacao-retrabalho-editor-modal.component.scss'],
})
export class GerarOperacaoRetrabalhoEditorModalComponent implements OnInit {
  public formControls = GerarOperacaoRetrabalhoFormControls;
  public form: FormGroup;
  public processando = false;
  private subscriptionManager = new VsSubscriptionManager();
  public get canSave(): boolean {
    return this.form.valid && !this.processando;
  }
  constructor(
    private formBuilder: FormBuilder,
    public gerarOperacaoRetrabalhoEditorModalService: GerarOperacaoRetrabalhoEditorModalService,
    private messageService: MessageService,
    private dialogRef: MatDialogRef<GerarOperacaoRetrabalhoEditorModalComponent>,
    private operacaoRetrabalhoNaoConformidadesService: OperacaoRetrabalhoNaoConformidadesService,
    private operacaoProxyService: OperacaoProxyService,
    private naoConformidadesEditorService: NaoConformidadesEditorService,
    @Inject(MAT_DIALOG_DATA) public numeroOdf: number,
  ) {}

  public ngOnInit(): void {
    this.createForm();
    this.configCloseBehavior();
  }

  public fecharModal():void {
    if (this.form.dirty) {
      this.messageService
        .confirm('NaoConformidades.GerarOperacaoRetrabalhoModal.ConfirmarFechar')
        .pipe(first())
        .subscribe((confirmado) => {
          if (confirmado) {
            this.dialogRef.close();
          }
        });
    } else {
      this.dialogRef.close();
    }
  }
  public gerarOperacao(): void {
    if (!this.canSave) {
      return;
    }
    const input = this.form.getRawValue() as OperacaoRetrabalhoInput;
    this.processando = true;

    this.gerarOperacaoRetrabalhoEditorModalService
      .gerarOperacao(input)
      .pipe(
        finalize(() => {
          this.processando = false;
        })
      )
      .subscribe({
        next: () => {
          this.dialogRef.close();
        },
        error: (error: HttpErrorResponse) => {
          if (error.status === 422) {
            const retorno = error.error as OperacaoRetrabalhoOutput;
            this.messageService.warn(retorno.message);
          }
        },
      });
  }

  public async operacaoSelected(operacao: ApontamentoOperacaoOutput): Promise<void> {
    if (!operacao) {
      this.form.get(this.formControls.saldoDisponivel).setValue(null);
      return;
    }
    const operacaoPosterior = await this.encontrarOperacaoPosterior(Number(operacao.operacao));

    if (operacaoPosterior != null) {
      this.operacaoRetrabalhoNaoConformidadesService.getOperacaoSaldo(operacaoPosterior.idOperacao)
        .subscribe((operacaoSaldo: OperacaoSaldoOutput) => {
          this.form.get(this.formControls.saldoDisponivel).setValue(operacaoSaldo.saldo);
        });
    }
  }

  private async encontrarOperacaoPosterior(numeroOperacaoSelecionada: number): Promise<ApontamentoOperacaoOutput | null> {
    const operacoesOdf = await this.operacaoProxyService
      .getList({
        maxResultCount: 99,
        skipCount: 0,
      } as VsGridGetInput, this.numeroOdf).toPromise();

    const operacaoPosterior = operacoesOdf.items
      .find((operacao: ApontamentoOperacaoOutput) => {
        const numeroOperacao = Number(operacao.operacao);

        if (numeroOperacao === numeroOperacaoSelecionada + OdfConsts.DistanciaPadraoEntreOperacoesEngenharia) {
          return operacao;
        }

        if (numeroOperacao === OdfConsts.NumeroOperacaoFinal) {
          return operacao;
        }

        return null;
      });

    return operacaoPosterior;
  }

  private createForm() {
    this.form = this.formBuilder.group({});

    this.form.addControl(
      this.formControls.numeroOperacaoARetrabalhar,
      this.formBuilder.control(null, [Validators.required])
    );

    this.form.addControl(this.formControls.quantidade, this.formBuilder.control(null, [Validators.required]));

    this.form.addControl(this.formControls.saldoDisponivel, this.formBuilder.control({ value: null, disabled: true }));
    this.form.addControl(this.formControls.maquinas, this.formBuilder.array([]));
  }

  private configCloseBehavior(): void {
    this.dialogRef.disableClose = true;
    this.subscriptionManager.add('backdropClick', this.dialogRef.backdropClick().subscribe(() => this.fecharModal()));
    this.subscriptionManager.add('keydownEvents', this.dialogRef.keydownEvents().subscribe((key) => {
      const clickNoEsc = key.code === 'Escape';
      if (clickNoEsc) {
        this.fecharModal();
      }
    }));
  }
}
