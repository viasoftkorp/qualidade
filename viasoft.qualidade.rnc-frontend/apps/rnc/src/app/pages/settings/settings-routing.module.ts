import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IAuthorizationData, VsAuthorizationGuard } from '@viasoft/common';

import { SettingsComponent } from './settings.component';
import { Policies } from '../../services/authorizations/policies/policies';

const routes: Routes = [
  {
    path: '',
    component: SettingsComponent,
    children: [
      {
        path: 'naturezas',
        loadChildren: () => import('./natureza/natureza.module').then((m) => m.NaturezaModule),
        canActivate: [VsAuthorizationGuard],
        data: {
          permission: Policies.ReadNatureza,
          authBackRoute: 'nao-conformidade'
        } as IAuthorizationData
      },
      {
        path: 'causas',
        loadChildren: () => import('./causa/causa.module').then((m) => m.CausaModule),
        canActivate: [VsAuthorizationGuard],
        data: {
          permission: Policies.ReadCausa,
          authBackRoute: 'nao-conformidade'
        } as IAuthorizationData
      },
      {
        path: 'solucoes',
        loadChildren: () => import('./solucao/solucao.module').then((m) => m.SolucaoModule),
        canActivate: [VsAuthorizationGuard],
        data: {
          permission: Policies.ReadSolucao,
          authBackRoute: 'nao-conformidade'
        } as IAuthorizationData
      },
      {
        path: 'defeitos',
        loadChildren: () => import('./defeito/defeito.module').then((m) => m.DefeitoModule),
        canActivate: [VsAuthorizationGuard],
        data: {
          permission: Policies.ReadDefeito,
          authBackRoute: 'nao-conformidade'
        } as IAuthorizationData
      },
      {
        path: 'acoes-preventivas',
        loadChildren: () => import('./acao-preventiva/acao-preventiva.module').then((m) => m.AcaoPreventivaModule),
        canActivate: [VsAuthorizationGuard],
        data: {
          permission: Policies.ReadAcaoPreventiva,
          authBackRoute: 'nao-conformidade'
        } as IAuthorizationData
      },
      {
        path: 'gerais',
        loadChildren: () => import('./geral/geral.module').then((m) => m.GeralModule),
        canActivate: [VsAuthorizationGuard],
        data: {
          permission: Policies.AtualizarConfiguracoesGerais,
          authBackRoute: 'nao-conformidade'
        } as IAuthorizationData
      }
    ]
  },
  {
    path: '**',
    redirectTo: ''
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingsRoutingModule { }
