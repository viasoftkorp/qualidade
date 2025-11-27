import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {IAuthorizationData, MustBeLoggedAuthGuard} from '@viasoft/common';
import { authorizationRoutes } from '@viasoft/authorization-management';
import { IS_ON_PREMISE } from '../environments/is-on-premise.const';
import { DefaultRouteGuard } from "@viasoft/controle-tratamento-termico/app/guards/default-route-guard.service";
import { Policy } from '@viasoft/controle-tratamento-termico/app/authorization/policy';
import {policiesToArray} from "@viasoft/controle-tratamento-termico/app/authorization/functions/policiesToArrayFuncion";

const ROUTES: Routes = [
  {
    path: '',
    loadChildren: () => import('./pages/pages.module').then((m) => m.PagesModule),
    canActivate: [MustBeLoggedAuthGuard, DefaultRouteGuard],
    data: { permission: policiesToArray(Policy), permissionOperator: 'OR' } as IAuthorizationData
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(IS_ON_PREMISE ? ROUTES : [...authorizationRoutes, ...ROUTES])],
  exports: [RouterModule]
})
export class AppRoutingModule {}
