import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ensureTrailingSlash } from '@viasoft/http';
import { VsGridGetInput } from '@viasoft/components';
import { SessionService } from '../../../services/session.service';
import { GetAllProcessamentoInspecaoEntradaOutput, ProcessamentoInspecaoEntradaFilters } from '../../../tokens';

@Injectable()
export class ProcessamentoInspecaoViewService {
  constructor(
    protected httpClient: HttpClient,
    private sessionService: SessionService
  ) {
  }

  public get(
    input: VsGridGetInput, filtros: ProcessamentoInspecaoEntradaFilters, estorno: boolean
  ): Observable<GetAllProcessamentoInspecaoEntradaOutput> {
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
    if (filtros.status) {
      params = params.set('status', filtros.status);
    }
    if (filtros.numeroExecucoes) {
      params = params.set('numeroExecucoes', filtros.numeroExecucoes);
    }
    if (filtros.erro) {
      params = params.set('erro', filtros.erro);
    }
    if (filtros.resultado) {
      params = params.set('resultado', filtros.resultado);
    }
    if (filtros.quantidadeTotal) {
      params = params.set('quantidadeTotal', filtros.quantidadeTotal);
    }
    if (filtros.codigoProduto) {
      params = params.set('codigoProduto', filtros.codigoProduto);
    }
    if (filtros.notaFiscal) {
      params = params.set('notaFiscal', filtros.notaFiscal);
    }
    if (filtros.idUsuarioExecucao) {
      params = params.set('idUsuarioExecucao', filtros.idUsuarioExecucao);
    }
    if (filtros.dataExecucao) {
      params = params.set('dataExecucao', new Date(filtros.dataExecucao).toISOString());
    }
    params = params.set('estorno', estorno);

    return this.httpClient.get<GetAllProcessamentoInspecaoEntradaOutput>(this.basePath, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  public reprocessar(idSaga: string): Observable<never> {
    return this.httpClient.put<never>(`${this.basePath}/${idSaga}/reprocessar`, {}, {
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  public removeProcess(idSaga: string) {
    const route = `${this.basePath}/${idSaga}`;

    return this.httpClient.delete(
      route,
      {
        headers: this.sessionService.defaultHttpHeaders
      }
    );
  }

  public getParametroBool(parametro: string, secao: string): Promise<boolean> {
    const route = `${ensureTrailingSlash(this.sessionService.currentBaseUrl)}parametros/${parametro}/secoes/${secao}`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }).toPromise() as Promise<boolean>;
  }

  private get basePath(): string {
    return `${ensureTrailingSlash(this.sessionService.currentBaseUrl)}processamentos`;
  }
}
