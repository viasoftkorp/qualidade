import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {
  DataLivelyService,
  MessageService,
  VsUserService,
  VsSubscriptionManager
} from '@viasoft/common';
import { VsDialog, VsMaskBase, VsSelectOption } from '@viasoft/components';
import {
  ConclusoesNaoConformidadesInput,
  NaoConformidadeModel,
  OrigemNaoConformidades,
  StatusNaoConformidade,
  NaoConformidadeValidationResult,
  NaoConformidadeViewInseridaNotificationUpdate,
} from '@viasoft/rnc/api-clients/Nao-Conformidades';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { UUID } from 'angular2-uuid';
import { ConfiguracaoGeralOutput } from '@viasoft/rnc/api-clients/Configuracoes/Gerais/configuracao-geral-output';
import { ConfiguracaoGeralService } from '@viasoft/rnc/app/services/configuracoes/configuracao-geral.service';
import { Observable, forkJoin, of } from 'rxjs';
import { finalize, tap } from 'rxjs/operators';
import { OrdemProducaoOutput } from '@viasoft/rnc/api-clients/Ordem-Producao/model';
import { VsCompanyService } from '@viasoft/http';
import { getMascaraCampoNumerico } from '@viasoft/rnc/app/tokens/consts/mask-decimal-place';
import { Policies } from '@viasoft/rnc/app/services/authorizations/policies';
import { NaoConformidadesFormControl } from './nao-conformidades-form-control';
import { NaoConformidadesService } from '../nao-conformidades.service';
import {
  ConclusaoNaoConformidadesEditorModalComponent,
  ConclusaoNaoConformidadesService
} from './conclusao-nao-conformidades';
import { NaoConformidadesEditorService } from './nao-conformidades-editor.service';
import { RelatorioNaoConformidadesService } from './relatorio-nao-conformidades/relatorio-nao-conformidades.service';
import { OrdemProducaoService } from '../../shared/ordens-producao/ordem-producao.service';
import { RetrabalhoNaoConformidadesService } from './retrabalho-nao-conformidades/retrabalho-nao-conformidades.service';

