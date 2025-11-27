import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';

import { IPagedResultOutputDto } from '@viasoft/common';
import { ensureTrailingSlash, VS_API_PREFIX } from '@viasoft/http';
import { VS_BACKEND_URL } from '@viasoft/client-core';

import { SessionService } from '../../services/session.service';
import { UsuarioOutput } from './usuario-autocomplete-select.component';

@Injectable()
export class UsuarioAutocompleteSelectService {
  constructor(protected httpClient: HttpClient, @Inject(VS_BACKEND_URL) private apiBaseUrl: string,
              @Inject(VS_API_PREFIX) private apiPrefix: string, private sessionService: SessionService) {
  }

  public getList(filter: string, skipCount: number, maxResultCount: number): Observable<IPagedResultOutputDto<UsuarioOutput>> {
    let queryParameters = new HttpParams();

    skipCount = skipCount ?? 0;

    if (filter !== null && filter !== undefined) {
      queryParameters = queryParameters.set('filter', filter);
    }
    if (skipCount !== null && skipCount !== undefined) {
      queryParameters = queryParameters.set('skipCount', skipCount.toString());
    }
    if (maxResultCount !== null && maxResultCount !== undefined) {
      queryParameters = queryParameters.set('maxResultCount', maxResultCount.toString());
    }

    return this.httpClient.get<IPagedResultOutputDto<UsuarioOutput>>(`${this.basePath()}`, {
      params: queryParameters,
      headers: this.sessionService.defaultHttpHeaders
    });
  }

  public get(id: string): Observable<UsuarioOutput> {
    return this.httpClient.get<UsuarioOutput>(`${this.basePath()}/${id}`, { headers: this.sessionService.defaultHttpHeaders });
  }

  private basePath(): string {
    return `${ensureTrailingSlash(this.apiBaseUrl)}administration/authentication/users`;
  }
}
