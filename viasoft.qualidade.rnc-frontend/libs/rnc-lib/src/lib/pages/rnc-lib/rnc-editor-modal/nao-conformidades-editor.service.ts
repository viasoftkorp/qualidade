import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { ensureTrailingSlash } from '@viasoft/http';
import {
  ConclusoesNaoConformidadesInput,
  ConclusoesNaoConformidadesOutput,
  NaoConformidadeInput,
  OperacaoRetrabalhoOutput,
  OrdemRetrabalhoOutput,
  ReclamacoesNaoConformidadesInput,
  ReclamacoesNaoConformidadesOutput,
  StatusNaoConformidade
} from '../../../api-clients/Nao-Conformidades';

import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { FormGroup } from '@angular/forms';
import { OdfRetrabalhoProxyService } from '../../../api-clients/Nao-Conformidades/Ordem-Retrabalho';
import { OperacaoRetrabalhoProxyService } from '../../../api-clients/Nao-Conformidades/Operacao-Retrabalho';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class NaoConformidadesEditorService {
  private readonly endpoint: string;
  public reclamacaoAtualizada = new Subject<string>();
  public arquivoInserido = new Subject<void>();
  public naoConformidadeConcluida = new Subject<void>();
  public saveButtonClicked = new Subject<void>();
  public validarAtualizacaoNaoConformidade = new BehaviorSubject<void>(null);
  public form: FormGroup;
  public somenteLeitura: boolean;
  private operacaoRetrabalho: OperacaoRetrabalhoOutput;
  private ordemRetrabalho: OrdemRetrabalhoOutput;
  private reclamacaoForm = new FormGroup({});

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

  constructor(
    @Inject(VS_BACKEND_URL) protected gateway: string,
    protected httpClient: HttpClient,
    private odfRetrabalhoProxyService: OdfRetrabalhoProxyService,
    private operacaoRetrabalhoProxyService: OperacaoRetrabalhoProxyService
  ) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/core/nao-conformidades`;
  }

  public setReclamacaoForm(form: FormGroup): void {
    this.reclamacaoForm = form;
  }

  public getOdfRetrabalho(id: string):Observable<OrdemRetrabalhoOutput | null> {
    return this.odfRetrabalhoProxyService
      .getOdf(id)
      .pipe(tap((ordemRetrabalhoOutput: OrdemRetrabalhoOutput) => {
        this.ordemRetrabalho = ordemRetrabalhoOutput;
      }));
  }

  public getOperacaoRetrabalho(id: string):Observable<OperacaoRetrabalhoOutput> {
    return this.operacaoRetrabalhoProxyService
      .getOperacaoRetrabalho(id)
      .pipe(tap((operacaoRetrabalho: OperacaoRetrabalhoOutput) => {
        this.operacaoRetrabalho = operacaoRetrabalho;
      }));
  }

  public concluir(id: string, input: ConclusoesNaoConformidadesInput): Observable<ConclusoesNaoConformidadesInput> {
    return this.httpClient.post<ConclusoesNaoConformidadesInput>(`${this.endpoint}/${id}/conclusao`, input);
  }
  public getConclusao(idNaoConformidade:string): Observable<ConclusoesNaoConformidadesOutput> {
    return this.httpClient.get<ConclusoesNaoConformidadesOutput>(`${this.endpoint}/${idNaoConformidade}/conclusao`);
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
  public calcularCicloTempo(idNaoConformidade:string): Observable<number> {
    return this.httpClient
      .get<number>(`${this.endpoint}/${idNaoConformidade}/calcular-ciclo-tempo`);
  }
}
