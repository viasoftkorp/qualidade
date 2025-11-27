import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash, VS_API_PREFIX } from '@viasoft/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import {
  AtualizarInspecaoInput, FiltrosInspecaoDto,
  FinalizarInspecaoInput,
  GetInspecaoSaidaDTO,
  GetOrdensProducaoDTO,
  GetPlanosInspecaoDTO,
  HistoricoInspecaoSaidaOutput,
  InspecaoSaidaDTO,
  NovaInspecaoInput,
  OrdemProducaoDTO,
  LocalOutput,
  ProcessoEngenhariaOutput
} from '../tokens';
import { GetInspecaoSaidaItensDTO } from '../tokens/interfaces/get-inspecao-saida-itens-dto.interface';
import { SessionService } from './session.service';
import { VsStorageService } from '@viasoft/common';

@Injectable()
export class QualidadeInspecaoSaidaService {
  public ordemSelecionada = new BehaviorSubject<OrdemProducaoDTO>(null);
  public ordemHistoricoSelecionada = new BehaviorSubject<HistoricoInspecaoSaidaOutput>(null);
  public refreshOrdemGrid = new Subject<void>();
  public refreshHistoricoGrid = new Subject<void>();
  public refreshInspecoesOrdemGrid = new Subject<void>();
  public refreshInspecoesHistoricoGrid = new Subject<void>();
  private readonly filtrosCacheKey = '637A6627-C725-4124-9FCB-5FD7523BBDDB';

  private get baseUrl(): string {
    return `${ensureTrailingSlash(this.apiBaseUrl)}${this.apiPrefix}`;
  }

  constructor(
    @Inject(VS_BACKEND_URL) private readonly apiBaseUrl: string,
    @Inject(VS_API_PREFIX) private readonly apiPrefix: string,
    private readonly httpClient: HttpClient,
    private readonly sessionService: SessionService,
    private readonly storageService: VsStorageService
  ) { }

  public setFiltros(filtros: FiltrosInspecaoDto): void {
    const filtrosSerializados = JSON.stringify(filtros);
    this.storageService.set(this.filtrosCacheKey, filtrosSerializados);
  }

  public getFiltros(): FiltrosInspecaoDto {
    const filtrosSerializados = this.storageService.get(this.filtrosCacheKey);

    if (filtrosSerializados) {
      return JSON.parse(filtrosSerializados) as FiltrosInspecaoDto;
    }

    return new FiltrosInspecaoDto();
  }

  private static setVsGridGetInputParams(input: VsGridGetInput): HttpParams {
    let params = new HttpParams();
    if (input.filter !== undefined && input.filter !== null && input.filter !== '') {
      params = params.set('filter', input.filter);
    }
    if (input.advancedFilter !== undefined && input.advancedFilter !== null && input.advancedFilter !== '') {
        params = params.set('advancedFilter', input.advancedFilter);
    }
    if (input.sorting !== undefined && input.sorting !== null && input.sorting !== '') {
        params = params.set('sorting', input.sorting);
    }
    if (input.skipCount !== undefined && input.skipCount !== null) {
      params = params.set('skip', input.skipCount.toString());
    }
    if (input.maxResultCount !== undefined && input.maxResultCount !== null) {
      params = params.set('pageSize', input.maxResultCount.toString());
    }

    return params;
  }

  public getOrdensInspecao(input: VsGridGetInput, filtros: FiltrosInspecaoDto): Observable<GetOrdensProducaoDTO> {
    const route = `${this.baseUrl}/ordens-producao`;

    let queryParams = QualidadeInspecaoSaidaService.setVsGridGetInputParams(input);

    filtros.observacoesMetricas.forEach((observacao) => {
      if (observacao) {
        queryParams = queryParams.append('observacoesMetricas', observacao);
      }
    });

    return this.httpClient.get(route, {
      params: queryParams,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetOrdensProducaoDTO>;
  }

  public getInspecoesSaida(input: VsGridGetInput, odf: number, filtros: FiltrosInspecaoDto): Observable<GetInspecaoSaidaDTO> {
    const route = `${this.baseUrl}/inspecoes`;

    let params = QualidadeInspecaoSaidaService.setVsGridGetInputParams(input);
    params = params.set('odf', odf);

    filtros.observacoesMetricas.forEach((observacao) => {
      if (observacao) {
        params = params.append('observacoesMetricas', observacao);
      }
    });

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

  public criarInspecao(input: NovaInspecaoInput): Observable<InspecaoSaidaDTO> {
    const route = `${this.baseUrl}/inspecoes`;

    return this.httpClient.post(route, input, {
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<InspecaoSaidaDTO>;
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

  public getLocais(tipoLocal: string): Promise<LocalOutput[]> {
    const route = `${this.baseUrl}/locais/${tipoLocal}`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }).toPromise() as Promise<LocalOutput[]>;
  }

  public getProcessoEngenharia(codigoProduto: string): Promise<ProcessoEngenhariaOutput> {
    const route = `${this.baseUrl}/produtos/${codigoProduto}/processo`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }).toPromise() as Promise<ProcessoEngenhariaOutput>;
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

  public imprimirInspecaoSaida(codigo: number) : Observable<unknown> {
    const route = `${this.baseUrl}/inspecoes/${codigo}/imprimir`;

    return this.httpClient.get<ArrayBuffer>(route, {
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<unknown>;
  }
}
