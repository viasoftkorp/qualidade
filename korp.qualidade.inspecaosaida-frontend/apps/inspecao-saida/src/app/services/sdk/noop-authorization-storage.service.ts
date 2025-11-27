import { API_GATEWAY, VS_API_PREFIX } from '@viasoft/http';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Optional } from '@angular/core';
import { of } from 'rxjs';
import { VsAuthorizationStorageService } from '@viasoft/common';

@Injectable({
  providedIn: 'root'
})
export class NoopAuthorizationStorageService extends VsAuthorizationStorageService {
  constructor(
    @Optional() @Inject(API_GATEWAY) apiGatewayUrl: string,
    @Inject(VS_API_PREFIX) apiPrefix: string,
      http: HttpClient
  ) {
    super(apiGatewayUrl, apiPrefix, http);
  }

  public userPermissionsAfterLogin(): void {
    this.permissions = [];
    this.finishedLoginAndGetAuthorizations.next(true);
  }

  public checkUserRoot(): boolean {
    return true;
  }

  public checkUserPermissions(permissions: string[], operation: 'OR' | 'AND'): Promise<boolean> {
    return of(true).toPromise();
  }

  public checkUserPermissionsMap(permissions: string[]): Promise<Map<string, boolean>> {
    const permissionsMap = new Map<string, boolean>();
    permissions.forEach((element) => {
      permissionsMap.set(element, true);
    });
    return of(permissionsMap).toPromise();
  }
}
