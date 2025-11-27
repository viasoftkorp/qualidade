import { Component, OnDestroy, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTabChangeEvent } from '@angular/material/tabs';

import { VsSelectOption } from '@viasoft/components';

import { Policy } from '../../../tokens/classes/policy.class';
import { NaoConformidadesFormControl } from '../../../tokens/classes/nao-conformidades-form-control';
import {
  DefeitosNaoConformidadesModel,
  NaoConformidadeAgregacaoOutput,
  NaoConformidadeModel
} from '../../../api-clients/Nao-Conformidades';
import { OrigemNaoConformidades } from '../../../api-clients/Nao-Conformidades/model/origem-nao-conformidades';
import { StatusNaoConformidade } from '../../../api-clients/Nao-Conformidades/model/status-nao-conformidades';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MessageService, UserService, VsSubscriptionManager } from '@viasoft/common';
import {
  NaoConformidadesEditorService
} from './nao-conformidades-editor.service';
import { UUID } from 'angular2-uuid';
import { NaoConformidadesService } from './nao-conformidades.service';
import { NaoConformidadeInput } from '../../../api-clients/Nao-Conformidades';
import { ReclamacoesNaoConformidadesInput } from '../../../api-clients/Nao-Conformidades/Reclamacoes-Nao-Conformidades/model';
import { finalize, switchMap, tap } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { OrdemProducaoService } from '../../../components/ordens-producao/ordem-producao.service';
import { OrdemProducaoOutput } from '../../../api-clients/Ordem-Producao/model';
import { forkJoin, Observable, of } from 'rxjs';

@Component({
  selector: 'rnc-editor-modal',
  templateUrl: './rnc-editor-modal.component.html',
  styleUrls: ['./rnc-editor-modal.component.scss']
})
export class RncEditorModalComponent implements OnDestroy, OnInit {
  public readonly policy = Policy;
  public form: FormGroup;
  public formControls = NaoConformidadesFormControl;
  public loaded: boolean;
  public showDefeitosGrid = true;
  public isUpdating: boolean;
  public defeito: DefeitosNaoConformidadesModel;
  public showConcluir = true;
  public canEditNaoConformidade = true;
  public origemNaoConformidadesEnum = OrigemNaoConformidades;
  public origem: OrigemNaoConformidades;
  public idPessoa:string;
  public processando = false;
  public buscandoInformacoesOdf = false;
  public saveScheduladoBuscandoInformacoesOdf = false;
  private numeroOdf: number;
  private subscriptionManager: VsSubscriptionManager = new VsSubscriptionManager();

  public get hasFornecedor():boolean {
    return !!(this.form.get(this.formControls.idPessoa).value as string);
  }

  public origemOptions: VsSelectOption[] = [
    {
      name: `NaoConformidades.OrigensOptions.${OrigemNaoConformidades.Cliente}`,
      value: OrigemNaoConformidades.Cliente
    },
    {
      name: `NaoConformidades.OrigensOptions.${OrigemNaoConformidades.Interno}`,
      value: OrigemNaoConformidades.Interno
    },
    {
      name: `NaoConformidades.OrigensOptions.${OrigemNaoConformidades.InspecaoEntrada}`,
      value: OrigemNaoConformidades.InspecaoEntrada,
    },
    {
      name: `NaoConformidades.OrigensOptions.${OrigemNaoConformidades.InspecaoSaida}`,
      value: OrigemNaoConformidades.InspecaoSaida,
    },
  ];

  public statusOptions: VsSelectOption[] = [
    { name: StatusNaoConformidade[StatusNaoConformidade.Aberto], value: StatusNaoConformidade.Aberto },
    { name: StatusNaoConformidade[StatusNaoConformidade.Pendente], value: StatusNaoConformidade.Pendente },
    { name: StatusNaoConformidade[StatusNaoConformidade.Fechado], value: StatusNaoConformidade.Fechado },
  ];

  public canSave(): boolean {
    return this.form.valid
      && this.naoConformidadesEditorService.getReclamacaoForm.valid
      && this.canEditNaoConformidade
      && !this.processando;
  }

