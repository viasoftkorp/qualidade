import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IAuthorizationData, VsAuthorizationGuard } from '@viasoft/common';
import { SettingsPolicies } from '../services/authorizations/policies/settings-policies';
import { NaoConformidadePolicies } from '../services/authorizations/policies/nao-conformidade-policies';
import { policiesToArray } from '../services/authorizations/functions/policiesToArrayFuncion';

const routes: Routes = [
  {
    path: 'configuracoes',
    loadChildren: () => import('./settings/settings.module').then((m) => m.SettingsModule),
    canActivate: [VsAuthorizationGuard],
    data: {
      permission: policiesToArray(SettingsPolicies),
      permissionOperator: 'OR',
      authBackRoute: 'nao-conformidades'
    } as IAuthorizationData
  },
  {
    path: 'nao-conformidades',
    loadChildren: () => import('./nao-conformidades/nao-conformidades.module').then((m) => m.NaoConformidadesModule),
    canActivate: [VsAuthorizationGuard],
    data: {
      permission: [NaoConformidadePolicies.ReadNaoConformidade],
      permissionOperator: 'OR',
      authBackRoute: 'configuracoes'
    } as IAuthorizationData
  },
  {
    path: '**', redirectTo: 'nao-conformidades'
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule {}
