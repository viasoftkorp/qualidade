import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ensureTrailingSlash } from '@viasoft/http';
import { IAuthorizationProvider } from '@viasoft/authorization-management';
import { SessionService } from '../services/session.service';
import { Observable } from 'rxjs';

@Injectable()
export class AuthorizationProvider implements IAuthorizationProvider {
  constructor(
    private httpClient: HttpClient,
    private session: SessionService) { }

  getPermissionsForCurrentApp(): Observable<string[]> {
    return this.httpClient.get<string[]>(
      `${ensureTrailingSlash(this.session.currentBaseUrl)}producao/gateway/tratamentos-termicos/authorizations`
    );
  }
}