  constructor(
    @Inject(MAT_DIALOG_DATA) public idNaoConformidade: string,
    private formBuilder: FormBuilder,
    public naoConformidadesEditorService: NaoConformidadesEditorService,
    private userService: UserService,
    private naoConformidadesService: NaoConformidadesService,
    private dialogRef: MatDialogRef<RncEditorModalComponent>,
    private ordemProducaoService: OrdemProducaoService,
    private messageService: MessageService
  ) {
    this.setForm();
    this.formActionCheck();
    this.subscribeBloquearAtualizacaoNaoConformidade();
    this.initComponent();
  }
  ngOnInit(): void {
    this.subscribeOrigemValueChanges();
  }

  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public tabChange(tab: MatTabChangeEvent): void {
    this.showDefeitosGrid = tab.index === 0;
  }

  public defeitoNaoConformidadeSelecionado(defeito: DefeitosNaoConformidadesModel): void {
    this.defeito = defeito;
  }

  public salvarNaoConformidade() {
    this.save().subscribe();
  }
  public save(): Observable<NaoConformidadeAgregacaoOutput> {
    this.naoConformidadesEditorService.saveButtonClicked.next();
    if (!this.canSave()) {
      return of(null) as Observable<null>;
    }

    if (this.buscandoInformacoesOdf) {
      this.saveScheduladoBuscandoInformacoesOdf = true;
      return of(null) as Observable<null>;
    }
    this.processando = true;
    const naoConformidade = this.form.getRawValue() as NaoConformidadeInput;

    return this.naoConformidadesService.update(naoConformidade.id, naoConformidade)
      .pipe(
        switchMap(() => this.naoConformidadesService.getAgregacao(naoConformidade.id)),
        tap({
          next: (model: NaoConformidadeAgregacaoOutput) => {
            const canUpdateReclamacao = this.naoConformidadesEditorService.getReclamacaoForm.valid
              && this.naoConformidadesEditorService.getReclamacaoForm.dirty;

            if (canUpdateReclamacao) {
              const reclamacao = new ReclamacoesNaoConformidadesInput(
                naoConformidade.id, this.naoConformidadesEditorService.getReclamacaoForm.getRawValue());

              this.naoConformidadesEditorService.updateReclamacao(reclamacao.idNaoConformidade, reclamacao)
                .subscribe(() => {
                  this.naoConformidadesEditorService.reclamacaoAtualizada.next(naoConformidade.id);
                });
            }

            this.dialogRef.close(model);
          },
          error: (error:HttpErrorResponse) => {
            if (error.status === 422) {
              this.messageService
                .error(`NaoConformidades.NaoConformidadesEditor.RNCInvalida.${error.error}`,
                  'NaoConformidades.NaoConformidadesEditor.RNCInvalida.Title');
            }
          }
        })
      )
  }

  public pessoaAlterada(pessoaSelecionada: { name:string, value:string }):void {
    if (pessoaSelecionada) {
      this.idPessoa = pessoaSelecionada.value;
    } else {
      this.idPessoa = null;
    }
    this.limparNotaFiscal();
  }

  public odfPreenchido():void {
    const numeroOdf = this.form.get(this.formControls.numeroOdf).value as number;

    if (!numeroOdf || numeroOdf === this.numeroOdf) {
      return;
    }
    this.numeroOdf = numeroOdf;
    this.buscandoInformacoesOdf = true;
    this.ordemProducaoService.getByNumeroOdf(numeroOdf)
      .pipe(finalize(() => {
        this.buscandoInformacoesOdf = false;
      }))
      .subscribe((ordemProducao: OrdemProducaoOutput) => {
        this.buscandoInformacoesOdf = false;
        if (!ordemProducao) {
          if (this.saveScheduladoBuscandoInformacoesOdf) {
            this.save().subscribe();
          }
          return;
        }
        this.form.get(this.formControls.idProduto).setValue(ordemProducao.idProduto);
        this.form.get(this.formControls.revisao).setValue(ordemProducao.revisao);
        this.form.get(this.formControls.numeroLote).setValue(ordemProducao.numeroLote);
        this.form.get(this.formControls.idPedido).setValue(ordemProducao.idPedido);
        this.form.get(this.formControls.numeroOdfFaturamento).setValue(ordemProducao.numeroOdfFaturamento);
        this.form.get(this.formControls.idProdutoFaturamento).setValue(ordemProducao.idProdutoFaturamento);

        if (this.saveScheduladoBuscandoInformacoesOdf) {
          this.save().subscribe();
        }
      });
  }

