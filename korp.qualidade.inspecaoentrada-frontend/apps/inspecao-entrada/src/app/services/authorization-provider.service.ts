import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { IAuthorizationProvider } from '@viasoft/authorization-management';
import { tap } from 'rxjs/operators';
import { ensureTrailingSlash } from '@viasoft/http';
import { SessionService } from '@viasoft/inspecao-entrada/app/services/session.service';

@Injectable()
export class AuthorizationProvider implements IAuthorizationProvider {
  protected policiesCache: string[];

  constructor(
    protected httpClient: HttpClient,
    private sessionService: SessionService
  ) {}

  public getPermissionsForCurrentApp(): Observable<string[]> {
    if (this.policiesCache) {
      return of(this.policiesCache);
    }
    return this.httpClient.get<string[]>(
      `${ensureTrailingSlash(this.sessionService.currentBaseUrl)}authorization/permissions`
    ).pipe(
      tap((policies: string[]) => {
        this.policiesCache = policies;
      }),
    );
  }
}
