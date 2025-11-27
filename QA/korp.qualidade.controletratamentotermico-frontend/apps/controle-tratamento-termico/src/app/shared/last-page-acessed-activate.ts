import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { VsStorageService } from "@viasoft/common";
import { LAST_POLICY_KEY, LAST_ROUTE_KEY } from "../tokens";
import { NavigationService } from "../services/navigation.service";

@Injectable({
  providedIn: 'root'
})
export class LastPageAcessedActivate {
  public readonly _lastRouteKey = LAST_ROUTE_KEY;
  public readonly _lastPolicyKey = LAST_POLICY_KEY;

  constructor(
    private vsStorage: VsStorageService,
    private navigationService: NavigationService,
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (state.url !== '/' && route.routeConfig && route.routeConfig.path) {
      this.vsStorage.set(this._lastRouteKey, state.url);
      const permission = this.getLastPermission(route);
      this.vsStorage.set(this._lastPolicyKey, permission);
      this.navigationService.setRedirectDone(true)
    }
    return true;
  }

  private getLastPermission(route: ActivatedRouteSnapshot): string {
    let permission = '';
    if (route.data.permission) {
      permission = route.data.permission;
    } else if (route.firstChild) {
      permission = this.getLastPermission(route.firstChild);
    }
    return permission;
  }
}
