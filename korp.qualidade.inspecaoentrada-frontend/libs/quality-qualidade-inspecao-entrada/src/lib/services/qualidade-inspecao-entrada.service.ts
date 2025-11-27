import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import {
  AtualizarInspecaoInput,
  EstoqueLocalPedidoVendaAlocacaoDTO,
  FinalizarInspecaoInput,
  GetAllEstoqueLocalPedidoVendaAlocacaoDTO,
  GetInspecaoEntradaDTO,
  GetInspecaoEntradaItensDTO,
  GetNotasFiscaisDTO,
  GetPlanosInspecaoDTO,
  InspecaoEntradaDTO,
  NotaFiscalDadosAdicionaisDTO,
  NotaFiscalDTO,
  FiltrosInspecaoDto,
  NovaInspecaoInput,
  PedidoVendaInput,
} from '../tokens';
import { PlanoAmostragem } from '../tokens/interfaces/planos-amostragem-dto.interface';
import { SessionService } from './session.service';
import { VsStorageService } from '@viasoft/common';

@Injectable()
export class QualidadeInspecaoEntradaService {
  public notaFiscalSelecionada = new BehaviorSubject<NotaFiscalDTO>(null);
  public notaFiscalHistoricoSelecionada = new BehaviorSubject<NotaFiscalDTO>(null);
  public refreshNotaFiscalGrid = new Subject<void>();
  public refreshHistoricoGrid = new Subject<void>();
  public refreshInspecoesNotaFiscalGrid = new Subject<void>();
  public refreshInspecoesHistoricoGrid = new Subject<void>();
  private readonly filtrosCacheKey = '06332A18-D9A4-45B8-8FA1-4D4CC43ECC49';

  private get baseUrl(): string {
    return `${ensureTrailingSlash(this.sessionService.currentBaseUrl)}`;
  }

