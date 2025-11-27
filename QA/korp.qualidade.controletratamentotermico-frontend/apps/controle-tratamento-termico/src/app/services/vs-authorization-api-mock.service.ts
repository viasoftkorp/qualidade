import { Injectable } from '@angular/core';
import { VsAuthorizationApiService } from '@viasoft/authorization-management';
import { Observable, of } from 'rxjs';

@Injectable()
export class VsAuthorizationApiMockService extends VsAuthorizationApiService {

  public addAuthorization(): Observable<any> {
    return of([]);
  }

  public getAllAuthorizations(): Observable<any> {
    return of([]);
  }

  public getAuthorization(): Observable<any> {
    return of('mock');
  }

  public updateAuthorization(): Observable<any> {
    return of([]);
  }

  public deleteAuthorization(): Observable<any> {
    return of();
  }

  public syncPermission(): Observable<any> {
    return of([]);
  }

  public resetCustomPoliciesForUser(): Observable<any> {
    return of([]);
  }

  public getPoliciesFromUser(): Observable<any> {
    return of([]);
  }

  public getUserAuthorizationsStatus(): Observable<any> {
    return of([]);
  }

  public updatePoliciesForUser(): Observable<any> {
    return of([]);
  }

  public updateAuthorizationsForUser(): Observable<any> {
    return of([]);
  }

  public getUserData(): Observable<any> {
    return of([]);
  }
}
