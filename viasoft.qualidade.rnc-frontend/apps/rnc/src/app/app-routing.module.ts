import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IAuthorizationData, MustBeLoggedAuthGuard } from '@viasoft/common';
import { authorizationRoutes } from '@viasoft/authorization-management';
import { IS_ON_PREMISE } from '../environments/is-on-premise.const';
import { DefaultRouteGuard } from './guards/default-route-guard.service';
import { policiesToArray } from './services/authorizations/functions/policiesToArrayFuncion';
import { Policies } from './services/authorizations/policies/policies';

const ROUTES: Routes = [

  ...authorizationRoutes,
  {
    path: '',
    loadChildren: () => import('./pages/pages.module').then((m) => m.PagesModule),
    canActivate: [MustBeLoggedAuthGuard, DefaultRouteGuard],
    data: { permission: policiesToArray(Policies), permissionOperator: 'OR' } as IAuthorizationData
  },
  {
    path: '**',
    redirectTo: ''
  }
];

@NgModule({
  imports: [RouterModule.forRoot(IS_ON_PREMISE ? ROUTES : [...authorizationRoutes, ...ROUTES], { enableTracing: false })],
  exports: [RouterModule]
})
export class AppRoutingModule {}
