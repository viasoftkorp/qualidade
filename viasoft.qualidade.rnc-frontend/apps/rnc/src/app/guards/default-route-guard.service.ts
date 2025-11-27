import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';

import { Observable } from 'rxjs';

import { MessageService, VsAuthorizationService } from '@viasoft/common';

@Injectable({
  providedIn: 'root',
})
export class DefaultRouteGuard implements CanActivate {
  constructor(private authorizationService: VsAuthorizationService, private messageService: MessageService) {
  }

  public canActivate(next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.authorizationService.isGranted(next.data.permission, next.data.permissionOperator)
      .then(async (isGranted: boolean) => {
        if (!isGranted) {
          await this.messageService.error('Rnc.NenhumaPermissaoAplicativo').toPromise();
          window.location.href = window.origin;
        }

        return isGranted;
      });
  }
}
