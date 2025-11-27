import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { MessageService, VsAuthorizationService } from '@viasoft/common';
import { VsCompaniesDetail, VsCompanyService, ensureTrailingSlash } from '@viasoft/http';
import {
  NaoConformidadeInput,
  ReclamacoesNaoConformidadesInput,
  ReclamacoesNaoConformidadesOutput,
  NaoConformidadeValidationResult,
  StatusNaoConformidade
} from '@viasoft/rnc/api-clients/Nao-Conformidades';

import {
  OrdemRetrabalhoOutput
} from '@viasoft/rnc/api-clients/Nao-Conformidades/Ordem-Retrabalho';

import { OdfRetrabalhoProxyService } from '@viasoft/rnc/api-clients/Nao-Conformidades/Ordem-Retrabalho/api';
import {
  BehaviorSubject,
  Observable,
  Subject,
  of
} from 'rxjs';
import {
  catchError,
  finalize,
  map,
  tap
} from 'rxjs/operators';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { ConfiguracaoGeralService } from '@viasoft/rnc/app/services/configuracoes/configuracao-geral.service';
import { ConfiguracaoGeralOutput } from '@viasoft/rnc/api-clients/Configuracoes/Gerais/configuracao-geral-output';
import { OrigemNaoConformidades } from '@viasoft/rnc/api-clients/Nao-Conformidades/model/origem-nao-conformidades';
import { Policies } from '@viasoft/rnc/app/services/authorizations/policies/policies';
import { policiesToArray } from '@viasoft/rnc/app/services/authorizations/functions/policiesToArrayFuncion';
import {
  OperacaoRetrabalhoOutput,
  OperacaoRetrabalhoProxyService
} from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho';
import { NaoConformidadesService } from '../nao-conformidades.service';
import { NaoConformidadesFormControl } from './nao-conformidades-form-control';
import { UUID } from 'angular2-uuid';

@Injectable({
  providedIn: 'root'
})
export class NaoConformidadesEditorService {
  public idNaoConformidade:string | null;
  public reclamacaoAtualizada = new Subject<string>();
  public arquivoInserido = new Subject<void>();
  public naoConformidadeConcluida = new Subject<void>();
  public saveButtonClicked = new Subject<void>();
  public validarAtualizacaoNaoConformidade = new BehaviorSubject<void>(null);
  public form:FormGroup;
  public editorAction:EditorAction;
  public configuracaoGeral: ConfiguracaoGeralOutput;
  public processando = false;
  public origem: OrigemNaoConformidades;
  public formControlsDesabilitados:Array<string> = [NaoConformidadesFormControl.idCriador]
  public canShowGerarRetrabalhoButton = false;
  public permissoesNaoConformidade: Map<string, boolean>;
  public operacaoRetrabalho: OperacaoRetrabalhoOutput;
  public ordemRetrabalho: OrdemRetrabalhoOutput;
  private reclamacaoForm = new FormGroup({});
  private readonly endpoint: string;

  public get naoConformidadeAtual(): NaoConformidadeInput {
    return this.form.getRawValue() as NaoConformidadeInput;
  }

  public get naoConformidadeTemRetrabalhoGerado(): boolean {
    return Boolean(this.operacaoRetrabalho) || Boolean(this.ordemRetrabalho);
  }

  public get naoConformidadeNaoConcluida(): boolean {
    return this.naoConformidadeAtual.status !== StatusNaoConformidade.Fechado;
  }

  public get getReclamacaoForm(): FormGroup {
    return this.reclamacaoForm;
  }

  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient,
  private odfRetrabalhoProxyService: OdfRetrabalhoProxyService,
  private operacaoRetrabalhoProxyService: OperacaoRetrabalhoProxyService,
  private messageService:MessageService,
  private naoConformidadesService: NaoConformidadesService,
  private configuracaoGeralService: ConfiguracaoGeralService,
  private vsCompanyService: VsCompanyService,
  private vsAuthorizationService: VsAuthorizationService) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/gateway/nao-conformidades`;
  }

  public async onInit():Promise<void> {
    this.permissoesNaoConformidade = await this.vsAuthorizationService
      .isGrantedMap(policiesToArray(Policies.NaoConformidadePolicies));
  }

  public getOdfRetrabalho():Observable<OrdemRetrabalhoOutput | null> {
    return this.odfRetrabalhoProxyService
      .getOdf(this.naoConformidadeAtual.id)
      .pipe(tap((ordemRetrabalhoOutput: OrdemRetrabalhoOutput) => {
        this.ordemRetrabalho = ordemRetrabalhoOutput;
      }));
  }

  public getOperacaoRetrabalho():Observable<OperacaoRetrabalhoOutput> {
    return this.operacaoRetrabalhoProxyService
      .getOperacaoRetrabalho(this.naoConformidadeAtual.id)
      .pipe(tap((operacaoRetrabalho:OperacaoRetrabalhoOutput) => {
        this.operacaoRetrabalho = operacaoRetrabalho;
      }));
  }

  public onDestroy():void {
    this.resetarVariaveis();
  }

  public createReclamacao(id: string, input: ReclamacoesNaoConformidadesInput):Observable<ReclamacoesNaoConformidadesInput> {
    return this.httpClient.post<ReclamacoesNaoConformidadesInput>(`${this.endpoint}/${id}/reclamacao`, input);
  }
  public updateReclamacao(id: string, input: ReclamacoesNaoConformidadesInput):Observable<ReclamacoesNaoConformidadesInput> {
    return this.httpClient.put<ReclamacoesNaoConformidadesInput>(`${this.endpoint}/${id}/reclamacao`, input);
  }
  public getReclamacao(idNaoConformidade:string): Observable<ReclamacoesNaoConformidadesOutput> {
    return this.httpClient.get<ReclamacoesNaoConformidadesOutput>(`${this.endpoint}/${idNaoConformidade}/reclamacao`);
  }
  public estornarOdf():Observable<OrdemRetrabalhoOutput> {
    return this.odfRetrabalhoProxyService.estornarOdf(this.idNaoConformidade);
  }

  public save(): Observable<NaoConformidadeValidationResult | null> {
    const naoConformidade = this.naoConformidadeAtual;

    naoConformidade.numeroOdf = naoConformidade.numeroOdf || naoConformidade.numeroOdf === 0
      ? Number(naoConformidade.numeroOdf)
      : null;
    this.processando = true;
    if (this.editorAction === EditorAction.Create) {
      return this.naoConformidadesService.create(naoConformidade)
        .pipe(
          tap(() => {
            const reclamacao = {
              id: UUID.UUID(),
              idNaoConformidade: naoConformidade.id,
              procedentes: 0,
              improcedentes: 0,
              quantidadeLote: 0,
              quantidadeNaoConformidade: 0,
              disposicaoProdutosAprovados: 0,
              disposicaoProdutosConcessao: 0,
              rejeitado: 0,
              retrabalho: 0
            } as ReclamacoesNaoConformidadesInput;

            this.createReclamacao(reclamacao.idNaoConformidade, reclamacao).subscribe();
          }),
          finalize(() => {
            this.processando = false;
          }),
          catchError((error:HttpErrorResponse) => {
            if (error.status === 422) {
              this.messageService
                .error(`NaoConformidades.NaoConformidadesEditor.RNCInvalida.${error.error}`,
                  'NaoConformidades.NaoConformidadesEditor.RNCInvalida.Title');
            }
            return of(error.error) as Observable<NaoConformidadeValidationResult>;
          })
        );
    }

    return this.naoConformidadesService.update(naoConformidade.id, naoConformidade)
      .pipe(
        tap(() => {
          const canUpdateReclamacao = this.reclamacaoForm.valid && this.reclamacaoForm.dirty;

          if (canUpdateReclamacao) {
            const reclamacao = new ReclamacoesNaoConformidadesInput(
              naoConformidade.id, this.reclamacaoForm.getRawValue() as ReclamacoesNaoConformidadesInput);

            this.updateReclamacao(reclamacao.idNaoConformidade, reclamacao)
              .subscribe(() => {
                this.reclamacaoAtualizada.next(naoConformidade.id);
              });
          }

          this.verifyCanShowGerarRetrabalhoButton();
          this.form.markAsPristine();
        }),
        catchError((error:HttpErrorResponse) => {
          if (error.status === 422) {
            this.messageService
              .error(`NaoConformidades.NaoConformidadesEditor.RNCInvalida.${error.error}`,
                'NaoConformidades.NaoConformidadesEditor.RNCInvalida.Title');
          }
          return of(error.error) as Observable<NaoConformidadeValidationResult>;
        }),
        finalize(() => {
          this.processando = false;
        }),
      );
  }

  public changeToUpdate(idNaoConformidade:string):void {
    this.editorAction = EditorAction.Update;
    this.form.markAsPristine();
    this.idNaoConformidade = idNaoConformidade;
    this.verifyCanShowGerarRetrabalhoButton();

    this.configuracaoGeralService.get().subscribe((configuracao:ConfiguracaoGeralOutput) => {
      this.configuracaoGeral = configuracao;
    });
  }

  public mudarParaEmpresaNaoConformidade(idEmpresaNaoConformidade: string): Observable<void> {
    return this.vsCompanyService.getCompanies().pipe(map((empresas:VsCompaniesDetail) => {
      const empresa = empresas.companies
        .find((e) => e.id.toLocaleLowerCase() === idEmpresaNaoConformidade.toLocaleLowerCase());
      this.vsCompanyService.selectCompany(empresa);
    }));
  }

  public verifyCanShowGerarRetrabalhoButton():void {
    const hasNumeroOdf = Boolean(this.naoConformidadeAtual.numeroOdf);

    if (hasNumeroOdf) {
      this.canShowGerarRetrabalhoButton = true;
    } else {
      this.canShowGerarRetrabalhoButton = false;
    }
  }

  public setReclamacaoForm(form: FormGroup): void {
    this.reclamacaoForm = form;
  }

  private resetarVariaveis() {
    this.idNaoConformidade = null;
    this.arquivoInserido = new Subject<void>();
    this.naoConformidadeConcluida = new Subject<void>();
    this.saveButtonClicked = new Subject<void>();
    this.validarAtualizacaoNaoConformidade = new BehaviorSubject<void>(null);
    this.form = null;
    this.editorAction = null;
    this.configuracaoGeral = null;
    this.processando = false;
    this.origem = null;
    this.canShowGerarRetrabalhoButton = false;
    this.ordemRetrabalho = null;
    this.operacaoRetrabalho = null;
    this.reclamacaoForm = new FormGroup({});
  }
}
