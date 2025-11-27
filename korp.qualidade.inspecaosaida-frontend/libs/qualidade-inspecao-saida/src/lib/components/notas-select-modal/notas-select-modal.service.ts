import {Inject, Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {VS_BACKEND_URL} from "@viasoft/client-core";
import {ensureTrailingSlash, VS_API_PREFIX} from "@viasoft/http";
import {SessionService} from "../../services/session.service";
import {Observable} from "rxjs";
import {IPagedResultOutputDto} from "@viasoft/common";
import {NotaFiscalDto} from "./notas-select-modal.component";

@Injectable()
export class NotasSelectModalService {

  constructor(protected httpClient: HttpClient, @Inject(VS_BACKEND_URL) private apiBaseUrl: string,
              @Inject(VS_API_PREFIX) private apiPrefix: string, private sessionService: SessionService) {
  }

  public getList(codigoInspecao: number, filter: string, sorting: string, skipCount: number, maxResultCount: number): Observable<IPagedResultOutputDto<NotaFiscalDto>> {
    let queryParameters = new HttpParams();

    skipCount = skipCount ?? 0;

    if (filter !== null && filter !== undefined) {
      queryParameters = queryParameters.set('advancedFilter', filter);
    }
    if (sorting !== null && sorting !== undefined) {
      queryParameters = queryParameters.set('sorting', sorting);
    }
    if (skipCount !== null && skipCount !== undefined) {
      queryParameters = queryParameters.set('skip', skipCount);
    }
    if (maxResultCount !== null && maxResultCount !== undefined) {
      queryParameters = queryParameters.set('pageSize', maxResultCount);
    }

    return this.httpClient.get<IPagedResultOutputDto<NotaFiscalDto>>(this.basePath(codigoInspecao), {
      params: queryParameters,
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  private basePath(codigoInspecao: number): string {
    return `${ensureTrailingSlash(this.apiBaseUrl)}${this.apiPrefix}/inspecoes/${codigoInspecao}/notas`;
  }
}