  private setForm(): void {
    this.form = this.formBuilder.group({});
    this.form.addControl(NaoConformidadesFormControl.id, this.formBuilder.control(null, Validators.required));
    this.form.addControl(NaoConformidadesFormControl.codigo, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.origem, this.formBuilder.control(null, Validators.required));
    this.form.addControl(NaoConformidadesFormControl.status, this.formBuilder.control({ value: null, disabled: true }));
    this.form.addControl(NaoConformidadesFormControl.idNotaFiscal, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.idNatureza, this.formBuilder.control(null, Validators.required));
    this.form.addControl(NaoConformidadesFormControl.idPessoa, this.formBuilder.control(null, Validators.required));
    this.form.addControl(NaoConformidadesFormControl.numeroOdf, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.idProduto, this.formBuilder.control(null, Validators.required));
    this.form.addControl(NaoConformidadesFormControl.revisao, this.formBuilder.control(null, Validators.min(0)));
    this.form.addControl(NaoConformidadesFormControl.equipe, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.aceitoConcessao, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.campoNf, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.dataFabricacaoLote, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.numeroLote, this.formBuilder.control(null, Validators.min(0)));
    this.form.addControl(NaoConformidadesFormControl.loteParcial, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.loteTotal, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.idCriador, this.formBuilder.control({ value: null, disabled: true }));
    this.form.addControl(NaoConformidadesFormControl.melhoriaEmPotencial, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.naoConformidadeEmPotencial, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.relatoNaoConformidade, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.rejeitado, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.retrabalhoNoCliente, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.retrabalhoPeloCliente, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.descricao, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.idPedido, this.formBuilder.control(null));

    this.naoConformidadesEditorService.form = this.form;
  }

  private populateForm(naoConformidade: NaoConformidadeModel): void {
    this.form.get(NaoConformidadesFormControl.id).setValue(naoConformidade.id, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.codigo).setValue(naoConformidade.codigo, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.origem).setValue(naoConformidade.origem, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.status).setValue(naoConformidade.status, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.idNotaFiscal).setValue(naoConformidade.idNotaFiscal, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.idNatureza).setValue(naoConformidade.idNatureza, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.idPessoa).setValue(naoConformidade.idPessoa, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.numeroOdf).setValue(naoConformidade.numeroOdf, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.idProduto).setValue(naoConformidade.idProduto, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.revisao).setValue(naoConformidade.revisao, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.equipe).setValue(naoConformidade.equipe, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.aceitoConcessao).setValue(naoConformidade.aceitoConcessao, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.campoNf).setValue(naoConformidade.campoNf, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.dataFabricacaoLote).setValue(naoConformidade.dataFabricacaoLote, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.numeroLote).setValue(naoConformidade.numeroLote, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.loteParcial).setValue(naoConformidade.loteParcial, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.loteTotal).setValue(naoConformidade.loteTotal, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.idCriador).setValue(naoConformidade.idCriador, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.melhoriaEmPotencial).setValue(naoConformidade.melhoriaEmPotencial, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.naoConformidadeEmPotencial).setValue(naoConformidade.naoConformidadeEmPotencial, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.relatoNaoConformidade).setValue(naoConformidade.relatoNaoConformidade, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.rejeitado).setValue(naoConformidade.rejeitado, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.retrabalhoNoCliente).setValue(naoConformidade.retrabalhoNoCliente, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.retrabalhoPeloCliente).setValue(naoConformidade.retrabalhoPeloCliente, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.descricao).setValue(naoConformidade.descricao, { emitEvent: false });
    this.form.get(NaoConformidadesFormControl.idPedido).setValue(naoConformidade.idPedido, { emitEvent: false });
  }

  private formActionCheck(): void {
      this.isUpdating = Boolean(this.idNaoConformidade);
  }

  private subscribeBloquearAtualizacaoNaoConformidade() {
    this.subscriptionManager.add(
      'bloquearAtualizacaoNaoConformidade',
      this.naoConformidadesEditorService.validarAtualizacaoNaoConformidade.subscribe(() => {
        if (this.naoConformidadesEditorService.somenteLeitura) {
          this.form.disable();
          this.canEditNaoConformidade = false;
        } else {
          if (this.naoConformidadesEditorService.naoConformidadeNaoConcluida) {
            this.canEditNaoConformidade = true;

            if (this.naoConformidadesEditorService.naoConformidadeTemRetrabalhoGerado) {
              this.form.disable();
              this.habilitarCamposTriviais();
            } else {
              this.form.enable();
            }
          } else {
            this.form.disable();
            this.canEditNaoConformidade = false;
          }
        }
      })
    );
  }

