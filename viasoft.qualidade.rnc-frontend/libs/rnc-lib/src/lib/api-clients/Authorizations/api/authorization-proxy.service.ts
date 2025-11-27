import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { ensureTrailingSlash } from '@viasoft/http';
import { VS_BACKEND_URL } from '@viasoft/client-core';

@Injectable({
  providedIn: 'root',
})
export class AuthorizationServiceProxy {
  private readonly endpoint: string;

  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/core/authorization/permissions`;
  }

  public getAuthorizations(): Observable<string[]> {
    return this.httpClient.get<string[]>(this.endpoint);
  }
}
