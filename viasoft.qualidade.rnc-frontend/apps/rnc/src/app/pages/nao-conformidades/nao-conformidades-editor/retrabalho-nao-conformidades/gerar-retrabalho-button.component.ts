import { HttpErrorResponse } from '@angular/common/http';
import {
  Component, Input, OnDestroy, OnInit
} from '@angular/core';
import {
  NaoConformidadeValidationResult,
  MovimentacaoEstoqueProcessadaNotificationUpdate
} from '@viasoft/rnc/api-clients/Nao-Conformidades';

import {
  OrdemRetrabalhoOutput,
  GerarOrdemRetrabalhoValidationResult,
} from '@viasoft/rnc/api-clients/Nao-Conformidades/Ordem-Retrabalho';

import { StatusProducaoRetrabalho } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho';

import { finalize } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';
import { VsDialog } from '@viasoft/components';
import { DataLivelyService, MessageService, VsSubscriptionManager } from '@viasoft/common';
import { RetrabalhoNaoConformidadePolicies } from '@viasoft/rnc/app/services/authorizations/policies';
import { forkJoin } from 'rxjs';
import { OrigemNaoConformidades } from '@viasoft/rnc/api-clients/Nao-Conformidades/model/origem-nao-conformidades';
import { OrdemProducaoOutput } from '@viasoft/rnc/api-clients/Ordem-Producao/model';
import {
  GerarOdfRetrabalhoEditorModalComponent
} from './odf-retrabalho-nao-conformidades';

import {
  GerarOperacaoRetrabalhoEditorModalComponent
} from './operacao-retrabalho-nao-conformidades';
import { NaoConformidadesEditorService } from '../nao-conformidades-editor.service';
import { RetrabalhoNaoConformidadesService } from './retrabalho-nao-conformidades.service';
import { OrdemProducaoService } from '../../../shared/ordens-producao/ordem-producao.service';
import { NaoConformidadesFormControl } from '../nao-conformidades-form-control';

@Component({
  selector: 'rnc-gerar-retrabalho-button',
  templateUrl: './gerar-retrabalho-button.component.html',
  styleUrls: ['./gerar-retrabalho-button.component.scss']
})
export class GerarRetrabalhoButtonComponent implements OnInit, OnDestroy {
  @Input() public disabled = false;
  public currentButton: 'ordemProducao' | 'operacao' | 'estornoOrdemProducao';
  public retrabalhoPolicies = RetrabalhoNaoConformidadePolicies;
  public loaded = false;
  public get disableGerarOdfRetrabalho():boolean {
    return this.naoConformidadesEditorService.processando
    || (!this.form.valid && !this.naoConformidadesEditorService.naoConformidadeNaoConcluida);
  }

  public get disableGerarOperacaoRetrabalho():boolean {
    return this.naoConformidadesEditorService.processando
    || this.naoConformidadesEditorService.operacaoRetrabalho !== null;
  }
  public get canEstornarOdfRetrabalho():boolean {
    return this.naoConformidadesEditorService.origem !== OrigemNaoConformidades.InspecaoSaida
    && this.naoConformidadesEditorService.ordemRetrabalho.status === StatusProducaoRetrabalho.Aberta;
  }

  private readonly MOVIMENTACAO_ESTOQUE_PROCESSADA_NOTIFICATION_UPDATE = 'MovimentacaoEstoqueProcessada';
  private subscriptionManager = new VsSubscriptionManager();

  private get form():FormGroup {
    return this.naoConformidadesEditorService.form;
  }

  private get numeroOdf():number {
    return this.form.get(NaoConformidadesFormControl.numeroOdf).value as number;
  }

  constructor(
    private messageService:MessageService,
    public naoConformidadesEditorService:NaoConformidadesEditorService,
    private dialog: VsDialog,
    private dataLivelyService: DataLivelyService,
    private retrabalhoNaoConformidadesService: RetrabalhoNaoConformidadesService,
    private ordemProducaoService: OrdemProducaoService
  ) { }

  public ngOnInit(): void {
    this.getCurrentButton();
    this.subscribeToMovimentacaoEstoqueProcessada();
    this.subscribeGetCurrentButtonFromOdfCommand();
  }

  public ngOnDestroy(): void {
    this.retrabalhoNaoConformidadesService.onDestroy();
    this.subscriptionManager.clear();
  }

  public async gerarOdfRetrabalho():Promise<void> {
    const saveResult = await this.salvarNaoConformidade();

    const rncSalva = saveResult === NaoConformidadeValidationResult.Ok;
    const rncConcluida = !this.naoConformidadesEditorService.naoConformidadeNaoConcluida;

    if (rncSalva || rncConcluida) {
      this.naoConformidadesEditorService.processando = true;
      this.retrabalhoNaoConformidadesService.getCanGerarOdfRetrabalho(true)
        .pipe(finalize(() => {
          this.naoConformidadesEditorService.processando = false;
        })).subscribe((result: GerarOrdemRetrabalhoValidationResult) => {
          if (result === GerarOrdemRetrabalhoValidationResult.Ok) {
            this.abrirModalGeracaoRetrabalho();
          } else {
            this.messageService
              .warn(`NaoConformidades.GerarOdfRetrabalhoModal.CanGenerateValidationResult.${result}`);
          }
        });
    }
  }