@Component({
  selector: 'rnc-nao-conformidades-editor',
  templateUrl: './nao-conformidades-editor.component.html',
  styleUrls: ['./nao-conformidades-editor.component.scss'],
})
export class NaoConformidadesEditorComponent implements OnInit, OnDestroy {
  public origemNaoConformidadesEnum = OrigemNaoConformidades;
  public statusNaoConformidade = StatusNaoConformidade;
  public form: FormGroup = new FormGroup({});
  public formControls = NaoConformidadesFormControl;
  public loaded: boolean;
  public canEditNaoConformidade = true;
  public idPessoa:string;
  public buscandoInformacoesOdf = false;
  public saveScheduladoBuscandoInformacoesOdf = false;
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
    { name: StatusNaoConformidade[0], value: StatusNaoConformidade.Aberto },
    { name: StatusNaoConformidade[1], value: StatusNaoConformidade.Pendente }
  ];
  public editorActionEnum = EditorAction;
  private subscriptionManager: VsSubscriptionManager = new VsSubscriptionManager();
  private numeroOdf: number;
  private readonly NAO_CONFORMIDADE_VIEW_INSERIDA_NOTIFICATION_UPDATE = 'NaoConformidadeViewInseridaNotificationUpdate';
  private readonly NAO_CONFORMIDADE_VIEW_ATUALIZADA_NOTIFICATION_UPDATE = 'NaoConformidadeViewAtualizadaNotificationUpdate';

  public get disableGerarRetrabalhoButton(): boolean {
    return this.naoConformidadesEditorService.naoConformidadeAtual.status === StatusNaoConformidade.Fechado;
  }

  public get hasFornecedor():boolean {
    return !!(this.form.get(this.formControls.idPessoa).value as string);
  }

  public get editorAction(): EditorAction {
    return this.naoConformidadesEditorService.editorAction;
  }

  public get naoConformidadeId(): string {
    return this.naoConformidadesEditorService.idNaoConformidade;
  }

  public get configuracaoGeral(): ConfiguracaoGeralOutput {
    return this.naoConformidadesEditorService.configuracaoGeral;
  }

  public get showOperacaoRetrabalho():boolean {
    return this.naoConformidadesEditorService.operacaoRetrabalho != null;
  }

  public get showOrdemRetrabalho():boolean {
    return this.naoConformidadesEditorService.ordemRetrabalho != null;
  }

  public get origem():OrigemNaoConformidades {
    return this.naoConformidadesEditorService.origem;
  }

  public get mascaraCampoNumerico():VsMaskBase {
    return getMascaraCampoNumerico(0, false, '');
  }

  public get canShowGerarRetrabalhoButton():boolean {
    return this.naoConformidadesEditorService.canShowGerarRetrabalhoButton;
  }

  public get canShowEstornarConclusaoButton():boolean {
    const temPermissaoEstornarConclusaoNaoConformidade = this.naoConformidadesEditorService
      .permissoesNaoConformidade?.get(Policies.NaoConformidadePolicies.EstornarConclusaoNaoConformidade);

    const result = this.naoConformidadesEditorService.naoConformidadeAtual.status === this.statusNaoConformidade.Fechado
      && temPermissaoEstornarConclusaoNaoConformidade;

    return result;
  }

  public get canShowConcluirButton():boolean {
    const temPermissaoConcluirNaoConformidade = this.naoConformidadesEditorService
      .permissoesNaoConformidade?.get(Policies.NaoConformidadePolicies.ConcluirNaoConformidade);

    const result = this.naoConformidadesEditorService.naoConformidadeAtual.status !== this.statusNaoConformidade.Fechado
      && temPermissaoConcluirNaoConformidade;

    return result;
  }

  private get currentCompanyId():string {
    return this.companyService.currentCompany.id.toLocaleLowerCase();
  }

  constructor(
    public naoConformidadesEditorService: NaoConformidadesEditorService,
    private formBuilder: FormBuilder,
    private naoConformidadesService: NaoConformidadesService,
    private route: ActivatedRoute,
    private router: Router,
    private dialog: VsDialog,
    private userService: VsUserService,
    private relatorioService: RelatorioNaoConformidadesService,
    private messageService: MessageService,
    private ordemProducaoService: OrdemProducaoService,
    private dataLivelyService: DataLivelyService,
    private configuracaoGeralService: ConfiguracaoGeralService,
    private retrabalhoNaoConformidadesService: RetrabalhoNaoConformidadesService,
    private companyService: VsCompanyService,
    private conclusaoNaoConformidadesService: ConclusaoNaoConformidadesService
  ) {
    this.formInit();
  }

  public async ngOnInit(): Promise<void> {
    await this.naoConformidadesEditorService.onInit();
    this.naoConformidadesEditorService.editorAction = this.getEditorAction();
    this.initComponent();
    this.subscribeOrigemValueChanges();
    this.subscribeToRncInserida();
    this.subscribeToRncAtualizada();
    this.subscribeBloquearAtualizacaoNaoConformidade();
  }
  public ngOnDestroy(): void {
    this.subscriptionManager.clear();
    this.dataLivelyService.remove(this.NAO_CONFORMIDADE_VIEW_INSERIDA_NOTIFICATION_UPDATE);
    this.dataLivelyService.remove(this.NAO_CONFORMIDADE_VIEW_ATUALIZADA_NOTIFICATION_UPDATE);
    this.naoConformidadesEditorService.onDestroy();
  }
  public canSave(): boolean {
    return this.form.valid
      && this.naoConformidadesEditorService.getReclamacaoForm.valid
      && (this.form.dirty || this.naoConformidadesEditorService.getReclamacaoForm.dirty)
      && this.canEditNaoConformidade
      && !this.naoConformidadesEditorService.processando;
  }

  public salvarNaoConformidade():void {
    this.save().subscribe();
  }

  public concluir(): void {
    const conclusaoData = <ConclusoesNaoConformidadesInput>{
      idNaoConformidade: this.naoConformidadeId,
    };

    this.dialog
      .open(ConclusaoNaoConformidadesEditorModalComponent,
        new EditorModalData(EditorAction.Create, conclusaoData), { maxWidth: '30%' })
      .afterClosed()
      .subscribe((data: ConclusoesNaoConformidadesInput) => {
        if (data) {
          this.atualizarNaoConformidade(this.naoConformidadeId).then(() => {
            this.naoConformidadesEditorService.naoConformidadeConcluida.next();
            this.atualizarStatusRetrabalhos();
          });
        }
      });
  }

  public estornarConclusao(): void {
    this.messageService.confirm('NaoConformidades.DesejaEstornarConclusaoConfirmMessage')
      .subscribe((confirmed) => {
        if (!confirmed) {
          return;
        }
        this.conclusaoNaoConformidadesService.estornarConclusao().subscribe(() => {
          this.atualizarNaoConformidade(this.naoConformidadeId).then(() => {
            this.atualizarStatusRetrabalhos();
          });
        });
      });
  }

  public async imprimir(): Promise<void> {
    let saveResult = NaoConformidadeValidationResult.Ok;

    if (this.naoConformidadesEditorService.naoConformidadeNaoConcluida && this.form.dirty) {
      saveResult = await this.naoConformidadesEditorService.save().toPromise();
    }

    if (saveResult !== NaoConformidadeValidationResult.Ok) {
      return;
    }

    this.naoConformidadesEditorService.processando = true;
    this.subscriptionManager.add(
      'imprimir',
      this.relatorioService.imprimirRelatorio(this.naoConformidadeId).subscribe(
        () => {
          this.naoConformidadesEditorService.processando = false;
        },
        (error: HttpErrorResponse) => {
          this.naoConformidadesEditorService.processando = false;
          if (error.status === HttpStatusCode.UnprocessableEntity) {
            this.messageService.error(`${error.error}`);
          }
        }
      )
    );
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
    const numeroOdf = this.form.get(this.formControls.numeroOdf).value as string;

    if (!numeroOdf || Number(numeroOdf) === this.numeroOdf) {
      return;
    }
    this.numeroOdf = Number(numeroOdf);
    this.buscandoInformacoesOdf = true;

    this.ordemProducaoService.getByNumeroOdf(this.numeroOdf)
      .pipe(finalize(() => {
        this.buscandoInformacoesOdf = false;
      }))
      .subscribe((ordemProducao: OrdemProducaoOutput) => {
        this.buscandoInformacoesOdf = false;
        if (this.editorAction === EditorAction.Update) {
          this.retrabalhoNaoConformidadesService.getCurrentButtonFromOdfCommand.next(ordemProducao);
        }
        if (!ordemProducao) {
          if (this.saveScheduladoBuscandoInformacoesOdf) {
            this.save().subscribe();
          }
          return;
        }
        this.form.get(this.formControls.idProduto).setValue(ordemProducao.idProduto);
        this.form.get(this.formControls.revisao).setValue(ordemProducao.revisao);
        this.form.get(this.formControls.numeroLote).setValue(ordemProducao.numeroLote);
        this.form.get(this.formControls.numeroPedido).setValue(ordemProducao.numeroPedido);
        this.form.get(this.formControls.numeroOdfFaturamento).setValue(ordemProducao.numeroOdfFaturamento);
        this.form.get(this.formControls.idProdutoFaturamento).setValue(ordemProducao.idProdutoFaturamento);

        const origem = this.form.get(this.formControls.origem).value as OrigemNaoConformidades;

        if (origem !== OrigemNaoConformidades.InspecaoEntrada) {
          this.form.get(this.formControls.idPessoa).setValue(ordemProducao.idCliente);
        }

        if (this.saveScheduladoBuscandoInformacoesOdf) {
          this.save().subscribe();
        }
      });
  }

  private save(): Observable<NaoConformidadeValidationResult | null> {
    this.naoConformidadesEditorService.saveButtonClicked.next();
    if (!this.canSave()) {
      return of(null) as Observable<null>;
    }
    if (this.buscandoInformacoesOdf) {
      this.saveScheduladoBuscandoInformacoesOdf = true;
      return of(null) as Observable<null>;
    }
    return this.naoConformidadesEditorService.save();
  }

  private initComponent():void {
    this.naoConformidadesEditorService.idNaoConformidade = null;

    if (this.editorAction === EditorAction.Create) {
      this.populateForm({
        id: UUID.UUID(),
        status: StatusNaoConformidade.Aberto,
        idCriador: this.userService.getCurrentUser().id,
      } as NaoConformidadeModel);
      this.loaded = true;
    } else {
      this.naoConformidadesEditorService.idNaoConformidade = this.route.snapshot.paramMap.get('id');

      forkJoin({
        naoConformidade: this.naoConformidadesService.get(this.naoConformidadeId),
        configuracaoGeral: this.configuracaoGeralService.get()
      }).subscribe(({ naoConformidade, configuracaoGeral }) => {
        this.naoConformidadesEditorService.configuracaoGeral = configuracaoGeral;
        const isEmpresaAtualNaoConformidade = this.currentCompanyId === naoConformidade.companyId.toLocaleLowerCase();

        if (!isEmpresaAtualNaoConformidade) {
          this.messageService.confirm('NaoConformidades.NaoConformidadesEditor.DesejaTrocarEmpresaConfirmMessage', null)
            .subscribe((mudarCompany: boolean) => {
              if (mudarCompany) {
                this.naoConformidadesEditorService.mudarParaEmpresaNaoConformidade(naoConformidade.companyId)
                  .subscribe(() => {
                    this.populateForm(naoConformidade);

                    this.naoConformidadesEditorService.verifyCanShowGerarRetrabalhoButton();
                    this.loaded = true;
                  });
              } else {
                this.router.navigate(['rnc', 'nao-conformidades']);
              }
            });
          return;
        }

        this.populateForm(naoConformidade);
        this.naoConformidadesEditorService.verifyCanShowGerarRetrabalhoButton();
        this.loaded = true;
      });
    }
  }

  private estaSeTornandoOuDeixandoDeSerInspecaoEntrada(novoValor: OrigemNaoConformidades): boolean {
    return this.naoConformidadesEditorService.origem === this.origemNaoConformidadesEnum.InspecaoEntrada
    || novoValor === this.origemNaoConformidadesEnum.InspecaoEntrada;
  }

  private formInit() {
    this.form = this.formBuilder.group({});
    this.form.addControl(NaoConformidadesFormControl.id, this.formBuilder.control(null, Validators.required));
    this.form.addControl(NaoConformidadesFormControl.codigo, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.origem, this.formBuilder.control(null, Validators.required));
    this.form.addControl(NaoConformidadesFormControl.status, this.formBuilder.control({ value: null, disabled: true }));
    this.form.addControl(NaoConformidadesFormControl.idNotaFiscal, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.idNatureza, this.formBuilder.control(null, Validators.required));
    this.form.addControl(NaoConformidadesFormControl.idPessoa, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.numeroOdf, this.formBuilder.control(null, Validators.maxLength(9)));
    this.form.addControl(NaoConformidadesFormControl.idProduto, this.formBuilder.control(null, Validators.required));
    this.form.addControl(NaoConformidadesFormControl.revisao, this.formBuilder.control(null, Validators.min(0)));
    this.form.addControl(NaoConformidadesFormControl.equipe, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.aceitoConcessao, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.campoNf, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.dataFabricacaoLote, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.numeroLote, this.formBuilder.control(null, Validators.min(0)));
    this.form.addControl(NaoConformidadesFormControl.loteParcial, this.formBuilder.control(false));
    this.form.addControl(NaoConformidadesFormControl.loteTotal, this.formBuilder.control(false));
    this.form.addControl(NaoConformidadesFormControl.idCriador, this.formBuilder.control([{ value: null, disabled: true }]));
    this.form.addControl(NaoConformidadesFormControl.melhoriaEmPotencial, this.formBuilder.control(false));
    this.form.addControl(NaoConformidadesFormControl.naoConformidadeEmPotencial, this.formBuilder.control(false));
    this.form.addControl(NaoConformidadesFormControl.relatoNaoConformidade, this.formBuilder.control(false));
    this.form.addControl(NaoConformidadesFormControl.rejeitado, this.formBuilder.control(false));
    this.form.addControl(NaoConformidadesFormControl.retrabalhoNoCliente, this.formBuilder.control(false));
    this.form.addControl(NaoConformidadesFormControl.retrabalhoPeloCliente, this.formBuilder.control(false));
    this.form.addControl(NaoConformidadesFormControl.descricao, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.numeroPedido, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.numeroOdfFaturamento, this.formBuilder.control(null));
    this.form.addControl(NaoConformidadesFormControl.idProdutoFaturamento, this.formBuilder.control(null));
    this.form.addControl(
      NaoConformidadesFormControl.dataCriacao, this.formBuilder.control([{ value: null, disabled: true }])
    );

    this.naoConformidadesEditorService.form = this.form;
  }

  private populateForm(naoConformidade: NaoConformidadeModel) {
    this.idPessoa = naoConformidade.idPessoa;
    this.form.get(NaoConformidadesFormControl.id).setValue(naoConformidade.id);
    this.form.get(NaoConformidadesFormControl.codigo).setValue(naoConformidade.codigo);
    this.form.get(NaoConformidadesFormControl.origem).setValue(naoConformidade.origem);
    this.form.get(NaoConformidadesFormControl.status).setValue(naoConformidade.status);
    this.form.get(NaoConformidadesFormControl.idNotaFiscal).setValue(naoConformidade.idNotaFiscal);
    this.form.get(NaoConformidadesFormControl.idNatureza).setValue(naoConformidade.idNatureza);
    this.form.get(NaoConformidadesFormControl.idPessoa).setValue(naoConformidade.idPessoa);
    this.form.get(NaoConformidadesFormControl.numeroOdf).setValue(naoConformidade.numeroOdf?.toString());
    this.form.get(NaoConformidadesFormControl.idProduto).setValue(naoConformidade.idProduto);
    this.form.get(NaoConformidadesFormControl.revisao).setValue(naoConformidade.revisao);
    this.form.get(NaoConformidadesFormControl.equipe).setValue(naoConformidade.equipe);
    this.form.get(NaoConformidadesFormControl.aceitoConcessao).setValue(naoConformidade.aceitoConcessao);
    this.form.get(NaoConformidadesFormControl.campoNf).setValue(naoConformidade.campoNf);
    this.form.get(NaoConformidadesFormControl.dataFabricacaoLote).setValue(naoConformidade.dataFabricacaoLote);
    this.form.get(NaoConformidadesFormControl.numeroLote).setValue(naoConformidade.numeroLote);
    this.form.get(NaoConformidadesFormControl.loteParcial).setValue(naoConformidade.loteParcial);
    this.form.get(NaoConformidadesFormControl.loteTotal).setValue(naoConformidade.loteTotal);
    this.form.get(NaoConformidadesFormControl.idCriador).setValue(naoConformidade.idCriador);
    this.form.get(NaoConformidadesFormControl.melhoriaEmPotencial).setValue(naoConformidade.melhoriaEmPotencial);
    this.form
      .get(NaoConformidadesFormControl.naoConformidadeEmPotencial)
      .setValue(naoConformidade.naoConformidadeEmPotencial);
    this.form.get(NaoConformidadesFormControl.relatoNaoConformidade).setValue(naoConformidade.relatoNaoConformidade);
    this.form.get(NaoConformidadesFormControl.rejeitado).setValue(naoConformidade.rejeitado);
    this.form.get(NaoConformidadesFormControl.retrabalhoNoCliente).setValue(naoConformidade.retrabalhoNoCliente);
    this.form.get(NaoConformidadesFormControl.retrabalhoPeloCliente).setValue(naoConformidade.retrabalhoPeloCliente);
    this.form.get(NaoConformidadesFormControl.descricao).setValue(naoConformidade.descricao);
    this.form.get(NaoConformidadesFormControl.numeroPedido).setValue(naoConformidade.numeroPedido);
    this.form.get(NaoConformidadesFormControl.numeroOdfFaturamento).setValue(naoConformidade.numeroOdfFaturamento);
    this.form.get(NaoConformidadesFormControl.idProdutoFaturamento).setValue(naoConformidade.idProdutoFaturamento);
    this.form.get(NaoConformidadesFormControl.dataCriacao).setValue(naoConformidade.dataCriacao);
  }

  private subscribeBloquearAtualizacaoNaoConformidade() {
    this.subscriptionManager.add(
      'bloquearAtualizacaoNaoConformidade',
      this.naoConformidadesEditorService.validarAtualizacaoNaoConformidade.subscribe(() => {
        if (this.naoConformidadesEditorService.naoConformidadeNaoConcluida) {
          this.canEditNaoConformidade = true;

          if (this.naoConformidadesEditorService.naoConformidadeTemRetrabalhoGerado) {
            this.form.disable({ emitEvent: false });
            this.habilitarCamposTriviais();
          } else {
            this.form.enable({ emitEvent: false });
            this.naoConformidadesEditorService.formControlsDesabilitados.forEach((formControlName: string) => {
              this.form.get(formControlName).disable();
            });
          }
        } else {
          this.form.disable({ emitEvent: false });
          this.canEditNaoConformidade = false;
        }
      })
    );
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
        if (this.estaSeTornandoOuDeixandoDeSerInspecaoEntrada(value)) {
          this.limparNotaFiscal();
          this.form.get(this.formControls.idPessoa).setValue(null);
          this.idPessoa = null;
        }
        this.naoConformidadesEditorService.origem = value;
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

  private subscribeToRncAtualizada() {
    this.dataLivelyService.get(this.NAO_CONFORMIDADE_VIEW_ATUALIZADA_NOTIFICATION_UPDATE).subscribe(
      (notification: NaoConformidadeViewInseridaNotificationUpdate) => {
        if (notification.idNaoConformidade === this.naoConformidadesEditorService.idNaoConformidade) {
          this.atualizarStatusRetrabalhos();
          this.naoConformidadesEditorService.processando = false;
        }
      }
    );
  }
  private subscribeToRncInserida() {
    this.dataLivelyService.get(this.NAO_CONFORMIDADE_VIEW_INSERIDA_NOTIFICATION_UPDATE).subscribe(
      () => {
        if (this.editorAction === EditorAction.Update) {
          return;
        }
        this.naoConformidadesEditorService.processando = false;
        const id = this.form.get(this.formControls.id).value as string;
        this.router.navigate(['nao-conformidades', id]).then(() => {
          this.naoConformidadesEditorService.changeToUpdate(this.route.snapshot.paramMap.get('id'));

          this.atualizarNaoConformidade(id);

          this.retrabalhoNaoConformidadesService.getCanGerarOdfRetrabalho(false).subscribe();
        });
      }
    );
  }
  private getEditorAction():EditorAction {
    if (this.route.snapshot.paramMap.get('id') === 'new') {
      return EditorAction.Create;
    }
    return EditorAction.Update;
  }

  private atualizarNaoConformidade(idNaoConformidade: string): Promise<NaoConformidadeModel> {
    return this.naoConformidadesService.get(idNaoConformidade)
      .pipe(tap((naoConformidade) => this.populateForm(naoConformidade))).toPromise();
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

  private atualizarStatusRetrabalhos(): void {
    this.subscriptionManager.add('get-retrabalhos-cabecalho',
      forkJoin({
        operacaoRetrabalho: this.naoConformidadesEditorService.getOperacaoRetrabalho(),
        ordemProducaoRetrabalho: this.naoConformidadesEditorService.getOdfRetrabalho()
      }).subscribe(() => {
        this.naoConformidadesEditorService.validarAtualizacaoNaoConformidade.next();
      }));
  }
 }
