import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';

import { ensureTrailingSlash, VS_API_PREFIX } from '@viasoft/http';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { VsGridGetInput } from '@viasoft/components';

import { GetAllProcessamentoInspecaoSaidaOutput, ProcessamentoInspecaoSaidaFilters } from '../../../tokens';
import { SessionService } from '../../../services/session.service';

@Injectable()
export class ProcessamentoInspecaoViewService {
  constructor(protected httpClient: HttpClient, @Inject(VS_BACKEND_URL) private apiBaseUrl: string,
    @Inject(VS_API_PREFIX) private apiPrefix: string, private sessionService: SessionService) {
  }

  public get(input: VsGridGetInput, filtros: ProcessamentoInspecaoSaidaFilters, estorno: boolean): Observable<GetAllProcessamentoInspecaoSaidaOutput> {
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
      params = params.set('quantidade', filtros.quantidadeTotal);
    }
    if (filtros.codigoProduto) {
      params = params.set('codigoProduto', filtros.codigoProduto);
    }
    if (filtros.odf) {
      params = params.set('odf', filtros.odf);
    }
    if (filtros.idUsuarioExecucao) {
      params = params.set('idUsuarioExecucao', filtros.idUsuarioExecucao);
    }
    if (filtros.dataExecucao) {
      params = params.set('dataExecucao', new Date(filtros.dataExecucao).toISOString());
    }
    params = params.set('estorno', estorno);

    return this.httpClient.get<GetAllProcessamentoInspecaoSaidaOutput>(this.basePath(), {
      params,
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  public reprocessar(idSaga: string): Observable<never> {
    return this.httpClient.put<never>(`${this.basePath()}/${idSaga}/reprocessar`, {}, {
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  public removeProcess(idSaga: string) {
    const route = `${this.basePath()}/${idSaga}`;

    return this.httpClient.delete(
      route,
      {
        headers: this.sessionService.defaultHttpHeaders
      }
    );
  }

  private basePath(): string {
    return `${ensureTrailingSlash(this.apiBaseUrl)}${this.apiPrefix}/processamentos`;
  }

  public getParametroBool(parametro: string, secao: string): Promise<boolean> {
    const route = `${ensureTrailingSlash(this.apiBaseUrl)}${this.apiPrefix}/parametros/${parametro}/secoes/${secao}`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }).toPromise() as Promise<boolean>;
  }
}
