import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from "@viasoft/http";
import { Observable } from 'rxjs';
import { SessionService } from '../../../../services/session.service';
import {
  FiltrosInspecaoDto,
  GetAllHistoricoInspecaoEntradaItensOutput,
  GetAllHistoricoInspecaoEntradaOutput,
  GetInspecaoEntradaItensDTO,
  InspecoesHistoricoInput
} from '../../../../tokens';

@Injectable()
export class HistoricoInspecaoViewService {
  constructor(
    protected httpClient: HttpClient,
    private sessionService: SessionService) {
  }

  public getItens(input: VsGridGetInput, dadosNota: InspecoesHistoricoInput, filtros: FiltrosInspecaoDto): Observable<GetAllHistoricoInspecaoEntradaItensOutput> {
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

    filtros.observacoesMetricas.forEach((observacao) => {
      if (observacao) {
        params = params.append('observacoesMetricas', observacao);
      }
    });

    params = params.set('recnoItemNotaFiscal', dadosNota.recno);

    const regex = new RegExp("/", 'g');
    dadosNota.lote = dadosNota.lote.replace(regex, "%3A");

    return this.httpClient.get<GetAllHistoricoInspecaoEntradaItensOutput>(`${this.basePath}/${dadosNota.notaFiscal}/lotes/${dadosNota.lote}/itens`, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  public get(input: VsGridGetInput, filtros: FiltrosInspecaoDto): Observable<GetAllHistoricoInspecaoEntradaOutput> {
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

    filtros.observacoesMetricas.forEach((observacao) => {
      if (observacao) {
        params = params.append('observacoesMetricas', observacao);
      }
    });

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
    params = params.set('codigoProduto', codigoProduto);

    return this.httpClient.get(route, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetInspecaoEntradaItensDTO>;
  }
}
