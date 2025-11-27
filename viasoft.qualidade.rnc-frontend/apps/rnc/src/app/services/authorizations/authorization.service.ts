import { Injectable } from '@angular/core';
import { IAuthorizationProvider } from '@viasoft/authorization-management';
import { Observable } from 'rxjs';
import { AuthorizationServiceProxy } from '@viasoft/rnc/api-clients/Authorizations/api/authorization-proxy.service';

@Injectable()
export class AuthorizationService implements IAuthorizationProvider {
  protected cache: Observable<string[]>;

  constructor(private readonly service: AuthorizationServiceProxy) {}

  public getPermissionsForCurrentApp(): Observable<string[]> {
    if (!this.cache) {
      this.cache = this.service.getAuthorizations();
    }
    return this.cache;
  }
}