  private initComponent(): void {
    if (!this.isUpdating) {
      this.populateForm({
        id: UUID.UUID(),
        status: StatusNaoConformidade.Aberto,
        idCriador: this.userService.getCurrentUser().id,
      } as NaoConformidadeModel);
      this.loaded = true;
    } else {
      this.updateNaoConformidade(this.idNaoConformidade);
    }
  }

  private updateNaoConformidade(id: string): void {
    forkJoin({
      ordemRetrabalho: this.naoConformidadesEditorService.getOdfRetrabalho(id),
      operacaoRetrabalho: this.naoConformidadesEditorService.getOperacaoRetrabalho(id),
      naoConformidade:  this.naoConformidadesService.get(id)
    }).subscribe(({ naoConformidade }) => {
      this.populateForm(naoConformidade);
      this.naoConformidadesEditorService.validarAtualizacaoNaoConformidade.next();
      this.loaded = true;
    });
  }
  private estaSeTornandoOuDeixandoDeSerInspecaoEntrada(novoValor: OrigemNaoConformidades): boolean {
    return this.origem === this.origemNaoConformidadesEnum.InspecaoEntrada
    || novoValor === this.origemNaoConformidadesEnum.InspecaoEntrada;
  }

  private tornarCampoObrigatorio(formControlName:string) {
    this.form.get(formControlName).addValidators(Validators.required);
  }
  private tornarCampoNaoObrigatorio(formControlName:string) {
    this.form.get(formControlName).removeValidators(Validators.required);
  }
  private limparNotaFiscal():void {
    this.form.get(this.formControls.idNotaFiscal).setValue(null);
  }
  private subscribeOrigemValueChanges():void {
    this.subscriptionManager.add('origemValueChanges', this.form.get(this.formControls.origem).valueChanges
      .subscribe((value:OrigemNaoConformidades) => {
        if (this.estaSeTornandoOuDeixandoDeSerInspecaoEntrada(value) && this.loaded) {
          this.limparNotaFiscal();
          this.form.get(this.formControls.idPessoa).setValue(null);
          this.idPessoa = null;
        }
        this.origem = value;
        if (
          value === OrigemNaoConformidades.Cliente
          || value === OrigemNaoConformidades.InspecaoEntrada
        ) {
          this.tornarCampoObrigatorio(this.formControls.idPessoa);
        } else {
          this.tornarCampoNaoObrigatorio(this.formControls.idPessoa);
        }

        if (value === OrigemNaoConformidades.InspecaoSaida) {
          this.tornarCampoObrigatorio(this.formControls.numeroOdf);
        } else {
          this.tornarCampoNaoObrigatorio(this.formControls.numeroOdf);
        }

        if (value === OrigemNaoConformidades.InspecaoEntrada) {
          this.tornarCampoObrigatorio(this.formControls.idNotaFiscal);
        } else {
          this.tornarCampoNaoObrigatorio(this.formControls.idNotaFiscal);
        }
      }));
  }

  private habilitarCamposTriviais(): void {
    this.form.get(NaoConformidadesFormControl.idNatureza).enable({ emitEvent: false });
    this.form.get(NaoConformidadesFormControl.status).enable({ emitEvent: false });
    this.form.get(NaoConformidadesFormControl.descricao).enable({ emitEvent: false });
    this.form.get(NaoConformidadesFormControl.equipe).enable({ emitEvent: false });
    this.form.get(NaoConformidadesFormControl.naoConformidadeEmPotencial).enable({ emitEvent: false });
    this.form.get(NaoConformidadesFormControl.loteTotal).enable({ emitEvent: false });
    this.form.get(NaoConformidadesFormControl.rejeitado).enable({ emitEvent: false });
    this.form.get(NaoConformidadesFormControl.retrabalhoNoCliente).enable({ emitEvent: false });
    this.form.get(NaoConformidadesFormControl.relatoNaoConformidade).enable({ emitEvent: false });
    this.form.get(NaoConformidadesFormControl.loteParcial).enable({ emitEvent: false });
    this.form.get(NaoConformidadesFormControl.aceitoConcessao).enable({ emitEvent: false });
    this.form.get(NaoConformidadesFormControl.retrabalhoPeloCliente).enable({ emitEvent: false });
    this.form.get(NaoConformidadesFormControl.melhoriaEmPotencial).enable({ emitEvent: false });
  }
}
