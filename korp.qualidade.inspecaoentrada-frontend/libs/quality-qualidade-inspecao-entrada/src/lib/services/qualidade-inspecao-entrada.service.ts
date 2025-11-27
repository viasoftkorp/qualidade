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
  NotaFiscalDTO,
  NotaFiscalFilters,
  NovaInspecaoInput,
  PedidoVendaInput,
} from '../tokens';
import { PlanoAmostragem } from '../tokens/interfaces/planos-amostragem-dto.interface';
import { SessionService } from './session.service';

@Injectable()
export class QualidadeInspecaoEntradaService {
  public actionsTemplate: Subject<any> = new Subject();
  public pesquisaAlterada = new BehaviorSubject<string>('');
  public notaFiscalSelecionada = new BehaviorSubject<NotaFiscalDTO>(null);
  public refreshNotaFiscalGrid = new BehaviorSubject<NotaFiscalDTO>(null);

  private get baseUrl(): string {
    return `${ensureTrailingSlash(this.sessionService.currentBaseUrl)}`;
  }

  constructor(
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

  public getNotasFiscais(input: VsGridGetInput, filtros: NotaFiscalFilters): Observable<GetNotasFiscaisDTO> {
    const route = `${this.baseUrl}notas-fiscais`;

    let queryParams = QualidadeInspecaoEntradaService.setVsGridGetInputParams(input);

    if (filtros.lote) {
      queryParams = queryParams.set('lote', filtros.lote);
    }
    if (filtros.notaFiscal) {
      queryParams = queryParams.set('notaFiscal', filtros.notaFiscal);
    }
    if (filtros.codigoProduto) {
      queryParams = queryParams.set('codigoProduto', filtros.codigoProduto);
    }
    if (filtros.fornecedor) {
      queryParams = queryParams.set('fornecedor', filtros.fornecedor);
    }
    if (filtros.dataEntrada) {
      queryParams = queryParams.set('dataEntrega', new Date(filtros.dataEntrada).toISOString());
    }

    return this.httpClient.get(route, {
      params: queryParams,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetNotasFiscaisDTO>;
  }

  public getInspecoesEntrada(input: VsGridGetInput, notaFiscal: number, lote: string): Observable<GetInspecaoEntradaDTO> {
    const route = `${this.baseUrl}inspecoes/${notaFiscal}/lotes/${lote}`;

    const params = QualidadeInspecaoEntradaService.setVsGridGetInputParams(input);

    return this.httpClient.get(route, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetInspecaoEntradaDTO>;
  }

  public getPlanosNovaInspecao(input: VsGridGetInput, codigoPlano: number, codigoProduto: string): Observable<GetPlanosInspecaoDTO> {
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

  public getInformacoesRNC(recnoInspecao: number, codigoProduto: string): Promise<{
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
}
