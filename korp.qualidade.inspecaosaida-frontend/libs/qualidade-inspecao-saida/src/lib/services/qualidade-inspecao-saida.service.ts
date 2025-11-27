import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash, VS_API_PREFIX } from '@viasoft/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import {
  AtualizarInspecaoInput,
  EstoqueLocalPedidoVendaAlocacaoDTO,
  FinalizarInspecaoInput,
  GetAllEstoqueLocalPedidoVendaAlocacaoDTO,
  GetInspecaoSaidaDTO,
  GetOrdensProducaoDTO,
  GetPlanosInspecaoDTO,
  InspecaoSaidaDTO,
  NovaInspecaoInput,
  OrdemProducaoDTO, OrdemProducaoFilters
} from '../tokens';
import { GetInspecaoSaidaItensDTO } from '../tokens/interfaces/get-inspecao-saida-itens-dto.interface';
import { SessionService } from './session.service';

@Injectable()
export class QualidadeInspecaoSaidaService {
  public actionsTemplate: Subject<any> = new Subject();
  public ordemSelecionada = new BehaviorSubject<OrdemProducaoDTO>(null);
  public refreshOrdemGrid = new BehaviorSubject<OrdemProducaoDTO>(null);

  private get baseUrl(): string {
    return `${ensureTrailingSlash(this.apiBaseUrl)}${this.apiPrefix}`;
  }

  constructor(
    @Inject(VS_BACKEND_URL) private apiBaseUrl: string,
    @Inject(VS_API_PREFIX) private apiPrefix: string,
    private httpClient: HttpClient,
    private sessionService: SessionService
  ) { }

  private static setVsGridGetInputParams(input: VsGridGetInput): HttpParams {
    let params = new HttpParams();
    if (input.filter !== undefined && input.filter !== null && input.filter !== '') {
      params = params.set('filter', input.filter);
    }
    if (input.skipCount !== undefined && input.skipCount !== null) {
      params = params.set('skip', input.skipCount.toString());
    }
    if (input.maxResultCount !== undefined && input.maxResultCount !== null) {
      params = params.set('pageSize', input.maxResultCount.toString());
    }

    return params;
  }

  public getOrdensInspecao(input: VsGridGetInput, filtros: OrdemProducaoFilters): Observable<GetOrdensProducaoDTO> {
    const route = `${this.baseUrl}/ordens-producao`;

    let queryParams = QualidadeInspecaoSaidaService.setVsGridGetInputParams(input);

    if (filtros.lote) {
      queryParams = queryParams.set('lote', filtros.lote);
    }
    if (filtros.odf) {
      queryParams = queryParams.set('odf', filtros.odf);
    }
    if (filtros.codigoProduto) {
      queryParams = queryParams.set('codigoProduto', filtros.codigoProduto);
    }
    if (filtros.dataInicio) {
      queryParams = queryParams.set('dataInicio', new Date(filtros.dataInicio).toISOString());
    }
    if (filtros.dataEntrega) {
      queryParams = queryParams.set('dataEntrega', new Date(filtros.dataEntrega).toISOString());
    }
    if (filtros.dataEmissao) {
      queryParams = queryParams.set('dataEmissao', new Date(filtros.dataEmissao).toISOString());
    }

    return this.httpClient.get(route, {
      params: queryParams,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetOrdensProducaoDTO>;
  }

  public getInspecoesSaida(input: VsGridGetInput, odf: number): Observable<GetInspecaoSaidaDTO> {
    const route = `${this.baseUrl}/inspecoes`;

    let params = QualidadeInspecaoSaidaService.setVsGridGetInputParams(input);
    params = params.set('odf', odf);

    return this.httpClient.get(route, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetInspecaoSaidaDTO>;
  }

  public getPlanosNovaInspecao(input: VsGridGetInput, recnoProcesso: number, plano: string): Observable<GetPlanosInspecaoDTO> {
    const route = `${this.baseUrl}/planos`;

    let params = QualidadeInspecaoSaidaService.setVsGridGetInputParams(input);
    params = params.set('recnoProcesso', recnoProcesso);
    params = params.set('plano', plano);

    return this.httpClient.get(route, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetPlanosInspecaoDTO>;
  }

  public criarInspecao(input: NovaInspecaoInput): Observable<number> {
    const route = `${this.baseUrl}/inspecoes`;

    return this.httpClient.post(route, input, {
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<number>;
  }

  public getInspecaoSaida(codigoInspecao: number): Observable<InspecaoSaidaDTO> {
    const route = `${this.baseUrl}/inspecoes/${codigoInspecao}`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<InspecaoSaidaDTO>;
  }

  public getInspecaoSaidaItens(codigoInspecao: number, input: VsGridGetInput): Observable<GetInspecaoSaidaItensDTO> {
    const route = `${this.baseUrl}/inspecoes/${codigoInspecao}/itens`;
    const params = QualidadeInspecaoSaidaService.setVsGridGetInputParams(input);

    return this.httpClient.get(route, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetInspecaoSaidaItensDTO>;
  }

  public excluirInspecaoSaida(codigoInspecao: number): Observable<unknown> {
    const route = `${this.baseUrl}/inspecoes/${codigoInspecao}`;

    return this.httpClient.delete(route, {
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<unknown>;
  }

  public atualizarInspecao(input: AtualizarInspecaoInput): Observable<unknown> {
    const route = `${this.baseUrl}/inspecoes/${input.codInspecao}`;

    return this.httpClient.put(route, input, {
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<number>;
  }

  public getResultadoInspecao(codigoInspecao: number): Observable<string> {
    const route = `${this.baseUrl}/inspecoes/${codigoInspecao}/resultado`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<string>;
  }

  public finalizarInspecao(input: FinalizarInspecaoInput): Observable<unknown> {
    const route = `${this.baseUrl}/inspecoes/${input.codInspecao}/finalizar`;

    return this.httpClient.post(route, input, {
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<unknown>;
  }

  public getParametroBool(parametro: string, secao: string): Promise<boolean> {
    const route = `${this.baseUrl}/parametros/${parametro}/secoes/${secao}`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }).toPromise() as Promise<boolean>;
  }

  public getLocais(tipoLocal: string): Promise<[{ codigo: string, descricao: string }]> {
    const route = `${this.baseUrl}/locais/${tipoLocal}`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }).toPromise() as Promise<[{ codigo: string, descricao: string }]>;
  }

  public getInformacoesRNC(recnoInspecao: number): Promise<{
    idCliente: string,
    idProduto: string,
    revisao: number,
    dataFabricacaoLote?: Date,
    quantidadeTotalLote: number,
    numeroOdf: string
  }> {
    const route = `${this.baseUrl}/inspecoes/${recnoInspecao}/rnc`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }).toPromise() as any;
  }
}