  public estornarOdfRetrabalho():void {
    this.messageService.confirm('NaoConformidades.EstornarOdfRetrabalhoConfirmMessage')
      .subscribe((confirmed:boolean) => {
        if (confirmed) {
          this.naoConformidadesEditorService.processando = true;
          this.naoConformidadesEditorService.estornarOdf().subscribe({
            error: (error:HttpErrorResponse) => {
              if (error.status === 422) {
                const result = error.error as OrdemRetrabalhoOutput;
                this.messageService.warn(result.message);
              }
              this.naoConformidadesEditorService.processando = false;
            }
          });
        }
      });
  }

  public async gerarOperacaoRetrabalho():Promise<void> {
    const saveResult = await this.salvarNaoConformidade();

    const rncSalva = saveResult === NaoConformidadeValidationResult.Ok;
    const rncConcluida = !this.naoConformidadesEditorService.naoConformidadeNaoConcluida;
    if (rncSalva || rncConcluida) {
      this.abrirModalGeracaoRetrabalho();
    }
  }

  private async salvarNaoConformidade():Promise<NaoConformidadeValidationResult | null> {
    let saveResult: NaoConformidadeValidationResult | null;

    if (this.naoConformidadesEditorService.naoConformidadeNaoConcluida) {
      if (this.form.dirty) {
        saveResult = await this.naoConformidadesEditorService.save().toPromise();
      } else {
        saveResult = NaoConformidadeValidationResult.Ok;
      }
    }

    return saveResult;
  }

  private abrirModalGeracaoRetrabalho():void {
    const modal = this.currentButton === 'ordemProducao'
      ? GerarOdfRetrabalhoEditorModalComponent
      : GerarOperacaoRetrabalhoEditorModalComponent;

    this.dialog
      .open(modal, this.numeroOdf, {
        hasBackdrop: true,
        maxWidth: '60%'
      })
      .afterClosed()
      .subscribe((isGerandoOdfRetrabalho: boolean) => {
        if (this.currentButton === 'ordemProducao' && isGerandoOdfRetrabalho) {
          this.naoConformidadesEditorService.processando = true;
        } else {
          this.getCurrentButton();
        }
      });
  }

  private subscribeToMovimentacaoEstoqueProcessada() {
    this.dataLivelyService.get(this.MOVIMENTACAO_ESTOQUE_PROCESSADA_NOTIFICATION_UPDATE).subscribe(
      (notification: MovimentacaoEstoqueProcessadaNotificationUpdate) => {
        if (notification.idNaoConformidade === this.naoConformidadesEditorService.idNaoConformidade) {
          this.naoConformidadesEditorService.processando = false;
          this.loaded = false;
          this.getCurrentButton();
        }
      }
    );
  }
  private subscribeGetCurrentButtonFromOdfCommand() {
    this.subscriptionManager.add('numeroOdfAlterado', this.retrabalhoNaoConformidadesService.getCurrentButtonFromOdfCommand
      .subscribe((ordemProducao: OrdemProducaoOutput) => {
        this.getCurrentButtonFromOdf(ordemProducao);
      }));
  }
  private getCurrentButton(): void {
    this.subscriptionManager.add('get-retrabalhos-botoes',
      forkJoin({
        operacaoRetrabalho: this.naoConformidadesEditorService.getOperacaoRetrabalho(),
        ordemProducaoRetrabalho: this.naoConformidadesEditorService.getOdfRetrabalho()
      }).subscribe(({operacaoRetrabalho, ordemProducaoRetrabalho}) => {
        if (operacaoRetrabalho !== null) {
          this.currentButton = 'operacao';
          this.loaded = true;
        } else if (ordemProducaoRetrabalho !== null) {
          this.currentButton = 'estornoOrdemProducao';
          this.loaded = true;
        } else {
          this.ordemProducaoService.getByNumeroOdf(this.numeroOdf).subscribe((ordemProducao: OrdemProducaoOutput) => {
            this.getCurrentButtonFromOdf(ordemProducao);
            this.loaded = true;
          });
        }

        this.naoConformidadesEditorService.validarAtualizacaoNaoConformidade.next();
      }));
  }
  private getCurrentButtonFromOdf(ordemProducao: OrdemProducaoOutput) {
    if (!ordemProducao) {
      return;
    }
    if (ordemProducao.odfFinalizada || ordemProducao.possuiPartida) {
      this.currentButton = 'ordemProducao';
    } else {
      this.currentButton = 'operacao';
    }
    this.loaded = true;
  }
}
