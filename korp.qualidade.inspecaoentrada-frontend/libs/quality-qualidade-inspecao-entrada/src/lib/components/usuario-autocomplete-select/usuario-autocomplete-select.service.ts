import {Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IPagedResultOutputDto } from '@viasoft/common';
import { ensureTrailingSlash } from '@viasoft/http';
import { SessionService } from '../../services/session.service';
import { UsuarioOutput } from './usuario-output.class';

@Injectable()
export class UsuarioAutocompleteSelectService {
  private readonly headers = this.sessionService.defaultHttpHeaders;

  constructor(
    protected httpClient: HttpClient,
    private sessionService: SessionService) {
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

    return this.httpClient.get<IPagedResultOutputDto<UsuarioOutput>>(`${this.basePath}`, {
      params: queryParameters,
      headers: this.headers
    });
  }

  public get(id: string): Observable<UsuarioOutput> {
    return this.httpClient.get<UsuarioOutput>(`${this.basePath}/${id}`, { headers: this.headers });
  }

  private get basePath(): string {
    return `${ensureTrailingSlash(this.sessionService.currentBaseUrl)}administration/authentication/users`;
  }
}
