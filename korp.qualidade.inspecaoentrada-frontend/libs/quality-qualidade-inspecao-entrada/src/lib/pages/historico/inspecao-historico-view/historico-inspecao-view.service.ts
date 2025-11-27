import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { VsGridGetInput } from '@viasoft/components';
import { Observable, Subject } from 'rxjs';
import { SessionService } from '../../../services/session.service';
import {
  GetAllHistoricoInspecaoEntradaItensOutput,
  GetAllHistoricoInspecaoEntradaOutput,
  GetInspecaoEntradaItensDTO,
  HistoricoInspecaoEntradaFilters,
  InspecoesHistoricoInput, NotaFiscalFilters
} from '../../../tokens';
import { ensureTrailingSlash } from "@viasoft/http";

@Injectable()
export class HistoricoInspecaoViewService {
  constructor(
    protected httpClient: HttpClient,
    private sessionService: SessionService) {
  }

  public refreshGrid = new Subject<HistoricoInspecaoEntradaFilters>();

  public getItens(input: VsGridGetInput, dadosNota: InspecoesHistoricoInput): Observable<GetAllHistoricoInspecaoEntradaItensOutput> {
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

    return this.httpClient.get<GetAllHistoricoInspecaoEntradaItensOutput>(`${this.basePath}/${dadosNota.notaFiscal}/lotes/${dadosNota.lote}/itens`, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  public get(input: VsGridGetInput, filtros: HistoricoInspecaoEntradaFilters): Observable<GetAllHistoricoInspecaoEntradaOutput> {
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
    if (filtros.notaFiscal) {
      params = params.set('notaFiscal', filtros.notaFiscal);
    }
    if (filtros.codigoProduto) {
      params = params.set('codigoProduto', filtros.codigoProduto);
    }
    if (filtros.lote) {
      params = params.set('lote', filtros.lote);
    }

    return this.httpClient.get<GetAllHistoricoInspecaoEntradaOutput>(this.basePath, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  public estornar(recnoInspecao: number): Observable<never> {
    return this.httpClient.put<never>(`${this.basePath}/${recnoInspecao}/estornar`, {}, {
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  public getParametroBool(parametro: string, secao: string): Promise<boolean> {
    const route = `${ensureTrailingSlash(this.sessionService.currentBaseUrl)}parametros/${parametro}/secoes/${secao}`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }).toPromise() as Promise<boolean>;
  }

  private get basePath(): string {
    return `${ensureTrailingSlash(this.sessionService.currentBaseUrl)}inspecoes/historico`;
  }

  public getInspecaoEntradaItens(codigoInspecao: number, codigoProduto: string, input: VsGridGetInput): Observable<GetInspecaoEntradaItensDTO> {
    const route = `${ensureTrailingSlash(this.sessionService.currentBaseUrl)}inspecoes/${codigoInspecao}/itens`;
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
    params = params.set('codigoProduto', codigoProduto);

    return this.httpClient.get(route, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetInspecaoEntradaItensDTO>;
  }
}