  constructor(
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

  public getNotasFiscais(input: VsGridGetInput, filtros: FiltrosInspecaoDto): Observable<GetNotasFiscaisDTO> {
    const route = `${this.baseUrl}notas-fiscais`;

    let queryParams = QualidadeInspecaoEntradaService.setVsGridGetInputParams(input);

    filtros.observacoesMetricas.forEach((observacao) => {
      if (observacao) {
        queryParams = queryParams.append('observacoesMetricas', observacao);
      }
    });

    return this.httpClient.get(route, {
      params: queryParams,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetNotasFiscaisDTO>;
  }

  public getInspecoesEntrada(input: VsGridGetInput, filtros: FiltrosInspecaoDto, notaFiscal: number, lote: string): Observable<GetInspecaoEntradaDTO> {
    const regex = new RegExp("/", 'g');
    lote = lote.replace(regex, "%3A");
    const route = `${this.baseUrl}inspecoes/${notaFiscal}/lotes/${lote}`;

    let params = QualidadeInspecaoEntradaService.setVsGridGetInputParams(input);

    filtros.observacoesMetricas.forEach((observacao) => {
      if (observacao) {
        params = params.append('observacoesMetricas', observacao);
      }
    });

    return this.httpClient.get(route, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetInspecaoEntradaDTO>;
  }

  public getPlanosNovaInspecao(input: VsGridGetInput, codigoPlano: string, codigoProduto: string): Observable<GetPlanosInspecaoDTO> {
    const route = `${this.baseUrl}planos/${codigoPlano}`;

    let params = QualidadeInspecaoEntradaService.setVsGridGetInputParams(input);
    params = params.set('codigoProduto', codigoProduto);

    return this.httpClient.get(route, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetPlanosInspecaoDTO>;
  }

  public criarInspecao(input: NovaInspecaoInput): Observable<number> {
    const route = `${this.baseUrl}inspecoes`;

    return this.httpClient.post(route, input, {
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<number>;
  }

  public getInspecaoEntrada(codigoInspecao: number): Observable<InspecaoEntradaDTO> {
    const route = `${this.baseUrl}inspecoes/${codigoInspecao}`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<InspecaoEntradaDTO>;
  }

  public getInspecaoEntradaItens(codigoInspecao: number, codigoProduto: string, input: VsGridGetInput): Observable<GetInspecaoEntradaItensDTO> {
    const route = `${this.baseUrl}inspecoes/${codigoInspecao}/itens`;
    let params = QualidadeInspecaoEntradaService.setVsGridGetInputParams(input);
    params = params.set('codigoProduto', codigoProduto);

    return this.httpClient.get(route, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetInspecaoEntradaItensDTO>;
  }

  public excluirInspecaoEntrada(codigoInspecao: number): Observable<unknown> {
    const route = `${this.baseUrl}inspecoes/${codigoInspecao}`;

    return this.httpClient.delete(route, {
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<unknown>;
  }

  public atualizarInspecao(input: AtualizarInspecaoInput): Observable<unknown> {
    const route = `${this.baseUrl}inspecoes`;

    return this.httpClient.put(route, input, {
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<number>;
  }

  public finalizarInspecao(input: FinalizarInspecaoInput): Observable<unknown> {
    const route = `${this.baseUrl}inspecoes/${input.codigoInspecao}/finalizar`;

    return this.httpClient.post(route, input, {
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<number>;
  }

  public getResultadoInspecao(codigoInspecao: number): Observable<string> {
    const route = `${this.baseUrl}inspecoes/${codigoInspecao}/resultado/`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<string>;
  }

  public getLocais(tipoLocal, codigoProduto: string): Promise<[{ codigo: string, descricao: string }]> {
    const route = `${this.baseUrl}locais/${tipoLocal}?codigoProduto=${codigoProduto}`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }).toPromise() as Promise<[{ codigo: string, descricao: string }]>;
  }

  public getValorParametro(chaveParametro: string, secao: string): Promise<boolean> {
    const route = `${this.baseUrl}parametros/${chaveParametro}/secoes/${secao}`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }).toPromise() as Promise<boolean>;
  }

  public getInspecaoEntradaPedidoVenda(input: VsGridGetInput, dadosInspecao: PedidoVendaInput): Observable<GetAllEstoqueLocalPedidoVendaAlocacaoDTO> {
    const route = `${this.baseUrl}inspecoes/${dadosInspecao.recnoInspecao}/pedidos-venda`;
    let params = QualidadeInspecaoEntradaService.setVsGridGetInputParams(input);
    params = params.set('codigoProduto', dadosInspecao.codigoProduto);
    params = params.set('lote', dadosInspecao.lote);

    return this.httpClient.get(route, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetAllEstoqueLocalPedidoVendaAlocacaoDTO>;
  }

  public getQuantidadeTotalAlocadaPedido(recnoInspecao: number): Promise<{ quantidadeTotalAlocada: number, quantidadeReprovada: number, quantidadeAprovada: number }> {
    const route = `${this.baseUrl}inspecoes/${recnoInspecao}/pedidos-venda/quantidade-alocada`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }).toPromise() as Promise<{ quantidadeTotalAlocada: number, quantidadeReprovada: number, quantidadeAprovada: number }>;
  }

  public atualizarDistribuicaoInspecaoEstoquePedidoVenda(recnoInspecao: number, input: EstoqueLocalPedidoVendaAlocacaoDTO): Promise<any> {
    const route = `${this.baseUrl}inspecoes/${recnoInspecao}/pedidos-venda/${input.id}`;
    return this.httpClient.put(route, input, {
      headers: this.sessionService.defaultHttpHeaders
    }).toPromise();
  }

  public getInformacoesRNC(recnoInspecao: number, codigoProduto: string, codigoFornecedor: string): Promise<{
    idNotaFiscal: string,
    idFornecedor: string,
    numeroNota: string,
    numeroLote: string,
    idProduto: string,
    revisao: number,
    dataFabricacaoLote?: Date,
    quantidadeTotalLote: number
  }> {
    let params = new HttpParams();
    params = params.set('codigoProduto', codigoProduto);
    params = params.set('codigoFornecedor', codigoFornecedor);
    const route = `${this.baseUrl}inspecoes/${recnoInspecao}/rnc`;
    return this.httpClient.get(route, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    }).toPromise() as any;
  }

  public getFaixaInspecao(quantidade: number) {
    let params = new HttpParams();
    params = params.set('quantidade', quantidade);

    return this.httpClient.get<PlanoAmostragem>(`${this.baseUrl}planos-amostragem/faixas`, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    }).toPromise();
  }

  public updateNotaFiscalDadosAdicionais(idNotaFiscal: string, nota: NotaFiscalDadosAdicionaisDTO): Observable<never> {
    return this.httpClient.put<never>(`${this.baseUrl}notas-fiscais/${idNotaFiscal}/dados-adicionais`, nota, {
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  public imprimirInspecaoSaida(codigo: number) : Observable<unknown> {
    const route = `${this.baseUrl}inspecoes/${codigo}/imprimir`;

    return this.httpClient.get<ArrayBuffer>(route, {
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<unknown>;
  }
}
