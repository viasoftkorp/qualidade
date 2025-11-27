import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { BehaviorSubject, Observable, Subject } from 'rxjs';

import { ensureTrailingSlash, VS_API_PREFIX } from '@viasoft/http';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { VsGridGetInput } from '@viasoft/components';

import {
  GetAllHistoricoInspecaoSaidaItensOutput,
  GetAllHistoricoInspecaoSaidaOutput,
  HistoricoInspecaoSaidaFilters,
  OrdemProducaoDTO, OrdemProducaoFilters
} from '../../../tokens';
import { SessionService } from '../../../services/session.service';
import { GetInspecaoSaidaItensDTO } from '../../../tokens/interfaces/get-inspecao-saida-itens-dto.interface';

@Injectable()
export class HistoricoInspecaoViewService {
  constructor(protected httpClient: HttpClient, @Inject(VS_BACKEND_URL) private apiBaseUrl: string,
    @Inject(VS_API_PREFIX) private apiPrefix: string, private sessionService: SessionService) {
  }

  public refreshGrid = new Subject<OrdemProducaoFilters>();

  public getItens(input: VsGridGetInput, odf: number): Observable<GetAllHistoricoInspecaoSaidaItensOutput> {
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

    return this.httpClient.get<GetAllHistoricoInspecaoSaidaItensOutput>(this.basePath() + "/" + odf + "/itens", {
      params,
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  public get(input: VsGridGetInput, filtros: HistoricoInspecaoSaidaFilters): Observable<GetAllHistoricoInspecaoSaidaOutput> {
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
    if (filtros.ordemFabricacao) {
      params = params.set('ordemFabricacao', filtros.ordemFabricacao);
    }
    if (filtros.codigoProduto) {
      params = params.set('codigoProduto', filtros.codigoProduto);
    }
    if (filtros.lote) {
      params = params.set('lote', filtros.lote);
    }

    return this.httpClient.get<GetAllHistoricoInspecaoSaidaOutput>(this.basePath(), {
      params,
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  public estornar(recnoInspecao: number): Observable<never> {
    return this.httpClient.put<never>(`${this.basePath()}/${recnoInspecao}/estornar`, {}, {
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  private basePath(): string {
    return `${ensureTrailingSlash(this.apiBaseUrl)}${this.apiPrefix}/inspecoes/historico`;
  }

  public getParametroBool(parametro: string, secao: string): Promise<boolean> {
    const route = `${ensureTrailingSlash(this.apiBaseUrl)}${this.apiPrefix}/parametros/${parametro}/secoes/${secao}`;

    return this.httpClient.get(route, {
      headers: this.sessionService.defaultHttpHeaders
    }).toPromise() as Promise<boolean>;
  }

  public getInspecaoSaidaItens(codigoInspecao: number, input: VsGridGetInput): Observable<GetInspecaoSaidaItensDTO> {
    const route = `${ensureTrailingSlash(this.apiBaseUrl)}${this.apiPrefix}/inspecoes/${codigoInspecao}/itens`;

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

    return this.httpClient.get(route, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetInspecaoSaidaItensDTO>;
  }
}
