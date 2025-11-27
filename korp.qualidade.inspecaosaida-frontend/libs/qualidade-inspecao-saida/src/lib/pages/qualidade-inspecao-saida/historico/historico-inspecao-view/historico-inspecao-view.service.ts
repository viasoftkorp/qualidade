import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ensureTrailingSlash, VS_API_PREFIX } from '@viasoft/http';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { VsGridGetInput } from '@viasoft/components';
import {
  FiltrosInspecaoDto,
  GetAllHistoricoInspecaoSaidaItensOutput,
  GetAllHistoricoInspecaoSaidaOutput
} from '../../../../tokens';
import { SessionService } from '../../../../services/session.service';
import { GetInspecaoSaidaItensDTO } from '../../../../tokens/interfaces/get-inspecao-saida-itens-dto.interface';

@Injectable()
export class HistoricoInspecaoViewService {
  constructor(protected httpClient: HttpClient, @Inject(VS_BACKEND_URL) private apiBaseUrl: string,
    @Inject(VS_API_PREFIX) private apiPrefix: string, private sessionService: SessionService) {
  }

  public getItens(input: VsGridGetInput, odf: number, codigoInspecao: number, filtros: FiltrosInspecaoDto): Observable<GetAllHistoricoInspecaoSaidaItensOutput> {
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

    params = params.set('codigoInspecao', codigoInspecao.toString());

    return this.httpClient.get<GetAllHistoricoInspecaoSaidaItensOutput>(this.basePath() + "/" + odf + "/itens", {
      params,
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  public get(input: VsGridGetInput, filtros: FiltrosInspecaoDto): Observable<GetAllHistoricoInspecaoSaidaOutput> {
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

    return this.httpClient.get(route, {
      params,
      headers: this.sessionService.defaultHttpHeaders
    }) as Observable<GetInspecaoSaidaItensDTO>;
  }
}
