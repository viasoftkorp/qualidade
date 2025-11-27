import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IPagedResultOutputDto } from '@viasoft/common';
import { ensureTrailingSlash } from '@viasoft/http';
import { SessionService } from '../../services/session.service';
import { LocalOutput } from './local-output.class';

@Injectable()
export class LocalAutocompleteSelectService {
  private readonly headers = this.sessionService.defaultHttpHeaders;

  constructor(
    protected httpClient: HttpClient,
    private sessionService: SessionService
  ) {
  }

  public getList(filter: string, skipCount: number, maxResultCount: number): Observable<IPagedResultOutputDto<LocalOutput>> {
    let queryParameters = new HttpParams();

    skipCount = skipCount ?? 0;

    if (filter !== null && filter !== undefined) {
      queryParameters = queryParameters.set('filter', filter);
    }
    if (skipCount !== null && skipCount !== undefined) {
      queryParameters = queryParameters.set('skip', skipCount.toString());
    }
    if (maxResultCount !== null && maxResultCount !== undefined) {
      queryParameters = queryParameters.set('pageSize', maxResultCount.toString());
    }

    return this.httpClient.get<IPagedResultOutputDto<LocalOutput>>(this.basePath, {
      params: queryParameters,
      headers: this.headers
    });
  }

  public get(id: number): Observable<LocalOutput> {
    return this.httpClient.get<LocalOutput>(`${this.basePath}/codigo/${id}`, { headers: this.headers });
  }

  private get basePath(): string {
    return `${ensureTrailingSlash(this.sessionService.currentBaseUrl)}locais`;
  }
}
